using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup.Localizer;
using static SpotifyAPI.Web.PersonalizationTopRequest;

namespace WPF_Spotify.MVVM.Model
{
    public interface IDataContext
    {
    }
    public class DataContext : IDataContext
    {
        public List<MyTrack>? RecentlyPlayed { get; set; }
        
        public List<MyArtist> Artists_4weeks;
        public List<MyArtist> Artists_6months;
        public List<MyArtist> Artists_all;

        public List<MyTrack> Tracks_4weeks;
        public List<MyTrack> Tracks_6months;
        public List<MyTrack> Tracks_all;

        public List<MyGenre> Genres_4weeks;
        public List<MyGenre> Genres_6months;
        public List<MyGenre> Genres_all;
        public PrivateUser Profile;

        private SpotifyClient _spotify;


        public DataContext() 
        {
            login();
        }
        public void login()
        {
            new SpotifyLogin();
            Thread.Sleep(1000);
            _spotify = new SpotifyClient(Token.access_token);

            Thread.Sleep(1000);

            // Fill tracks list
            Tracks_4weeks = GetTopTracks(TimeRange.ShortTerm);
            Tracks_6months = GetTopTracks(TimeRange.MediumTerm);
            Tracks_all = GetTopTracks(TimeRange.LongTerm);
            Thread.Sleep(1000);

            // Fill artists list
            Artists_all = GetTopArtists(TimeRange.LongTerm);
            Artists_6months = GetTopArtists(TimeRange.MediumTerm);
            Artists_4weeks = GetTopArtists(TimeRange.ShortTerm);

            // Fill Recentrly Played list
            RecentlyPlayed = GetRecentrlyPlayed();

            // Fill artists trends
            Artists_all.ForEach(e =>
            {
                e.Trend = new Trend(GetArtistTrend(TimeRange.LongTerm, e), true);
            });
            Artists_6months.ForEach(e =>
            {
                e.Trend = new Trend(GetArtistTrend(TimeRange.MediumTerm, e));
            });
            Artists_4weeks.ForEach(e =>
            {
                e.Trend = new Trend(GetArtistTrend(TimeRange.ShortTerm, e));
            });

            // Fill tracks trends
            Tracks_all.ForEach(e =>
            {
                e.Trend = new Trend(GetTracksTrend(TimeRange.LongTerm, e), true);
            });
            Tracks_6months.ForEach(e =>
            {
                e.Trend = new Trend(GetTracksTrend(TimeRange.MediumTerm, e));
            });
            Tracks_4weeks.ForEach(e =>
            {
                e.Trend = new Trend(GetTracksTrend(TimeRange.ShortTerm, e));
            });

            Thread.Sleep(1000);
            Profile = _spotify.UserProfile.Current().Result;
        }


        public int GetTime()
        {
            int time1 = _spotify.Player.GetRecentlyPlayed(new PlayerRecentlyPlayedRequest() { Limit=50 }).Result.Items.Sum(e => e.Track.DurationMs);
            Profile = _spotify.UserProfile.Current().Result;
            return time1;
        }
        private List<MyTrack>? GetRecentrlyPlayed()
        {
            List<MyTrack> recentlyPlayedList = new List<MyTrack>();

            _spotify.Player.GetRecentlyPlayed(new PlayerRecentlyPlayedRequest() { Limit=50 }).Result.Items.ForEach(e =>
            {
                recentlyPlayedList.Add(new MyTrack()
                {
                    Lp = recentlyPlayedList.Count()+1,
                    Name = e.Track.Name,
                    Album = e.Track.Album,
                    Artists = e.Track.Artists,
                    Date = e.PlayedAt
                });
            });

            return recentlyPlayedList;
        }
        // Get Top Artists
        private List<MyArtist> GetTopArtists(TimeRange term = TimeRange.MediumTerm)
        {
            if(_spotify == null)
            {
                login();
            }
            List<MyArtist> artistsList = new List<MyArtist>();
            
            _spotify.Personalization.GetTopArtists(new PersonalizationTopRequest() { Limit=50, TimeRangeParam=term }).Result.Items.ForEach(e =>
            {
                artistsList.Add(new MyArtist()
                {
                    Lp = artistsList.Count()+1,
                    Name = e.Name,
                    Images = e.Images,
                    TotalFollowers = e.Followers.Total,
                    Top = GetTopArtistTrack(term, e)
                });
            });

            return artistsList;
        }
        // Get Top Tracks
        private List<MyTrack> GetTopTracks(TimeRange term = TimeRange.MediumTerm)
        {
            List<MyTrack> trackList = new List<MyTrack>();
            _spotify.Personalization.GetTopTracks(new PersonalizationTopRequest() { Limit=50, TimeRangeParam=term }).Result.Items.ForEach(e =>
            {
                trackList.Add(new MyTrack()
                {
                    Lp = trackList.Count()+1,
                    Name = e.Name,
                    Album = e.Album,
                    Artists = e.Artists,
                    DurationMs = e.DurationMs
                });
            });
            return trackList;
        }
        // Get Top artist track
        private string GetTopArtistTrack(TimeRange term, FullArtist artist)
        {
            switch (term)
            {
                case TimeRange.ShortTerm:
                    return Tracks_4weeks.Where(a => a.Artists.Select(e => e.Name).ToList().Contains(artist.Name)).FirstOrDefault(new MyTrack() { Name = "..." }).Name;

                case TimeRange.MediumTerm:
                    return Tracks_6months.Where(a => a.Artists.Select(e => e.Name).ToList().Contains(artist.Name)).FirstOrDefault(new MyTrack() { Name = "..." }).Name;

                case TimeRange.LongTerm:
                    return Tracks_all.Where(a => a.Artists.Select(e => e.Name).ToList().Contains(artist.Name)).FirstOrDefault(new MyTrack() { Name = "..." }).Name;

                default:
                    return "Brak";
            }
        }

        // Get artist trends
        private int GetArtistTrend(TimeRange term, FullArtist e)
        {
            if (term == TimeRange.LongTerm)
            {
                int first = Artists_6months.IndexOf(Artists_6months.Find(a => a.Name == e.Name));
                int second = Artists_all.IndexOf(Artists_all.Find(a => a.Name == e.Name));
                if (first == -1 || second == -1) return -99;
                return second - first;
            }
            if (term == TimeRange.ShortTerm)
            {
                int first = Artists_4weeks.IndexOf(Artists_4weeks.Find(a => a.Name == e.Name));
                int second = Artists_6months.IndexOf(Artists_6months.Find(a => a.Name == e.Name));
                if (first == -1 || second == -1) return -99;
                return second - first;
            }
            if(term == TimeRange.MediumTerm)
            {
                int first = Artists_6months.IndexOf(Artists_6months.Find(a => a.Name == e.Name));
                int second = Artists_all.IndexOf(Artists_all.Find(a => a.Name == e.Name));
                if (first == -1 || second == -1) return -99;
                return second - first;
            }
            return 0;
        }
        // Get tracks trends
        private int GetTracksTrend(TimeRange term, MyTrack e)
        {
            if (term == TimeRange.LongTerm)
            {
                int first = Tracks_6months.IndexOf(Tracks_6months.Find(a => a.Name == e.Name));
                int second = Tracks_all.IndexOf(Tracks_all.Find(a => a.Name == e.Name));
                if (first == -1 || second == -1) return -99;
                return second - first;
            }
            if (term == TimeRange.ShortTerm)
            {
                int first = Tracks_4weeks.IndexOf(Tracks_4weeks.Find(a => a.Name == e.Name));
                int second = Tracks_6months.IndexOf(Tracks_6months.Find(a => a.Name == e.Name));
                if (first == -1 || second == -1) return -99;
                return second - first;
            }
            if (term == TimeRange.MediumTerm)
            {
                int first = Tracks_6months.IndexOf(Tracks_6months.Find(a => a.Name == e.Name));
                int second = Tracks_all.IndexOf(Tracks_all.Find(a => a.Name == e.Name));
                if (first == -1 || second == -1) return -99;
                return second - first;
            }
            return 0;
        }
    }
}
