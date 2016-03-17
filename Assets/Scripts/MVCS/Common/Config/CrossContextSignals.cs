//This file contains all the signals that are dispatched between Contexts

using System;
using strange.extensions.signal.impl;

namespace TeamBrookvale
{
	public class StartSignal 			: Signal {}

	public class PlayerGotoSignal 		: Signal<TouchScreenPosition> {}
	public class PlayerActionSignal		: Signal<TouchScreenPosition,bool> {} // true when starting action, false when finishing

	public class InventoryIconPressSignal : Signal<bool> {}
	public class PauseResumeLevelPassedFailedSignal	: Signal<TeamBrookvale.UI.InGameMenuModel.MenuState> {}
	public class CurrentInventoryIconSignal : Signal<TeamBrookvale.Game.InvItem.IDType> {} // sent once the new button became active

	// Inventory event signals
	public class InventoryEventFireSignal: Signal<TeamBrookvale.Game.InventoryModel.Events, TouchScreenPosition> {}

	public class LoadNextLevelSignal	: Signal {}
	public class RetryLevelSignal		: Signal {}

	// Status bar message
	public class StatusBarMessageSignal			: Signal<string> {}
	public class StatusBarMessageRemoveSignal	: Signal<string> {}

/*
	//Input
	public class GameInputSignal 		: Signal<int>{};

	//Game
	public class GameStartSignal 		: Signal{}
	public class GameEndSignal 			: Signal{}
	public class LevelStartSignal 		: Signal{}
	public class LevelEndSignal 		: Signal{}
	public class UpdateLivesSignal 		: Signal<int>{}
	public class UpdateScoreSignal 		: Signal<int>{}
	public class UpdateLevelSignal 		: Signal<int>{}
*/
}

