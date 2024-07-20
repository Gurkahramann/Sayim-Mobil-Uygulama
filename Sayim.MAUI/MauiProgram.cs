using Microsoft.Extensions.Logging;
using DevExpress.Maui;
using Sayim.ApiClient.IoC;
using Sayim.ApiClient;
using CommunityToolkit.Maui;
using Sayim.MAUI.Pages;

namespace Sayim.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {

            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().UseDevExpress(useLocalization: false).UseDevExpressCollectionView().UseDevExpressControls().UseDevExpressDataGrid().UseDevExpressEditors().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkit();
            builder.Services.AddApiClientService(options =>
            {
                options.ApiBaseAddress = "";
            });
            builder.Services.AddSingleton<ApiClientService>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<LoginPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}