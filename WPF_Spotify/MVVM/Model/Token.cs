using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Spotify.MVVM.Model
{
    public static class Token
    {
        public static string access_token { get; set; }
        public static string token_type { get; set; }
        public static int expires_in { get; set; }
    }
}
