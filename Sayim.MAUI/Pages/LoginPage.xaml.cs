using Sayim.ApiClient;
using Sayim.ApiClient.Models.ApiModels;

namespace Sayim.MAUI.Pages;

public partial class LoginPage : ContentPage
{
    private readonly ApiClientService _apiClientService;
    public LoginPage(ApiClientService apiClientService)
	{
		InitializeComponent();
        _apiClientService = apiClientService;
        LoadUserCredentials();

    }
    private async void OnGirisYapClicked(object sender, EventArgs e)
    {
        string kullaniciKodu = KullaniciKoduEntry.Text;
        string sifre = SifreEntry.Text;

        // Giri� yapma i�lemi burada ger�ekle�tirilecek
        var kullanici = await AuthenticateUser(kullaniciKodu, sifre);
        if (kullanici != null)
        {
            await DisplayAlert("Ba�ar�l�", "Giri� ba�ar�l�!", "OK");

            // Ba�ar�l� giri� sonras� y�nlendirme ve veri ta��ma
            if (RememberMeCheckBox.IsChecked)
            {
                SaveUserCredentials(kullaniciKodu, sifre);
            }
            else
            {
                ClearUserCredentials();
            }
            await Shell.Current.GoToAsync($"//MainPage?kullaniciKodu={kullanici.KullaniciKodu}");

        }
        else
        {
            await DisplayAlert("Hata", "Ge�ersiz kullan�c� kodu veya �ifre.", "OK");
        }
    }

    private async void OnCikisYapClicked(object sender, EventArgs e)
    {
        bool isExit = await DisplayAlert("Uyar�", "Uygulamadan ��k�� Yapmak �stiyor Musunuz??", "Yes", "No");
        if (isExit)
        {
            Application.Current.Quit();
        }
        else
        {
            return;
        }
    }

    private async Task<Kullanici?> AuthenticateUser(string kullaniciKodu, string sifre)
    {
        try
        {
            var kullanici = await _apiClientService.GetKullanici(kullaniciKodu, sifre);
            return kullanici;
        }
        catch
        {
            return null;
        }
    }
    private void SaveUserCredentials(string kullaniciKodu, string sifre)
    {
        Preferences.Set("KullaniciKodu", kullaniciKodu);
        Preferences.Set("Sifre", sifre);
        Preferences.Set("RememberMe", true);
    }

    private void ClearUserCredentials()
    {
        Preferences.Remove("KullaniciKodu");
        Preferences.Remove("Sifre");
        Preferences.Set("RememberMe", false);
    }

    private void LoadUserCredentials()
    {
        if (Preferences.Get("RememberMe", false))
        {
            KullaniciKoduEntry.Text = Preferences.Get("KullaniciKodu", string.Empty);
            SifreEntry.Text = Preferences.Get("Sifre", string.Empty);
            RememberMeCheckBox.IsChecked = true;
        }
    }
}