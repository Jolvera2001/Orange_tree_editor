using Microsoft.Extensions.DependencyInjection;
using Orange_tree_editor.Services;
using Orange_tree_editor.ViewModels;

namespace Orange_tree_editor;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// This is a helper to put di related classes and services in one place.
    /// I don't expect this app to grow a lot but it doesn't hurt to do this sort of
    /// setup.
    /// </summary>
    public static void AddCommonServices(this IServiceCollection services)
    {
        // ViewModels
        services.AddTransient<MainWindowViewModel>();
        
        // Other Services
        services.AddSingleton<DataContext>();
        services.AddTransient<IDbService, DbService>();
        services.AddTransient<IFileHelper, FileHelper>();
    }
}