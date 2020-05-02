using System.Collections.Generic;
using Webao.Attributes;
using Webao.Dto;

namespace WebaoDynamics.Interfaces
{
    [BaseUrl("https://anapioficeandfire.com/api/")]
    [AddParameter("format", "json")]
    public interface IWebaoCharacter
    {
        [Get("characters/{id}")]
        [Mapping(typeof(Character), ".")]
        Character GetCharacter(int id);

        [Get("characters")]
        [Mapping(typeof(DtoList), ".Results.CharacterMatches.Character")]
        List<Character> GetList();
    }  
}

