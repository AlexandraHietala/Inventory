using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UserApi.Models.System
{
    public static class Roles
    {
        public static readonly int AdminAccess = 9999;
        public static readonly int ReadWriteFullAccess = 4000;
        public static readonly int ReadOnlyFullAccess = 3000;
        public static readonly int ReadWriteLimitedAccess = 2000;
        public static readonly int ReadOnlyLimitedAccess = 1000;
    }
}
