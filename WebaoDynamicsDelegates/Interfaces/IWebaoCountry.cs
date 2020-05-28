using System.Collections.Generic;
using Webao.Attributes;
using Webao.Dto;

namespace WebaoDynamicsDelegates.Interfaces
{
    [BaseUrl("https://api.nationalize.io")]
    [AddParameter("format", "json")]
    public interface IWebaoCountry
    {
        [Get("?name={name}")]
        [Mapping(typeof(DtoCountrySearch), With = "Webao.Dto.Country")]
        List<Country> GetNationality(string name);
    }
}
