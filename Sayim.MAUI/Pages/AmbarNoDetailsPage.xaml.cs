using DevExpress.Maui.DataGrid;
using Microsoft.Maui.Controls;
using Sayim.ApiClient;
using Sayim.ApiClient.Models.ApiModels;

namespace Sayim.MAUI.Pages
{
    public partial class AmbarNoDetailsPage : ContentPage
    {
        private readonly ApiClientService _apiClientService;
        private Ambar selectedAmbar;
        public AmbarNoDetailsPage(ApiClientService apiClientService)
        {
            InitializeComponent();
            _apiClientService = apiClientService;
            LoadData();
        }

         private async void LoadData()
        {
            var ambarList = await _apiClientService.GetAmbar();
            if (ambarList != null)
            {
                listView.ItemsSource = ambarList;
            }
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedAmbar = e.SelectedItem as Ambar;
        }

        [Obsolete]
        private async void OnKaydetButtonClicked(object sender, EventArgs e)
        {
            if (selectedAmbar != null)
            {
                MessagingCenter.Send(this, "UpdateWarehouseNo", selectedAmbar.AmbarNo);
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Uyarý", "Lütfen bir Ambar seçin.", "OK");
            }
        }



        private void dataGrid_SelectionChanged(object sender, DevExpress.Maui.DataGrid.SelectionChangedEventArgs e)
        {

        }
    }
}
