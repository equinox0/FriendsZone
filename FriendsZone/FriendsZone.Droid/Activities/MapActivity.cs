
using Android.Support.V4.App;
using Android.OS;
using Android.Content;
using Android.Gms.Maps;
//using Android.App;

namespace FriendsZone.Droid.Activities
{
    [Android.App.Activity(Label = "Mapa")]
    public class MapActivity : FragmentActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Map);

            SupportMapFragment mapFrag = (SupportMapFragment)SupportFragmentManager.FindFragmentById(Resource.Id.map);
            if (mapFrag != null)
            {
                GoogleMap map = mapFrag.Map;
                if (map != null)
                {
                    map.UiSettings.ZoomControlsEnabled = true;
                    map.UiSettings.CompassEnabled = true;
                    map.MapType = GoogleMap.MapTypeSatellite;
                }
            }
        }
    }
}