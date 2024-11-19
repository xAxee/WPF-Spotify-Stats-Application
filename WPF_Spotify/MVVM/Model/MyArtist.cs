using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Spotify.MVVM.Model
{
    public class MyArtist : FullArtist
    {
        public int Lp { get; set; }
        public string Top { get; set; }
        public Trend Trend { get; set; }
        public int TotalFollowers { get; set; }
    }
}
