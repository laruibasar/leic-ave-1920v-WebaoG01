using System;
using System.Collections.Generic;
using System.Reflection;
using Webao;
using Webao.Attributes;
using Webao.Base;
using WebaoDynamic;
using WebaoTestProject.Dto;

namespace WebaoDynDummy
{
    
    public class WebaoCharacterDummy : WebaoDynCharacter
    {
        private readonly IRequest req;

        public WebaoCharacterDummy(IRequest req)
        {
            this.req = req;
            req.BaseUrl("https://anapioficeandfire.com/api/");
            req.AddParameter("format", "json");
        }

        public Character GetCharacter(int id)
        {
            string path = "characters/{id}";
            path = path.Replace("{id}", id.ToString());

            Type type = typeof(Character);
            Character character = (Character)req.Get(path, type);

            return character;
        }

        public List<Character> GetList()
        {
            string path = "characters";

            Type type = typeof(DtoList);
            DtoList dto = (DtoList)req.Get(path, type);

            return dto.Results.CharacterMatches.Character;
        }
    }
}
