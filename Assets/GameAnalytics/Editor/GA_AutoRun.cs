using UnityEngine;
using UnityEditor;

public class GA_Autorun : AssetPostprocessor
{
	static void OnPostprocessAllAssets ( string[] importedAssets,string[] deletedAssets,string[] movedAssets,string[] movedFromAssetPaths)
	{
		string[] splitPath = Application.dataPath.Split('/');
		
		if (!splitPath[splitPath.Length - 2].Equals("ga_unity_wrapper copy"))
		{
			GA_Inspector.CheckForUpdates();
			
			GA_Tracking.Setup();
		}
	}
}