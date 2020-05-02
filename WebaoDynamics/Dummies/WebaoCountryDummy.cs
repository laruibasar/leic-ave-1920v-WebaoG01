using System.Collections.Generic;
using Webao;
using Webao.Dto;
using WebaoDynamics.Interfaces;

namespace WebaoDynamics.Dummies
{
    public class WebaoCountryDummy : WebaoDyn, IWebaoCountry
    {
        public WebaoCountryDummy(IRequest req) : base(req)
		{
            base.SetUrl("https://api.nationalize.io");
            base.SetParameter("format", "json");
        }

        public List<Country> GetNationality(string name)
		{
            string path = "?name={name}";
            path = path.Replace("{name}", name.ToString());

            DtoCountrySearch dto = (DtoCountrySearch)base.GetRequest(path, typeof(DtoCountrySearch));

            return dto.Country;
		}
    }
}
