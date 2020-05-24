using Webao.Attributes;
using Webao.Dto;

namespace WebaoDynamicsDelegates.Interfaces
{
    [BaseUrl("https://www.boredapi.com/api/")]
    [AddParameter("format", "json")]
    public interface IWebaoBoredom
    {
        [Get("activity?key={key}")]
        [Mapping(typeof(Boredom), ".")]
        Boredom GetActivityByKey(int key);

        [Get("activity?participants={participants}&price={price}")]
        [Mapping(typeof(Boredom), ".")]
        Boredom GetActivity(int participants, float price);
    }
}
