namespace OrderBooking.Web.Utility
{
    public class StaticDetails
    {
        public static string CouponAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }

        public const string RoleAdmin = "Admin";
        public const string RoleCustomer = "Customer";
        public const string TokenCookie = "JWTToken";

        public enum ApiTypes
        {
            GET, POST, PUT, DELETE
        }
    }
}
