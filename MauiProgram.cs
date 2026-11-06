using Microsoft.Extensions.Logging;
using NotasAppSQLite.Services;
using NotasAppSQLite.ViewModels;
using NotasAppSQLite.Views;

namespace NotasAppSQLite;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ==========================================
        // Inyección de dependencias
        // ==========================================

        // Base de Datos (Singleton)
        builder.Services.AddSingleton<NotasDatabase>();

        // ViewModels
        builder.Services.AddTransient<NotasViewModel>();
        builder.Services.AddTransient<DetalleNotaViewModel>();
        builder.Services.AddTransient<HistorialViewModel>();

        // Views
        builder.Services.AddTransient<NotasPage>();
        builder.Services.AddTransient<DetalleNotaPage>();
        builder.Services.AddTransient<HistorialPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
