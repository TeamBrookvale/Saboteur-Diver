using UnityEngine;
using System.Collections;

[System.Serializable]
public class GA_CustomAdTrigger// : ScriptableObject
{
	[SerializeField]
	public GA_AdSupport.GAEventCat eventCat = GA_AdSupport.GAEventCat.Design;
	
	[SerializeField]
	public string eventID = "";
	
	[SerializeField]
	public GA_AdSupport.GAAdNetwork AdNetwork = GA_AdSupport.GAAdNetwork.Any;
}
