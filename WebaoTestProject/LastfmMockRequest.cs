using System;
using System.Collections.Generic;
using Webao;
using WebaoTestProject.Dto;

namespace WebaoTestProject
{
    public class LastfmMockRequest : IRequest
    {
        private Dictionary<string, object> lastFmObjects = new Dictionary<string, object>();

        public LastfmMockRequest()
        {
            // Hard coded objects
            /* Test 1 */
            DtoArtist dtoArtist = new DtoArtist();             dtoArtist.Artist = new Artist();
            dtoArtist.Artist.Name = "Muse";
            dtoArtist.Artist.Mbid = "fd857293-5ab8-40de-b29e-55a69d4e4d0f";
            dtoArtist.Artist.Url = "https://www.last.fm/music/Muse";
            dtoArtist.Artist.Stats = new Statistics();
            dtoArtist.Artist.Stats.Listeners = 4173831;
            dtoArtist.Artist.Stats.Playcount = 355366444;
            lastFmObjects.Add("?method=artist.getinfo&artist=muse", dtoArtist);

            /* Test 2 */
            DtoSearch dtoSearch = new DtoSearch();
            dtoSearch.Results = new DtoResults();
            dtoSearch.Results.ArtistMatches = new DtoArtistMatches();
            dtoSearch.Results.ArtistMatches.Artist = new List<Artist>();
            Artist artist0 = new Artist();
            artist0.Name = "The Black Keys";
            artist0.Mbid = "d15721d8-56b4-453d-b506-fc915b14cba2";
            artist0.Url = "https://www.last.fm/music/The+Black+Keys";
            dtoSearch.Results.ArtistMatches.Artist.Add(artist0);
            Artist artist1 = new Artist();
            artist1.Name = "Black Sabbath";
            artist1.Mbid = "5182c1d9-c7d2-4dad-afa0-ccfeada921a8";
            artist1.Url = "https://www.last.fm/music/Black+Sabbath";
            dtoSearch.Results.ArtistMatches.Artist.Add(artist1);

            Artist artist2 = new Artist();
            artist2.Name = "Black Eyed Peas";
            artist2.Mbid = "d5be5333-4171-427e-8e12-732087c6b78e";
            artist2.Url = "https://www.last.fm/music/Black+Eyed+Peas";
            dtoSearch.Results.ArtistMatches.Artist.Add(artist2);
            lastFmObjects.Add("?method=artist.search&artist=black", dtoSearch);
            lastFmObjects.Add("?method=artist.search&artist=black&page=1", dtoSearch);


            /* Test 3 */
            DtoGeoTopTracks dtoGeoTopTracks = new DtoGeoTopTracks();
            DtoTracks dtoTracks = new DtoTracks();
            List<Track> track = new List<Track>();

            Track track0 = new Track();
            track0.Name = "The Less I Know the Better";
            track.Add(track0);
            Track track1 = new Track();
            track1.Name = "Mr. Brightside";
            Artist trackArtist = new Artist();
            trackArtist.Name = "The Killers";
            track1.Artist = trackArtist;
            track.Add(track1);

            dtoTracks.Track = track;
            dtoGeoTopTracks.Tracks = dtoTracks;

            lastFmObjects.Add("?method=geo.gettoptracks&country=australia", dtoGeoTopTracks);
        }

        public IRequest AddParameter(string arg, string val)         {             return this;         }          public IRequest BaseUrl(string host)         {             return this;         }          public object Get(string path, Type targetType)         {
            if (lastFmObjects.TryGetValue(path, out object value))
            {
                if (value.GetType() == targetType)
                {
                    return value;
                }
                else
                {
                    return new object();
                }
            }
            else
            {
                return new object();
            }
        }
    }
} 