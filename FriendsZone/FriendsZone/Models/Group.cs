using System;

namespace FriendsZone.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
