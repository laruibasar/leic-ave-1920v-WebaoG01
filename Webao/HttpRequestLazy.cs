namespace Webao
{
    public class HttpRequestLazy : HttpRequest
    {
        public HttpRequestLazy() : base() { }

        public HttpRequestLazy(int limit, int page) : base()
        {
            this.Page = page;
            this.Limit = limit;
        }

        public override int Page { get; set; }

        public override int Limit { get; set; }
    }
}
