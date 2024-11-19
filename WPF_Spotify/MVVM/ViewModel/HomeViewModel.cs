using EmbedIO.Utilities;
using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Spotify.MVVM.Model;
using WPF_Spotify.Services;

namespace WPF_Spotify.MVVM.ViewModel
{
    public class HomeViewModel : Core.ViewModel
    {
        private PrivateUser _user;
        public PrivateUser User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(); }
        }
        public HomeViewModel(DataContext _context) {
            User = _context.Profile;
        }
    }
}
