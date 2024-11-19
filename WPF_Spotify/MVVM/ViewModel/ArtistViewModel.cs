using System.Collections.Generic;
using WPF_Spotify.Core;
using WPF_Spotify.MVVM.Model;

namespace WPF_Spotify.MVVM.ViewModel
{
    public class ArtistViewModel : Core.ViewModel
    {
        private List<MyArtist>? _artists { get; set; }

        public RelayCommand Time4WeeksCommand { get; set; }
        public RelayCommand Time6MonthsCommand { get; set; }
        public RelayCommand TimeAllCommand { get; set; }
        public List<MyArtist>? Artists
        {
            get => _artists;
            set
            {
                _artists = value;
                OnPropertyChanged();
            }
        }

        public ArtistViewModel(DataContext _context)
        {
            Artists = _context.Artists_6months;

            Time4WeeksCommand = new RelayCommand(o => {
                Artists = _context.Artists_4weeks;
            }, o => true);
            Time6MonthsCommand = new RelayCommand(o => {
                Artists = _context.Artists_6months;
            }, o => true);
            TimeAllCommand = new RelayCommand(o => {
                Artists = _context.Artists_all;
            }, o => true);
        }
    }
}
