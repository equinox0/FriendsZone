using System;

namespace FriendsZone.Models
{
    public class Users
    {
        public String email { get; set; }
        public String name { get; set; }
        public String surname { get; set; }
        public String password { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1}",
                name,
                surname);
        }
    }
}
