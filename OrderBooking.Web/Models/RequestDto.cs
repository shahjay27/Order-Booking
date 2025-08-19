using static OrderBooking.Web.Utility.StaticDetails;

namespace OrderBooking.Web.Models
{
    public class RequestDto
    {
        public ApiTypes ApiType { get; set; } = ApiTypes.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
    }
}
