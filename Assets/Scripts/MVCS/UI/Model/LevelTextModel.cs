using UnityEngine;

namespace TeamBrookvale.UI
{	
	public class LevelTextModel : ILevelTextModel
	{
		public Font AtwriterFont {get;set;}

		public LevelTextModel ()
		{
			AtwriterFont = Resources.Load<Font> ("Fonts/Atwriter");
			if (AtwriterFont == null) Debug.LogError ("Fonts/Atwriter does not exist");
		}
	}
}
