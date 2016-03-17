using System;
using UnityEngine;
using strange.extensions.signal.impl;

namespace TeamBrookvale.Game
{
	// Sound FX
	public class PlaySoundFxSignal					: Signal<SoundFx,TouchScreenPosition> {}
	public class SingletonSoundFxSignal				: Signal<SoundFx,SingletonSoundFxCmd> {}

	// spotlight noticed the diver or cannot see the diver anymore by one spotlights
	// int is view.GetInstanceID()
	// bool is true if noticed, false if diver cannot be seen anymore
	public class SpotlightPlayerNoticedSignal 		: Signal<int> {}
	public class SpotlightSunkShipNoticedSignal		: Signal<Vector2> {}

	// one ship got successfully drilled
	public class ShipSunkSignal 					: Signal<int, Vector2> {}
	public class ShipsHealthReachedZeroSignal		: Signal {}

	// Update positions
	public class CameraPositionSignal				: Signal<Vector2> {}

	// Request if this (spotlight) position is within camera bounds <light position, callback>
	public class SpotLighIsWithinCameraBoundsSignal	: Signal<Vector3,Action<bool>> {}

	// register all drillable ships in the model on startup
	public class ShipDrillableOnRegisterSignal		: Signal<int> {}

	// spawn item
	public class SpawnPrefabSignal					: Signal<SpawnPrefabModel.Prefab, Vector2, float> {}

	// smoke bomb explode and then exploded signal
	public class SmokeBombExplodeSignal				: Signal<TouchScreenPosition> {}
	public class SmokeBombExplodedSignal			: Signal<TouchScreenPosition> {}

	// when player is embarking or disembarking UBoat
	public class PlayerUBoatEmbarkSignal			: Signal<bool> {}

	// schmitt trigger player close to boat ONLY ONE UBOAT SHOULD BE IN A SCENE
	public class UBoatPlayerCloseSignal				: Signal<bool> {}

	// patrolship drilled signal
	public class ShipPatrolDrilledSignal			: Signal<ShipPatrolProperties> {}

	// Main events
	public class GameLevelPassedSignal				: Signal {}
	public class GameLevelFailedSignal				: Signal {}
	public class AllShipsSunkOrBombsMountedSignal	: Signal {}
	public class PlayerSwimmedToCamaradeBoatSignal	: Signal {}

	// Panic mode started or ended
	public class PanicModeStartedOrEndedSignal		: Signal<bool> {}

	// Drilling started or ended
	public class DrillingSignal						: Signal<bool> {}

	// Time bomb mounting started or ended
	public class TimeBombMountingSignal				: Signal<bool> {}

	// In app payment
	public class IAPInGameMenuSignal				: Signal<TeamBrookvale.UI.InGameMenuModel.IAPButton> {}
	public class IAPThankYouSignal	: Signal {}
}