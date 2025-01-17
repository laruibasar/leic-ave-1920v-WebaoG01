﻿using System.Collections.Generic;

namespace Webao.Dto
{ 
    public class DtoList
    {
        //public List<Character> Character { get; set; }
        public DtoListResults Results { get; set; }
    }

    public class DtoListResults
    {
        public DtoCharacterMatches CharacterMatches { get; set; }
    }

    public class DtoCharacterMatches
    {
        public List<Character> Character { get; set; }
    }
}