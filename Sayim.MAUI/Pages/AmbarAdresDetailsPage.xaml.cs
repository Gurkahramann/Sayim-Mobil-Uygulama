using Microsoft.Maui.Controls;
using Sayim.ApiClient;
using Sayim.ApiClient.Models.ApiModels;

namespace Sayim.MAUI.Pages
{
    public partial class AmbarAdresDetailsPage : ContentPage
    {
        private AmbarAdres selectedAmbarAdres;
        private readonly ApiClientService _apiClientService;
        private string _ambarNo;

        public AmbarAdresDetailsPage(ApiClientService apiClientService,string ambarNo)
        {
            InitializeComponent();
            _apiClientService = apiClientService;
            _ambarNo = ambarNo;
            Load();
        }
        private async void Load()
        {
            var ambarAdresList = await _apiClientService.GetAmbarAdres(_ambarNo);
            if (ambarAdresList != null)
            {
                listView.ItemsSource = ambarAdresList;
            }
        }

        [Obsolete]
        private async void OnKaydetButtonClicked(object sender, EventArgs e)
        {
            if (selectedAmbarAdres != null)
            {
                MessagingCenter.Send(this, "UpdateWarehouseAddress", selectedAmbarAdres.Adres);
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Uyarý", "Lütfen bir Ambar seçin.", "OK");
            }
        }
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedAmbarAdres = (AmbarAdres)e.SelectedItem;
        }

    }
}
