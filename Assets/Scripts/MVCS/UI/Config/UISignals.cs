using System;
using UnityEngine;
using strange.extensions.signal.impl;

namespace TeamBrookvale.UI
{
	public class FireTouchEventSignal		: Signal<TouchFSM.Events,TouchScreenPosition> {}
	public class TouchPinchZoomPanSignal 	: Signal<float,Vector3> {}
	public class GUIElemRectRegisterSignal	: Signal<Rect,bool> {}

	public class UIPlayAreaPressSignal		: Signal<TouchScreenPosition> {}
	public class UIPanBackToPlayerSignal 	: Signal {}
	public class UIPlayerActionSignal		: Signal<TouchScreenPosition,bool> {} // true when starting action, false when finishing

	public class InGameMenuFadeSignal		: Signal<bool> {}
	public class InGameMenuShowSignal		: Signal<bool, InGameMenuModel.MenuState> {}
	public class InGameMenuPushSignal		: Signal<InGameMenuModel.Icon> {}
}