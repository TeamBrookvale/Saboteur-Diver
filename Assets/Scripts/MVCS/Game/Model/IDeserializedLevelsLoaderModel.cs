using UnityEngine;
using System.Collections.Generic;

namespace TeamBrookvale.Game
{
	public interface IDeserializedLevelsLoaderModel
	{
		void generateItems (int level);
		Transform parentOfXmlItems {get;}
	}
}