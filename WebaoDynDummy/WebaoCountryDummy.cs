using System;
using System.Collections.Generic;
using Webao;
using WebaoDynamic;
using WebaoTestProject.Dto;

namespace WebaoDynDummy
{
    public class WebaoCountryDummy : WebaoDyn, WebaoDynCountry
    {
        public WebaoCountryDummy(IRequest req) : base(req)
		{
            req.BaseUrl("https://api.nationalize.io");
            req.AddParameter("format", "json");
        }

        public List<Country> GetNationality(string name)
		{
            string path = "?name={name}";
            path = path.Replace("{name}", name.ToString());

            Type type = typeof(DtoCountrySearch);
            DtoCountrySearch dto = (DtoCountrySearch)req.Get(path, type);

            return dto.Country;
		}
    }
}
