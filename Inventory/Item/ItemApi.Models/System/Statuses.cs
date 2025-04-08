using System.ComponentModel.DataAnnotations;

namespace ItemApi.Models.System
{
    public static class Statuses
    {
        public static readonly string Owned = "OWNED";
        public static readonly string NotOwned = "NOT OWNED";
        public static readonly string Wishlist = "WISHLIST";
        public static readonly string Pending = "PENDING";
    }
}
