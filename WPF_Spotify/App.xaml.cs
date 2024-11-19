using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPF_Spotify.Core;
using WPF_Spotify.MVVM.Model;
using WPF_Spotify.MVVM.View;
using WPF_Spotify.MVVM.ViewModel;
using WPF_Spotify.Services;

namespace WPF_Spotify
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        public App()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });

            // Register ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<ArtistViewModel>();
            services.AddSingleton<TrackViewModel>();
            services.AddSingleton<AboutViewModel>();
            services.AddSingleton<RecentrlyPlayedViewModel>();
            services.AddSingleton<DataContext>();

            services.AddSingleton<IDataContextService, DataContextService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<Func<Type, ViewModel>>(servicesProvider => viewModelType => (ViewModel)servicesProvider.GetRequiredService(viewModelType));

            _serviceProvider = services.BuildServiceProvider();

        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

    }
}
