package md57d71a28de70112b3d338c1ad6194a120;


public class SpotDetailsActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("FriendsZone.Droid.Activities.SpotDetailsActivity, FriendsZone.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", SpotDetailsActivity.class, __md_methods);
	}


	public SpotDetailsActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == SpotDetailsActivity.class)
			mono.android.TypeManager.Activate ("FriendsZone.Droid.Activities.SpotDetailsActivity, FriendsZone.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
