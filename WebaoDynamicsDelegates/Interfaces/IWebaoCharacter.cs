using System.Collections.Generic;
using Webao.Attributes;
using Webao.Dto;

namespace WebaoDynamicsDelegates.Interfaces
{
    [BaseUrl("https://anapioficeandfire.com/api/")]
    [AddParameter("format", "json")]
    public interface IWebaoCharacter
    {
        [Get("characters/{id}")]
        [Mapping(typeof(Character), With = "Webao.Dto.Character")]
        Character GetCharacter(int id);

        [Get("characters")]
        [Mapping(typeof(DtoList), With = "Webao.Dto.DtoList.GetListCharacters")]
        List<Character> GetList();
    }  
}

