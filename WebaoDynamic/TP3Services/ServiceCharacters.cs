using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webao;
using WebaoTestProject;
using WebaoTestProject.Dto;

namespace WebaoDynamic.TP3Services
{
    class ServiceCharacters 
    {
        private readonly IWebaoCharacter webao;
        public ServiceCharacters() : this(new HttpRequest())
        {
        }
        public ServiceCharacters(IRequest req) {
            webao = (IWebaoCharacter)WebaoDynBuilder.Build(typeof(IWebaoCharacter), req);
        }
        public IEnumerable<Character> GetList()
        {
            List<Character> list = webao.GetList();
            foreach (Character item in list)
            {
                yield return item;
            }
        }
    }
}
