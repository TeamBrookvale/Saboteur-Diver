using System.Collections.Generic;

namespace TeamBrookvale.Game
{
	public interface IInventoryModel 
	{
		InvItem getCurrentInvItem ();
		InvItem.IDType fire (InventoryModel.Events e, TouchScreenPosition t);

		string getCurrentInvItemText ();

		int smokeBombsLeft {get;set;}
		bool cachedIsSmokeBombEnabledOnThisLevel {get;set;}

		void moreSmokeBombsForJustNow ();
	}
}