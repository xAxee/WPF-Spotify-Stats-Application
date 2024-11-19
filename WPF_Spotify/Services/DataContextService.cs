using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Spotify.Core;
using WPF_Spotify.MVVM.Model;

namespace WPF_Spotify.Services
{
    public interface IDataContextService
    {
        DataContext DataContext { get; }
    }
    public class DataContextService : ObservableObject, IDataContextService
    {
        DataContext _context;
        public DataContext DataContext
        {
            get => _context;
            private set
            {
                _context = value;
                OnPropertyChanged();
            }
        }
    }
}
