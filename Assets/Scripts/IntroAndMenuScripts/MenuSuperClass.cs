using UnityEngine;
using System.Collections;

namespace TeamBrookvale.NonMVCS
{
	public class MenuSuperClass : MonoBehaviour {

		protected GUIStyle textGUIStyle = new GUIStyle ();
		protected Font AtwriterFont;

		protected readonly int largeFontSize = TeamBrookvale.Game.TBUtil.CalcFontSize (50);
		protected readonly int smallFontSize = TeamBrookvale.Game.TBUtil.CalcFontSize (25);

		protected virtual void Start ()
		{
			Time.timeScale = 1;

			AtwriterFont = Resources.Load<Font> ("Fonts/Atwriter");
			if (AtwriterFont == null) Debug.LogError ("Fonts/Atwriter does not exist");

			textGUIStyle.font = AtwriterFont;
			textGUIStyle.alignment = TextAnchor.MiddleCenter;
			textGUIStyle.normal.textColor = Color.black;
			textGUIStyle.fontSize = largeFontSize;
		}

		protected void AddScriptAndRemoveThisOne (string script)
		{
			// e.g. add CreditsScript
			gameObject.AddComponent (script);
			
			// then destroy this script
			Destroy (this);
		}
	}
}