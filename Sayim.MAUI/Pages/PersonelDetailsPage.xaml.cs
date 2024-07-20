using Microsoft.Maui.Controls;
using Sayim.ApiClient;
using Sayim.ApiClient.Models.ApiModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Sayim.MAUI.Pages
{
    public partial class PersonelDetailsPage : ContentPage
    {
        private readonly ApiClientService _apiClientService;
        private string _kullaniciKodu;
        private Personel selectedPersonel;

        public PersonelDetailsPage(ApiClientService apiClientService, string kullaniciKodu)
        {
            InitializeComponent();
            _apiClientService = apiClientService;
            _kullaniciKodu = kullaniciKodu;
            LoadData();
        }

        private async void LoadData()
        {
            var personelList = await _apiClientService.GetPersoneller(_kullaniciKodu);
            if (personelList != null)
            {
                listView.ItemsSource = new ObservableCollection<Personel>(personelList);
            }
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedPersonel = (Personel)e.SelectedItem;
        }

        private async void OnKaydetButtonClicked(object sender, EventArgs e)
        {
            if (selectedPersonel != null)
            {
                MessagingCenter.Send(this, "PersonelSelected", selectedPersonel.PersonelAdi);
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Uyarý", "Lütfen bir personel seçin.", "OK");
            }
        }
    }
}
