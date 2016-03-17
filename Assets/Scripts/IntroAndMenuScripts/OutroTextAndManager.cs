using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TeamBrookvale.NonMVCS
{
	public class OutroTextAndManager : MonoBehaviour {

		string[] dialog = { 
		"6|We've done it!", 
		"6|Yeah, the battleships, the bridge, the dam... \nit's all gone. ", 
		"5|Look at them! They're leaving!  ",
		"6|They're saving the last few remaining boats...\nHaha! ", 
		"5|I think we've won...  ",
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

		bool showDialog = true;
		bool outro02Loaded;
		float outro02LoadedTime;

		const float MAXOUTRO02LENGTH = 45;

		IList keepGameObjects = new List<GameObject> ();

		void Start ()
		{
			StartCoroutine (BlackPixelTransparentDelayed ());

			keepGameObjects.Add (this.gameObject);
			keepGameObjects.Add (GameObject.Find ("OutroMusic"));
			keepGameObjects.Add (GameObject.Find ("Main Camera"));
			keepGameObjects.Add (GameObject.Find ("BlackPixel"));


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

			if (outro02Loaded && outro02LoadedTime + MAXOUTRO02LENGTH < Time.time)
				Application.LoadLevel ("MainMenuScene");
		}

		IEnumerator BlackPixelTransparentDelayed ()
		{
			yield return new WaitForSeconds (1);
			GameObject.Find("BlackPixel").GetComponent<Animator>().SetTrigger ("Transparent");
		}

		void showNextDialog ()
		{
			// pushed the screen too many times or outro 02 loaded
			if (dialogIndex == dialog.Length)
			{
				// tapped on the outro screen
				if (outro02Loaded && outro02LoadedTime + 3 < Time.time)
					Application.LoadLevel ("MainMenuScene");

				// pushed the screen too many times
				return;
			}

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

			// if no more dialogs left then load OutroScene02
			if (dialogIndex == dialog.Length)
			{
				GameObject.Find("BlackPixel").GetComponent<Animator>().SetTrigger ("FadeInOutNow");

				StartCoroutine (LoadOutroScene02Delayed ());

				return;
			}
		}

		void OnGUI ()
		{
			if (showDialog)
			{
				GUI.Box (boxRect, textBackground, textBackgroundGUIStyle);
				GUI.Label (textRect, text, textGUIStyle);
			}
		}

		IEnumerator LoadOutroScene02Delayed ()
		{
			yield return new WaitForSeconds (.5f);
			destroyGameObjects ();
			showDialog = false;
			Application.LoadLevelAdditive ("OutroScene02");
			gameObject.AddComponent<CreditsScript>();
			outro02Loaded = true;
			outro02LoadedTime = Time.time;
			nextDialogShowAt = Time.time + MAXOUTRO02LENGTH;
		}

		void destroyGameObjects ()
		{
			foreach (GameObject o in (GameObject[]) GameObject.FindObjectsOfType (typeof (GameObject)))
				if (!keepGameObjects.Contains (o))
					GameObject.Destroy ((GameObject) o);
		}
	}
}