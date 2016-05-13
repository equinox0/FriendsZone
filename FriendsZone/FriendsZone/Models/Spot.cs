using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendsZone.Models
{
    public class Spot
    {
        public int id { get; set; }
        public int gid { get; set; }
        public string name { get; set; }
        public decimal latitutde { get; set; } 
        public decimal longitude { get; set; }
        public string description { get; set; }

        public override string ToString()
        {
            return name;
        }
    } 
}
