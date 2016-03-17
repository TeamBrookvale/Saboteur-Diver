using UnityEngine;
using System.Collections;

namespace TeamBrookvale.NonMVCS
{
	public class Intro04TextAndManager : MonoBehaviour {

		string[] dialog = { 
		"10|I can't believe we got away. I don't think they will ever \nfind us here. But let's not test the waters.", 
		"4|That's it! You are a genius!", 
		"4|What? ... What do you mean?", 
		"10|What I mean is that we have tried everything from derailing\ntheir cargo to blowing up their warehouses and yet they still\nmanaged to take over the whole town. We need to get to\nthem unnoticed where they will never suspect us.", 
		"4|And how do you suppose we do that?", 
		"8|We are going to test the WATER!!! ... \nStill got that old wetsuit you found a while back?", 
		"6|I think I may have it stashed away somewhere.", 
		"8|Good!! Let's move... \nYou get the wetsuit and I'll get the boat from my house.", 
		"4|Alright.", 
		"6|Try not to get caught alright. We are the only hope this town has.", 
		"4|Are you sure we can do this?", 
		"10|Mate, this is not my first quest. I think I can handle it...\nDon't worry I have a plan. I'll meet you at the old abandoned \nswimming pool at the outskirts of town.", 
		"4|Alright then."
		};

		Animator[] talkingMen = new Animator[2];

		Rect textRect = new Rect (
			Screen.width * .02f,	// top left corner x
			Screen.height * .77f,	// top left corner y
			Screen.width * .96f,	// width
			Screen.height * .23f);	// height

		Rect boxRect = new Rect (
			0,						// top left corner x
			Screen.height * .75f,	// top left corner y
			Screen.width,			// width
			Screen.height * .25f);	// height

		float lastTouchTime;
		GUIStyle textGUIStyle = new GUIStyle ();
		Texture2D textBackground;
		GUIStyle textBackgroundGUIStyle = new GUIStyle ();

		int pipeIndex;
		string text;
		int dialogIndex;
		float nextDialogShowAt;

		void Start ()
		{
			// Delete blackpixel that was kept
			GameObject bp = GameObject.Find ("BlackPixel");
			if (bp == null)
				Debug.LogError ("BlackPixel not found hence not destroyed");
			else
				GameObject.Destroy (bp);

			Font AtwriterFont = Resources.Load<Font> ("Fonts/Atwriter");
			if (AtwriterFont == null) Debug.LogError ("Fonts/Atwriter does not exist");

			textBackground = new Texture2D (1,1);
			textBackground.SetPixel (0, 0, Color.black);
			textBackground.wrapMode = TextureWrapMode.Repeat;
			textBackground.Apply ();
			textBackgroundGUIStyle.normal.background = textBackground;

			talkingMen[0] = GameObject.Find ("TalkingMan01").GetComponent<Animator> ();
			talkingMen[1] = GameObject.Find ("TalkingMan02").GetComponent<Animator> ();

			if (talkingMen [0] == null || talkingMen [1] == null)
				Debug.LogError ("TalkingMan01's or TalkingMan02's animator component cannot be found");

			textGUIStyle.font = AtwriterFont;
			//textGUIStyle.alignment = TextAnchor.MiddleCenter;
			textGUIStyle.normal.textColor = Color.white;
			textGUIStyle.fontSize = TeamBrookvale.Game.TBUtil.CalcFontSize (20);
		}

		void Update ()
		{
			if (0 < Input.touchCount
			    || nextDialogShowAt < Time.time
			    || Input.GetMouseButtonDown(0)) // Input.GetMouseButtonDown just for development purposes
					showNextDialog ();
		}

		void showNextDialog ()
		{
			// at least .25 sec between each click
			if (.25f < Time.time - lastTouchTime)
			{
				lastTouchTime = Time.time;

				pipeIndex = dialog [dialogIndex].IndexOf ('|');
				nextDialogShowAt = Time.time + float.Parse (dialog [dialogIndex].Substring (0, pipeIndex));
				text = dialog [dialogIndex].Substring (pipeIndex + 1);

				talkingMen [dialogIndex     % 2].SetTrigger ("Talk");
				talkingMen [(dialogIndex+1) % 2].SetTrigger ("Stop");

				dialogIndex++;
			}

			// if no more dialogs left then load the main menu
			if (dialogIndex == dialog.Length)
			{
				loadMainMenu ();
				return;
			}
		}

		void OnGUI ()
		{
			GUI.Box (boxRect, textBackground, textBackgroundGUIStyle);
			GUI.Label (textRect, text, textGUIStyle);
		}

		void loadMainMenu ()
		{
			Application.LoadLevel ("GameScene");
		}
	}
}