#if false

using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using UnityEditor;
using UnityEngine;

namespace HutongGames.PlayMakerEditor
{
    [CustomActionEditor(typeof (SendUserEvent))]
    public class SendUserEventActionEditor : CustomActionEditor
    {
		
        public override bool OnGUI()
        {
		bool edited = false;
		SendUserEvent _target = (SendUserEvent)target;
		
		if (_target.GenderAsString==null)
		{
			_target.GenderAsString = new HutongGames.PlayMaker.FsmString(){UseVariable=false};
		}
		
		if (_target.GenderAsString.UseVariable)
		{
			EditField("GenderAsString");
				
		}else{
			GUILayout.BeginHorizontal();
			 	_target.Gender = (GA_User.Gender)EditorGUILayout.EnumPopup("Gender", _target.Gender);
			
				if (PlayMakerEditor.FsmEditorGUILayout.MiniButtonPadded(PlayMakerEditor.FsmEditorContent.VariableButton))
				{
					_target.GenderAsString.UseVariable = true;
				}
			GUILayout.EndHorizontal();
		}
		
		EditField("BirthYear");
		EditField("FriendCount");
		
		bool _advanced = EditorGUILayout.Foldout(_target.advanced,"Advanced Data");
		
		if (_advanced!=_target.advanced)
		{
			edited = true;
			_target.advanced = _advanced;
		}

		if (_target.advanced)
		{
			EditField("iosId");
			EditField("androidId");
			EditField("platform");
			EditField("device");
			EditField("os");
			EditField("osVersion");
			EditField("sdk");
			EditField("installPublisher");
			EditField("installSite");
			EditField("installCampaign");
			EditField("installAdGroup");
			EditField("installAd");
			EditField("installKeyword");
		}
		
		return GUI.changed || edited;
        }

    }
}

#endif