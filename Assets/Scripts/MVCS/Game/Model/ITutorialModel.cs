using UnityEngine;
using System.Collections.Generic;

namespace TeamBrookvale.Game
{
	public interface ITutorialModel
	{
		List<TutorialModel.TutorialEntry> getTutorialEntriesForLevel (int level);
	}
}