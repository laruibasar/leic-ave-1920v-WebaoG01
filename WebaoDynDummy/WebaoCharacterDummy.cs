using System;
using System.Collections.Generic;
using Webao;
using WebaoDynamic;
using WebaoTestProject.Dto;

namespace WebaoDynDummy
{
    
    public class WebaoCharacterDummy : WebaoDyn, WebaoDynCharacter
    {
        public WebaoCharacterDummy(IRequest req) : base(req)
        {
            base.SetUrl("https://anapioficeandfire.com/api/");
            base.SetParameter("format", "json");
        }

        public Character GetCharacter(int id)
        {
            string path = "characters/{id}";
            path = path.Replace("{id}", id.ToString());

            Type type = typeof(Character);
            Character character = (Character)base.GetRequest(path, type);

            return character;
        }

        public List<Character> GetList()
        {
            string path = "characters";

            Type type = typeof(DtoList);
            DtoList dto = (DtoList)base.GetRequest(path, type);

            return dto.Results.CharacterMatches.Character;
        }
    }
}
