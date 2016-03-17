using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace TeamBrookvale.Game
{
	public class TutorialModel : ITutorialModel
	{
		public enum Anchor { Center, PauseButton, ToolButton, ProgressCounter };

		public class TutorialEntry
		{
			public int levelNumber;
			public string file;
			public Anchor anchor;
			public Texture2D texture;

			public TutorialEntry (int levelNumber, string file, Anchor anchor)
			{
				this.levelNumber 	= levelNumber;
				this.file			= file;
				this.anchor 		= anchor;

			}
		}

		List<TutorialEntry> tutorialEntries = new List<TutorialEntry>();

		public TutorialModel ()
		{
			tutorialEntries.Add (new TutorialEntry (
				1,
				"Tutorial01Before",
				Anchor.Center));

			tutorialEntries.Add (new TutorialEntry (
				1,
				"Tutorial01Swim",
				Anchor.Center));

			tutorialEntries.Add (new TutorialEntry (
				1,
				"Tutorial01GetawayBoat",
				Anchor.Center));

			tutorialEntries.Add (new TutorialEntry (
				2,
				"Tutorial02Status",
				Anchor.Center));

			tutorialEntries.Add (new TutorialEntry (
				2,
				"Tutorial02Inventory",
				Anchor.Center));

			tutorialEntries.Add (new TutorialEntry (
				2,
				"Tutorial02Pinch",
				Anchor.Center));

			tutorialEntries.Add (new TutorialEntry (
				2,
				"Tutorial02Drilling01",
				Anchor.Center));

			tutorialEntries.Add (new TutorialEntry (
				2,
				"Tutorial02Drilling02",
				Anchor.Center));

			tutorialEntries.Add (new TutorialEntry (
				4,
				"Tutorial04SmokeBomb01",
				Anchor.Center));

			tutorialEntries.Add (new TutorialEntry (
				4,
				"Tutorial04SmokeBomb02",
				Anchor.Center));
			
			tutorialEntries.Add (new TutorialEntry (
				4,
				"Tutorial04SmokeBomb03",
				Anchor.Center));
			
			tutorialEntries.Add (new TutorialEntry (
				4,
				"Tutorial04SmokeBomb04",
				Anchor.Center));
			
			tutorialEntries.Add (new TutorialEntry (
				4,
				"Tutorial04SmokeBomb05",
				Anchor.Center));

			tutorialEntries.Add (new TutorialEntry (
				11,
				"Tutorial11TimeBomb01",
				Anchor.Center));

			tutorialEntries.Add (new TutorialEntry (
				11,
				"Tutorial11TimeBomb02",
				Anchor.Center));

			tutorialEntries.Add (new TutorialEntry (
				11,
				"Tutorial11TimeBomb03",
				Anchor.Center));

			foreach (TutorialEntry e in tutorialEntries)
			{
				e.texture = Resources.Load <Texture2D> ("UI/TutorialMessages/" + e.file);
				if (e.texture == null) Debug.LogError ("UI/TutorialMessages/" + e.file + " cannot be found");
			}
		}

		public List<TutorialModel.TutorialEntry> getTutorialEntriesForLevel (int level)
		{
			return (from e in tutorialEntries where e.levelNumber == level select e).ToList();
		}
	}
}