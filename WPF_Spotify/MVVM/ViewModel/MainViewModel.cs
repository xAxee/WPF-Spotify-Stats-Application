using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using WPF_Spotify.Core;
using WPF_Spotify.MVVM.Model;
using WPF_Spotify.Services;
using Timer = System.Timers.Timer;

namespace WPF_Spotify.MVVM.ViewModel
{
    public class MainViewModel : Core.ViewModel
    {
        private INavigationService _navigation;
        private PrivateUser _privateUser;
        private SpotifyLogin spotifyApi;
        private SpotifyClient _spotify;
        private string _currentPlaying;
        private string _iconMusic;
        public PrivateUser Profile
        {
            get => _privateUser;
            private set
            {
                _privateUser = value;
                OnPropertyChanged();
            }
        }
        public string CurrentPlaying
        { 
            get 
            { 
                return _currentPlaying; 
            } 
            set 
            { 
                _currentPlaying = value; 
                OnPropertyChanged(); 
            } 
        }
        public string IconMusic 
        { 
            get => _iconMusic; 
            private set 
            { 
                _iconMusic = value; 
                OnPropertyChanged(); 
            } 
        }
        public INavigationService Navigation
        {
            get => _navigation;
            private set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand NavigateToHomeViewCommand { get; set; }
        public RelayCommand NavigateToArtistViewCommand { get; set; }
        public RelayCommand NavigateToTrackViewCommand { get; set; }
        public RelayCommand NavigateToRecentlyPlayedViewCommand { get; set; }
        public RelayCommand NavigateToAboutViewCommand { get; set; }
        public RelayCommand SwitchPlaying { get; set; }
        public DataContext dataContext;

        public MainViewModel(INavigationService navService, DataContext _context)
        {
            Navigation = navService;
            // Register Navigate Commands
            NavigateToHomeViewCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);  
            NavigateToArtistViewCommand = new RelayCommand(o => { Navigation.NavigateTo<ArtistViewModel>(); }, o => true);
            NavigateToTrackViewCommand = new RelayCommand(o => { Navigation.NavigateTo<TrackViewModel>(); }, o => true);
            NavigateToRecentlyPlayedViewCommand = new RelayCommand(o => { Navigation.NavigateTo<RecentrlyPlayedViewModel>(); }, o => true);
            NavigateToAboutViewCommand = new RelayCommand(o => { Navigation.NavigateTo<AboutViewModel>(); }, o => true);
            
            Navigation.NavigateTo<HomeViewModel>();

            Load_app();
        }
        private void Load_app()
        {
            // Get spotify access 
            //new SpotifyLogin();
            Thread.Sleep(1000);
            _spotify = new SpotifyClient(Token.access_token);

            SwitchPlaying = new RelayCommand(o => {
                if (_spotify.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest()).Result.IsPlaying)
                {
                    _spotify.Player.PausePlayback();
                    _spotify.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest()).Result.IsPlaying = false;
                }
                else
                {
                    _spotify.Player.ResumePlayback();
                    _spotify.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest()).Result.IsPlaying = true;
                }
            }, o => true);

            Timer timer = new Timer(15000);
            timer.Elapsed += Refresh_data;
            timer.Start();
        }

        private void Refresh_data(object sender, ElapsedEventArgs e)
        {
            try
            {
                var _current = _spotify.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest()).Result;
                if(_current == null)
                {
                    CurrentPlaying = "";
                } else
                {

                    CurrentPlaying = $"{((FullTrack)_current.Item).Artists[0].Name} - {((FullTrack)_current.Item).Name}";
                    IconMusic = _current.IsPlaying ? "\uf04d;" : "\uf04b;";
                }
                Profile = _spotify.UserProfile.Current().Result;
            } catch(Exception ex) { }

        }
    }
}
