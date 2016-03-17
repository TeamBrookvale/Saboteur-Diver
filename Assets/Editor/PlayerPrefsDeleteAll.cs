using UnityEditor;
using UnityEngine;

class PlayerPrefsDeleteAll
{
	[MenuItem ("TeamBrookvale/PlayerPrefsDeleteAll")]
	static void Build_iOS_Device ()
	{
		PlayerPrefs.DeleteAll ();

		/* Start Xcode build script
		ProcessStartInfo proc = new ProcessStartInfo();
		proc.FileName = "open";
		proc.WorkingDirectory = "/users/myUserName";
		proc.Arguments = "talk.sh";
		proc.WindowStyle = ProcessWindowStyle.Minimized;
		proc.CreateNoWindow = true;
		Process.Start(proc);
		UnityEngine.Debug.Log("Halløjsa");
		*/
	}
}