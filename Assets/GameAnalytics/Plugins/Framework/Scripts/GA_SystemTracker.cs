using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(GA_SpecialEvents))]
[RequireComponent(typeof(GA_Gui))]

[ExecuteInEditMode]
public class GA_SystemTracker : MonoBehaviour
{
	#region public values
	
	public static GA_SystemTracker GA_SYSTEMTRACKER;
	
	public bool UseForSubsequentLevels = true;
	
	public bool IncludeSystemSpecs = true;
	public bool IncludeSceneChange = true;
	public bool SubmitErrors = true;
	public int MaxErrorCount = 10;
	public bool SubmitErrorStackTrace = true;
	public bool SubmitErrorSystemInfo = true;
	public bool SubmitFpsAverage = true;
	public bool SubmitFpsCritical = true;
	public int FpsCriticalThreshold = 20;
	public int FpsCirticalSubmitInterval = 1;
	public bool GuiEnabled;
	public bool GuiAllowScreenshot;

	public bool ErrorFoldOut = true;
	
	#endregion
	
	#region unity derived methods
	
	#if UNITY_EDITOR
	void OnEnable ()
	{
		EditorApplication.hierarchyWindowItemOnGUI += GA.HierarchyWindowCallback;
		
		if (Application.isPlaying)
			GA_SYSTEMTRACKER = this;
	}
	
	void OnDisable ()
	{
		EditorApplication.hierarchyWindowItemOnGUI -= GA.HierarchyWindowCallback;
	}
	#endif
	
	public void Awake ()
	{
		if (!Application.isPlaying)
			return;
		
		if (GA_SYSTEMTRACKER != null)
		{
			// only one system tracker allowed per scene
			GA.LogWarning("Destroying dublicate GA_SystemTracker - only one is allowed per scene!");
			Destroy(gameObject);
			return;
		}
		GA_SYSTEMTRACKER = this;
	}
	
	/// <summary>
	/// Setup involving other components
	/// </summary>
	public void Start ()
	{
		if (!Application.isPlaying || GA_SYSTEMTRACKER != this)
			return;
		
		if (UseForSubsequentLevels)
			DontDestroyOnLoad(gameObject);
		
		GA_Gui gui = GetComponent<GA_Gui>();
		gui.GuiAllowScreenshot = GuiAllowScreenshot;
		gui.GuiEnabled = GuiEnabled;
		
		GA.API.Debugging.SubmitErrors = SubmitErrors;
		GA.API.Debugging.SubmitErrorStackTrace = SubmitErrorStackTrace;
		GA.API.Debugging.SubmitErrorSystemInfo = SubmitErrorSystemInfo;
		GA.API.Debugging.MaxErrorCount = MaxErrorCount;

		#if (UNITY_4_9 || UNITY_4_8 || UNITY_4_7 || UNITY_4_6 || UNITY_4_5 || UNITY_4_3 || UNITY_4_2 || UNITY_4_1 || UNITY_4_0_1 || UNITY_4_0 || UNITY_3_5 || UNITY_3_4 || UNITY_3_3 || UNITY_3_2 || UNITY_3_1 || UNITY_3_0_0 || UNITY_3_0 || UNITY_2_6_1 || UNITY_2_6)
		if (GA.API.Debugging.SubmitErrors)
			Application.RegisterLogCallback(GA.API.Debugging.HandleLog);
		#else
		if (GA.API.Debugging.SubmitErrors)
			Application.logMessageReceived += GA.API.Debugging.HandleLog;
		#endif

		// Add system specs to the submit queue
		if (IncludeSystemSpecs)
		{
			List<Hashtable> systemspecs = GA.API.GenericInfo.GetGenericInfo("");
			
			foreach (Hashtable spec in systemspecs)
			{
				GA_Queue.AddItem(spec, GA_Submit.CategoryType.GA_Error, false);
			}
		}
	}
	
	void OnDestroy()
	{
		if (!Application.isPlaying)
			return;
		
		if (GA_SYSTEMTRACKER == this)
			GA_SYSTEMTRACKER = null;

		#if (UNITY_4_9 || UNITY_4_8 || UNITY_4_7 || UNITY_4_6 || UNITY_4_5 || UNITY_4_3 || UNITY_4_2 || UNITY_4_1 || UNITY_4_0_1 || UNITY_4_0 || UNITY_3_5 || UNITY_3_4 || UNITY_3_3 || UNITY_3_2 || UNITY_3_1 || UNITY_3_0_0 || UNITY_3_0 || UNITY_2_6_1 || UNITY_2_6)
		if (GA.API.Debugging.SubmitErrors)
			Application.RegisterLogCallback(null);
		#else
		if (GA.API.Debugging.SubmitErrors)
			Application.logMessageReceived -= GA.API.Debugging.HandleLog;
		#endif
	}
	
	#endregion
}
