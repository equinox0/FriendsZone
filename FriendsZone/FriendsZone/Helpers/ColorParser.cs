using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendsZone.Helpers
{
    public static class ColorParser
    {

        public static float parseColorToFloat(string colorTxt)
        {
            switch (colorTxt)
            {
                case "Niebieski":
                    return 240.0f;
                case "Zielony":
                    return 120.0f;
                case "Pomarańczowy":
                    return 30.0f;
                case "Czerwony":
                    return 0.0f;
                case "Fioletowy":
                    return 270.0f;
                case "Żółty":
                    return 60.0f;
                default:
                    return 0.0f;
            }
        }

        public static string parseFloatToColor(float color)
        {
            switch ((int)color)
            {
                case 240:
                    return "Niebieski";
                case 120:
                    return "Zielony";
                case 30:
                    return "Pomarańczowy";
                case 0:
                    return "Czerwony";
                case 270:
                    return "Fioletowy";
                case 60:
                    return "Żółty";
                default:
                    return "Czerwony";
            }
        }
    }
}
