using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webao;
using WebaoTestProject;
using WebaoTestProject.Dto;

namespace WebaoDynamic.Part3
{
    class ServiceCountries : AbstractAccessObject
    {
        private readonly IWebaoCountry webao;
        public ServiceCountries() : this(new HttpRequest())
        {
        }
        public ServiceCountries(IRequest req) : base(req) {
            webao = (IWebaoCountry)WebaoDynBuilder.Build(typeof(IWebaoCountry), req);
        }
        public IEnumerable<Country> GetNationality(string name)
        {
            List<Country> list = webao.GetNationality(name);
            foreach (Country item in list){
                yield return item;
            }
             
        }
    }
}
