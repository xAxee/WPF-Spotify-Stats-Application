using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Spotify.Core;
using WPF_Spotify.MVVM.Model;

namespace WPF_Spotify.MVVM.ViewModel
{
    class TrackViewModel : Core.ViewModel
    {
        private SpotifyClient _spotify;

        private List<MyTrack>? _tracks;
        public RelayCommand Time4WeeksCommand { get; set; }
        public RelayCommand Time6MonthsCommand { get; set; }
        public RelayCommand TimeAllCommand { get; set; }

        public List<MyTrack>? Tracks
        {
            get => _tracks;
            set
            {
                _tracks = value;
                OnPropertyChanged();
            }
        }

        public TrackViewModel(DataContext _context)
        {
            Tracks = _context.Tracks_6months;

            Time4WeeksCommand = new RelayCommand(o => {
                Tracks = _context.Tracks_4weeks;
            }, o => true);
            Time6MonthsCommand = new RelayCommand(o => {
                Tracks= _context.Tracks_6months;
            }, o => true);
            TimeAllCommand = new RelayCommand(o => {
                Tracks= _context.Tracks_all;
            }, o => true);
        }
    }
}
