using System.Collections.Generic;
using WebaoTestProject.Dto;

namespace WebaoDynamic.TP3Fluent
{
    public interface IWebaoArtist3B
    {
        Artist GetInfo(string name);
        List<Artist> Search(string name, int page);
    }
}
