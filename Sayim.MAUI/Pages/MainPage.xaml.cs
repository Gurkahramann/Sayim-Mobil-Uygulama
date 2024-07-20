using Sayim.ApiClient;
using Sayim.ApiClient.Models.ApiModels;
using System.Collections.ObjectModel;
using Microsoft.Maui.Handlers;
using Android.Views.InputMethods;
namespace Sayim.MAUI.Pages
{
    [QueryProperty(nameof(KullaniciKodu), "kullaniciKodu")]
    public partial class MainPage : ContentPage
    {
        CancellationToken token = new();
        private readonly ApiClientService _apiClientService;
        private ObservableCollection<Tablo> _seriNoList;
        private int _sira = 1;
        private string _kullaniciKodu;
        public string KullaniciKodu
        {
            get => _kullaniciKodu;
            set
            {
                _kullaniciKodu = value;
                OnPropertyChanged();
            }
        }
        private bool _kaydetButonunaBasildiMi;

        [Obsolete]
        public MainPage(ApiClientService apiClientService)
        {
            InitializeComponent();
            personEntry.IsEnabled = false;
            NavigationPage.SetHasBackButton(this, false);
            warehouseAddressEntry.IsEnabled = false;
            warehouseNoEntry.IsEnabled = false;
            counterEntry.IsEnabled=false;
            _apiClientService = apiClientService;
            _seriNoList = new ObservableCollection<Tablo>();
            listView.ItemsSource = _seriNoList;
            MessagingCenter.Subscribe<AmbarNoDetailsPage, string>(this, "UpdateWarehouseNo", (sender, ambarNo) =>
            {
                warehouseNoEntry.Text = ambarNo;
            });
            MessagingCenter.Subscribe<AmbarAdresDetailsPage,string>(this,"UpdateWarehouseAddress", (sender, ambarAddress) =>
            {
                warehouseAddressEntry.Text = ambarAddress;
            });
            MessagingCenter.Subscribe<PersonelDetailsPage, string>(this, "PersonelSelected", (sender, personelAdi) =>
            {
                personEntry.Text = personelAdi;
            });
            // Add the keyboard handling to the counterEntry
            EntryHandler.Mapper.AppendToMapping("NoKeyboardEntry", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.ShowSoftInputOnFocus = false;
                handler.PlatformView.EditorAction += (sender, args) =>
                {
                    if (args.ActionId == ImeAction.Done)
                    {
                        var inputMethodManager = (InputMethodManager)handler.PlatformView.Context.GetSystemService(Android.Content.Context.InputMethodService);
                        inputMethodManager.HideSoftInputFromWindow(handler.PlatformView.WindowToken, HideSoftInputFlags.None);
                    }
                };
#endif

            });
            // Radiobutton CheckedChanged event handlerlarını ekle
            rdbTopNo.CheckedChanged += OnRadioButtonCheckedChanged;
            rdbSeriNo.CheckedChanged += OnRadioButtonCheckedChanged;
            rdbRefakatKartNo.CheckedChanged += OnRadioButtonCheckedChanged;
        }
        private async void OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (rdbTopNo.IsChecked || rdbSeriNo.IsChecked || rdbRefakatKartNo.IsChecked)
            {
                counterEntry.IsEnabled = true;
                counterEntry.Focus();
                    
            }
            else
            {
                counterEntry.IsEnabled = false;
            }
        }
        public void SetKullaniciKodu(string kullaniciKodu)
        {
            _kullaniciKodu = kullaniciKodu;
        }
        private async void OnPersonelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PersonelDetailsPage(_apiClientService, _kullaniciKodu));
        }


        private async void OnAmbarNoButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AmbarNoDetailsPage(_apiClientService));
        }

        private async void OnAmbarAdresButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(warehouseNoEntry.Text))
            {
                Vibration.Vibrate(500);
                await DisplayAlert("Uyarı", "Ambar No Seçilmelidir", "OK");
            }
            else
            {
                await Navigation.PushAsync(new AmbarAdresDetailsPage(_apiClientService, warehouseNoEntry.Text));
            }
                
        }


        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var selectedItem = button?.CommandParameter as Tablo;
            if (selectedItem == null)
            {
                Vibration.Vibrate(500);
                await DisplayAlert("Uyarı", "Lütfen silmek için bir öğe seçin.", "OK");
                return;
            }

            string secilen = selectedItem.SeriNo;
            string filter = $"SeriNo='{secilen}'"; // Burada filtreyi uygun formatta oluşturuyoruz

            if (await _apiClientService.CountAsync("StokSayim", "SeriNo", filter, true) > 0)
            {
                Vibration.Vibrate(500);
                await DisplayAlert("Uyarı", "Silmek İstediğiniz SeriNo Kaydedilmiş. \nSilme İşlemi Yapamazsınız.!!", "OK");
                return;
            }
            counterLabel.Text=(_seriNoList.Count-1).ToString();
            _seriNoList.Remove(selectedItem);
            UpdateSira();
        }



        private async void OnKaydetButtonClicked(object sender, EventArgs e)
        {

            _kaydetButonunaBasildiMi = true;

            // Liste boşsa uyarı mesajı göster ve işlemi durdur
            if (_seriNoList.Count <= 0)
            {
                Vibration.Vibrate(500);
                await DisplayAlert("Uyarı", "Seri No Giriniz.!!!", "OK");
                counterEntry.Focus();
                return;
            }

            // Kontrol işlemlerini gerçekleştir
            if (!await KontrolAsync())
            {
                _kaydetButonunaBasildiMi = false;
                return;
            }
            int sayimAnaId = await _apiClientService.GetMaxSayimNoAsync();
            bool isKaydet = await DisplayAlert("Kayıt Mesaj", "Kaydetmek İstiyor Musunuz??", "Yes", "No");
            if (!isKaydet)
            {
                _kaydetButonunaBasildiMi = false;
                return;
            }
            var stokSayimAna = new StokSayimAna
            {
                SayimNo = sayimAnaId,
                SayimTarihi = DateTime.Now,
                SayimYapan = personEntry.Text,
                SayimiKapat = false,
                Aciklama = null
            };
            if (!await _apiClientService.InsertStokSayimAnaAsync(stokSayimAna))
            {
                _kaydetButonunaBasildiMi = false;
                return;
            }
            // Her bir sayım detayını kaydet
            foreach (var item in _seriNoList)
            {
                var stokSayim = new StokSayim
                {
                    SayimNo = sayimAnaId,
                    SiraNo = item.Sira,
                    StokTuru = null,
                    StokKodu = null,
                    LotNo = null,
                    SeriNo = item.SeriNo,
                    Miktar1 = item.Miktar,
                    OlcuBirimi1 = null,
                    Miktar2 = 0,
                    OlcuBirimi2 = null,
                    Miktar3 = 0,
                    OlcuBirimi3 = null,
                    AmbarNo = warehouseNoEntry.Text,
                    AmbarAdresi = warehouseAddressEntry.Text,
                    DokumanNo = null
                };

                if (!await _apiClientService.InsertStokSayimAsync(stokSayim))
                {
                    _kaydetButonunaBasildiMi = false;
                    return;
                }
            }
            Vibration.Vibrate(500);
            await DisplayAlert("Başarılı", $"Sayım Numarası : {sayimAnaId} olarak kaydedildi.", "OK");
            AlanBosalt();
            _kaydetButonunaBasildiMi = false;
        }
        private void AlanBosalt()
        {
            // Alanları temizlemek için gerekli işlemler
            personEntry.Text = string.Empty;
            warehouseNoEntry.Text = string.Empty;
            warehouseAddressEntry.Text = string.Empty;
            counterEntry.Text = string.Empty;
            _seriNoList.Clear();
            counterLabel.Text = "0";
            rdbTopNo.IsEnabled = true;
            rdbSeriNo.IsEnabled = true;
            rdbRefakatKartNo.IsEnabled = true;
            _sira = 1;
        }
        private async void OnCikisButtonClicked(object sender, EventArgs e)
        {
            bool isExit=await DisplayAlert("Uyarı", "Uygulamadan Çıkış Yapmak İstiyor Musunuz??", "Yes", "No");
            if(isExit)
            {
                Application.Current.Quit();
            }
            else
            {
                return;
            }
            
        }
        private void UpdateSira()
        {
            int orderSira = 1;
            foreach (var item in _seriNoList)
            {
                item.Sira = orderSira++;
            }
            listView.ItemsSource = null;
            listView.ItemsSource = _seriNoList;
        }
        private async void OnCounterEntryCompleted(object sender, EventArgs e)
        {
            var seriNo = counterEntry.Text.Trim();
            if (string.IsNullOrEmpty(seriNo)) return;

            try
            {
                string islemAdi = null;
                decimal miktar = 0;

                if (rdbTopNo.IsChecked) islemAdi = "TopNo";
                else if (rdbSeriNo.IsChecked) islemAdi = "Seri No";
                else if (rdbRefakatKartNo.IsChecked) islemAdi = "Parti No";

                if (!await KontrolAsync()) return;

                if (islemAdi == "TopNo")
                {
                    miktar = await _apiClientService.KumasTopNetMt(seriNo);
                    rdbSeriNo.IsEnabled = false;
                    rdbRefakatKartNo.IsEnabled = false;
                }
                else if (islemAdi == "Seri No")
                {
                    miktar = await _apiClientService.SeriNoMiktar1(seriNo);
                    rdbTopNo.IsEnabled = false;
                    rdbRefakatKartNo.IsEnabled = false;
                }
                else if (islemAdi == "Parti No")
                {
                    miktar = await _apiClientService.SiparisPartiMiktar1(seriNo);
                    rdbSeriNo.IsEnabled = false;
                    rdbTopNo.IsEnabled = false;
                }
                string secilen = counterEntry.Text;
                string filter = $"SeriNo='{secilen}'";
                if (await _apiClientService.CountAsync("StokSayim", "SeriNo", filter, true) > 0)
                {
                    Vibration.Vibrate(500);
                    await DisplayAlert("Uyarı", $"{seriNo} Seri Numarası Daha Önce Kaydedilmiş.", "OK");
                    counterEntry.Text = string.Empty;
                    if(_seriNoList.Count>=1)
                    {
                        return;
                    }
                    else
                    {
                        rdbRefakatKartNo.IsEnabled = true;
                        rdbSeriNo.IsEnabled = true;
                        rdbTopNo.IsEnabled = true;
                        return;
                    }

                }

                // Yeni veri ekle
                var newItem = new Tablo
                {
                    Sira = _sira++,
                    SeriNo = seriNo,
                    Miktar = miktar
                };
                _seriNoList.Insert(0, newItem); // Insert the new item at the beginning of the list
                counterEntry.Text = string.Empty;
                counterEntry.Focus();
                counterLabel.Text = _seriNoList.Count.ToString();

                // Update the sequence numbers to reflect the new order
                UpdateSira();
            }
            catch (Exception ex)
            {
                Vibration.Vibrate(500);
                await DisplayAlert("Hata", ex.Message, "OK");
            }
        }
        private async Task<bool> KontrolAsync()
        {
            string mesaj = null;

            void SeriNoTemizle()
            {
                counterEntry.Focus();
                counterEntry.Text = string.Empty;
            }

            string seriNo = counterEntry.Text.Trim();
            string islemAdi = null;
            if (rdbTopNo.IsChecked) islemAdi = "TopNo";
            else if (rdbSeriNo.IsChecked) islemAdi = "Seri No";
            else if (rdbRefakatKartNo.IsChecked) islemAdi = "Parti No";
            if(!_kaydetButonunaBasildiMi)
            {
                if (string.IsNullOrEmpty(seriNo)) mesaj += "Seri No Giriniz." + Environment.NewLine;
            }
            if (string.IsNullOrEmpty(personEntry.Text)) mesaj += "Sayım Yapan Personel Seçilmelidir." + Environment.NewLine;
            if (string.IsNullOrEmpty(warehouseNoEntry.Text)) mesaj += "Ambar Seçilmelidir." + Environment.NewLine;
            if (!string.IsNullOrEmpty(warehouseNoEntry.Text) && !string.IsNullOrEmpty(warehouseAddressEntry.Text))
            {
                try
                {
                    bool isAmbarAdresValid = await _apiClientService.AmbarAdresKontrol(warehouseNoEntry.Text, warehouseAddressEntry.Text);
                    if (!isAmbarAdresValid) mesaj += "Girmiş Olduğunuz Adres Tanımsız." + Environment.NewLine;
                }
                catch (HttpRequestException ex)
                {
                    mesaj += $"Ambar Adres Kontrol Hatası: {ex.Message}" + Environment.NewLine;
                }
            }

            if (MukerrerKayitKontrol(seriNo)) mesaj += $"Girmiş Olduğunuz {islemAdi} Listede Var.";
            if (islemAdi == "TopNo")
            {
                if (!string.IsNullOrEmpty(seriNo))
                {
                    try
                    {
                        bool isTopNoValid = await _apiClientService.TopNoKontrol(seriNo);
                        if (!isTopNoValid)
                        {
                            mesaj += "Tanımsız Top Numarası." + Environment.NewLine;
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        mesaj += $"Top No Kontrol Hatası: {ex.Message}" + Environment.NewLine;
                    }
                }
            }
            if (islemAdi == "Seri No")
            {
                if (!string.IsNullOrEmpty(seriNo))
                {
                    try
                    {
                        bool isSeriNoValid = await _apiClientService.SeriNoKontrol(seriNo);
                        if (!isSeriNoValid) mesaj += "Tanımsız Seri Numarası." + Environment.NewLine;
                    }
                    catch (HttpRequestException ex)
                    {
                        mesaj += $"Seri No Kontrol Hatası: {ex.Message}" + Environment.NewLine;
                    }
                }
            }
            if (islemAdi == "Parti No")
            {
                if (!string.IsNullOrEmpty(seriNo))
                {
                    try
                    {
                        bool isPartiNoValid = await _apiClientService.PartiNoKontrol(seriNo);
                        if (!isPartiNoValid) mesaj += "Tanımsız Parti Numarası." + Environment.NewLine;
                    }
                    catch (HttpRequestException ex)
                    {
                        mesaj += $"Parti No Kontrol Hatası: {ex.Message}" + Environment.NewLine;
                    }
                }
            }

            if (!string.IsNullOrEmpty(mesaj))
            {
                Vibration.Vibrate(500);
                await DisplayAlert("Uyarı", mesaj, "OK");
                SeriNoTemizle();
                return false;
            }

            return true;
        }
        private bool MukerrerKayitKontrol(string seriNo)
        {
            return _seriNoList.Any(item => item.SeriNo == seriNo);
        }
    }
    public class Tablo
    {
        public int Sira { get; set; }
        public string? SeriNo { get; set; }
        public decimal Miktar { get; set; }
    }
}
