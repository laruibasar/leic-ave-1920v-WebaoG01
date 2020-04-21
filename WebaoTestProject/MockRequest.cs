using System;
using System.Collections.Generic;
using Webao;
using WebaoTestProject.Dto;

namespace WebaoTestProject
{
    public class MockRequest : IRequest
    {
        // format <string query, object testObject>
        private Dictionary<string, object> mockRequest = new Dictionary<string, object>();

        public MockRequest()
        {
            Boredom boredom = new Boredom();
            boredom.Activity = "Learn a new programming language";
            boredom.Type = "education";
            boredom.Participants = 1; 
            mockRequest.Add("activity?key=5881028", boredom);

            Boredom boredom2 = new Boredom();
            boredom2.Activity = "Learn the Chinese erhu";
            boredom2.Type = "music";
            mockRequest.Add("activity?participants=1&price=0.6", boredom2);

            DtoCountrySearch dtoCountrySearch = new DtoCountrySearch();
            dtoCountrySearch.Country = new List<Country>();
            Country country0 = new Country();
            country0.Country_Id = "PE";
            country0.Probability = 0.06323779f;
            dtoCountrySearch.Country.Add(country0);
            mockRequest.Add("?name=luis", dtoCountrySearch);

            Character character = new Character();
            character.Name = "Jon Snow";
            character.Culture = "Northmen";
            character.Born = "In 283 AC";
            mockRequest.Add("characters/583", character);

            //list with 2 tracks
            DtoTracks dtoTracks = new DtoTracks();
            List<Track> tracks = new List<Track>();
            Track track0 = new Track();
            track0.Name = "The Less I Know the Better";
            tracks.Add(track0);

            Track track1 = new Track();
            track1.Name = "Mr. Brightside";
            Artist artist10 = new Artist();
            artist10.Name = "The Killers";
            track1.Artist = artist10;
            tracks.Add(track1);
            DtoGeoTopTracks dtoGeoTopTracks = new DtoGeoTopTracks();
            dtoTracks.Track = tracks;
            dtoGeoTopTracks.Tracks = dtoTracks;

            dtoTracks.Track = tracks;
            mockRequest.Add("?method=geo.gettoptracks&country=australia", dtoGeoTopTracks);

        }

        /*
         * Not of interest to do this test, because we only set internal state
         */
        public IRequest BaseUrl(string host)
        {
            return this;
        }

        /*
         * Not of interest to do this test, because we only set internal state
         */
        public IRequest AddParameter(string arg, string value)
        {
            return this;
        }

        /*
        * This should be used to test the path argument, as to match or entry
        * in the request Dictonary, as to match a valid request
        */
        public object Get(string path, Type targetType)
        {
            if (mockRequest.TryGetValue(path, out object value))
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
