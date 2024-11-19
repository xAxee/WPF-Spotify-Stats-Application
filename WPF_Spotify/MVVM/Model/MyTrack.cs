using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Spotify.MVVM.Model
{
    public class MyTrack : FullTrack
    {
        public int Lp { get; set; }
        public Trend Trend { get; set; }
        public DateTime Date { get; set; }
    }
    public class MySimpleTrack : SimpleTrack
    {
        public int Lp { get; set; }
        public int Trend { get; set; }
    }
}
