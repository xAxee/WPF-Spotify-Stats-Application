using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Spotify.Core;
using WPF_Spotify.MVVM.Model;

namespace WPF_Spotify.MVVM.ViewModel
{
    public class RecentrlyPlayedViewModel : Core.ViewModel
    {
        private List<MyTrack> _recentlyPlayed;
        public List<MyTrack>? RecentlyPlayed
        {
            get => _recentlyPlayed;
            set
            {
                _recentlyPlayed = value;
                OnPropertyChanged();
            }
        }
        public RecentrlyPlayedViewModel(DataContext _context)
        {
            RecentlyPlayed = _context.RecentlyPlayed;

        }
    }
}
