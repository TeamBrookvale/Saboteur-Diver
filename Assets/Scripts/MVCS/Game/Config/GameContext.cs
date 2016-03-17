using UnityEngine;
using System.Collections;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.pool.api;
using strange.extensions.pool.impl;
using System;

namespace TeamBrookvale.Game
{
	public class GameContext : SignalContext
	{
		
		public GameContext (MonoBehaviour view) : base(view) {}

		protected override void mapBindings()
		{
			base.mapBindings();

			// IRoutineRunner binding should be implicit but it does not work maybe due to not using namespaces?
			injectionBinder.Bind<IRoutineRunner> ().To<RoutineRunner> ().CrossContext ();

			// Cross context signals
			injectionBinder.Bind<PlayerGotoSignal> 				().ToSingleton ().CrossContext ();
			injectionBinder.Bind<PlayerActionSignal> 			().ToSingleton ().CrossContext ();
			injectionBinder.Bind<AllShipsSunkOrBombsMountedSignal> 			().ToSingleton ().CrossContext ();
			injectionBinder.Bind<InventoryIconPressSignal>		().ToSingleton ().CrossContext ();
			injectionBinder.Bind<CurrentInventoryIconSignal> 	().ToSingleton ().CrossContext ();
			injectionBinder.Bind<InventoryEventFireSignal>		().ToSingleton ().CrossContext ();
			injectionBinder.Bind<GameLevelPassedSignal>			().ToSingleton ().CrossContext ();
			injectionBinder.Bind<GameLevelFailedSignal>			().ToSingleton ().CrossContext ();
			injectionBinder.Bind<PauseResumeLevelPassedFailedSignal>().ToSingleton().CrossContext();
			injectionBinder.Bind<LoadNextLevelSignal> 			().ToSingleton ().CrossContext ();
			injectionBinder.Bind<RetryLevelSignal> 				().ToSingleton ().CrossContext ();
			injectionBinder.Bind<StatusBarMessageSignal>		().ToSingleton ().CrossContext ();
			injectionBinder.Bind<StatusBarMessageRemoveSignal>	().ToSingleton ().CrossContext ();
			injectionBinder.Bind<IAPInGameMenuSignal>			().ToSingleton ().CrossContext ();
			injectionBinder.Bind<IAPThankYouSignal>().ToSingleton().CrossContext ();
		
			// Bind Interfaces to Models
			injectionBinder.Bind<ICameraModel>			().To <CameraModel>			().ToSingleton ().CrossContext ();
			injectionBinder.Bind<IDamBridgeModel> 		().To <DamBridgeModel> 		().ToSingleton ().CrossContext ();
			injectionBinder.Bind<IDeserializedLevelsLoaderModel>().To<DeserializedLevelsLoaderModel> ().ToSingleton ();
			injectionBinder.Bind<IGameModel>			().To <GameModel>			().ToSingleton ().CrossContext ();
			injectionBinder.Bind<IInventoryModel>		().To <InventoryModel>		().ToSingleton ().CrossContext ();
			injectionBinder.Bind<ILevelBoundsModel>		().To <LevelBoundsModel>	().ToSingleton ();
			injectionBinder.Bind<IPlayerModel>			().To <PlayerModel>			().ToSingleton ();
			injectionBinder.Bind<IShipDrillableModel>	().To <ShipDrillableModel>	().ToSingleton ().CrossContext ();
			injectionBinder.Bind<IShipPatrolModel>		().To <ShipPatrolModel>		().ToSingleton ();
			injectionBinder.Bind<ISmokeBombCircleModel>	().To <SmokeBombCircleModel>().ToSingleton ().CrossContext ();
			injectionBinder.Bind<ISpawnPrefabModel>		().To <SpawnPrefabModel>	().ToSingleton ();
			injectionBinder.Bind<ISpotlightDictionary>	().To <SpotlightDictionary>	().ToSingleton ();
			injectionBinder.Bind<ISpotlightModel>		().To <SpotlightModel>		();
			injectionBinder.Bind<ITutorialModel>		().To <TutorialModel>		().ToSingleton ().CrossContext ();
			injectionBinder.Bind<IUBoatModel>			().To <UBoatModel>			().ToSingleton ();
			injectionBinder.Bind<IZeppelinModel>		().To <ZeppelinModel>		().ToSingleton ();

			// Pooling SoundFx
			injectionBinder.Bind<IPool<GameObject>>().To<Pool<GameObject>>().ToSingleton().ToName(GameElement.SOUND_FX_AUDIO_SOURCE);

			// Bind Mediators
			mediationBinder.Bind<SpotLightView>().To<SpotLightMediator>();
			mediationBinder.Bind<ShipDrillableView>().To<ShipDrillableMediator>();
			mediationBinder.Bind<PlayerView>().To<PlayerMediator>();
			mediationBinder.Bind<CameraView>().To<CameraMediator>();
			mediationBinder.Bind<SmokeBombCircleView> ().To <SmokeBombCircleMediator> ();
			mediationBinder.Bind<ShipPatrolView> ().To<ShipPatrolMediator> ();
			mediationBinder.Bind<SoldierWalkingView> ().To<SoldierWalkingMediator> ();
			mediationBinder.Bind<ZeppelinView> ().To<ZeppelinMediator> ();
			mediationBinder.Bind<UBoatView> ().To<UBoatMediator> ();
			mediationBinder.Bind<AirBubblesDiverView> ().To<AirBubblesDiverMediator> ();
			mediationBinder.Bind<CamaradeBoatView> ().To<CamaradeBoatMediator> ();
			mediationBinder.Bind<MineView> ().To<MineMediator> ();
			mediationBinder.Bind<DamBridgeView> ().To<DamBridgeMediator> ();
			mediationBinder.Bind<LevelBoundsView> ().To<LevelBoundsMediator> ();
			mediationBinder.Bind<MuzzleFlashView> ().To<MuzzleFlashMediator> ();
			mediationBinder.Bind<WaterView> ().To<WaterMediator> ();
			mediationBinder.Bind<GameModelView> ().To<GameModelMediator> ();
			mediationBinder.Bind<StatusBarMessagesDispatcherView> ().To <StatusBarMessagesDispatcherMediator> ();
			mediationBinder.Bind<IAPView> ().To <IAPMediator> ();
			mediationBinder.Bind<InGameMusicView> ().To <InGameMusicMediator> ();

			//Bind Commands
			commandBinder.Bind<SpotlightPlayerNoticedSignal>().To<SpotlightPlayerNoticedCommand>();
			commandBinder.Bind<ShipSunkSignal>().To<ShipDrillableSunkCommand>();
			commandBinder.Bind<PlaySoundFxSignal>().To<PlaySoundFxCommand>();
			commandBinder.Bind<SingletonSoundFxSignal>().To<SingletonSoundFxCommand>();
			commandBinder.Bind<ShipDrillableOnRegisterSignal>().To<ShipDrillableOnRegisterCommand>();
			commandBinder.Bind<PlayerActionSignal> ().To<PlayerActionCommand> ();
			commandBinder.Bind<SpawnPrefabSignal> ().To<SpawnPrefabCommand> ();
			commandBinder.Bind<SpotlightSunkShipNoticedSignal> ().To<PanicModeCommand> ();
			commandBinder.Bind<UBoatPlayerCloseSignal> ().To<UBoatPlayerCloseCommand> ();
			commandBinder.Bind<PlayerUBoatEmbarkSignal> ().To<PlayerUBoatEmbarkCommand> ();
			commandBinder.Bind<ShipPatrolDrilledSignal> ().To<PanicModeCommand> ();
			commandBinder.Bind<PlayerSwimmedToCamaradeBoatSignal> ().To<PlayerSwimmedToCamaradeBoatCommand> ();
			commandBinder.Bind<AllShipsSunkOrBombsMountedSignal> ().To<AllShipsSunkOrBombsMountedCommand> ();
			commandBinder.Bind<GameLevelPassedSignal> ().To<GameLevelPassedCommand> ();
			commandBinder.Bind<GameLevelFailedSignal> ().To<GameLevelFailedCommand>();
			commandBinder.Bind<InventoryIconPressSignal> ().To<InventoryIconPressCommand> ().Pooled ();
			commandBinder.Bind<InventoryEventFireSignal> ().To<InventoryEventFireCommand> ().Pooled ();
			commandBinder.Bind<SmokeBombExplodeSignal> ().To<SmokeBombExplodeCommand> ();
			commandBinder.Bind<DrillingSignal> ().To<DrillingCommand> ();
			commandBinder.Bind<TimeBombMountingSignal> ().To<TimeBombMountingCommand> ();
			commandBinder.Bind<ShipsHealthReachedZeroSignal> ().To<ShipsHealthReachedZeroCommand> ();
			commandBinder.Bind<LoadNextLevelSignal> ().To <LoadNextLevelCommand> ();
			commandBinder.Bind<RetryLevelSignal> ().To <RetryLevelCommand> ();
			commandBinder.Bind<IAPInGameMenuSignal> ().To <IAPInGameMenuCommand> ();

			//StartSignal is now fired instead of the START event.
			//Note how we've bound it "Once". This means that the mapping goes away as soon as the command fires.
			commandBinder.Bind<StartSignal>().To<GameStartCommand>().To<ShipPatrolAvoidCrashCommand>().Once();

			//The Signal isn't bound to any Command,
			//so we map it as an injection so a Command can fire it, and a Mediator can receive it
			//injectionBinder.Bind<GameLevelFailedSignal>().ToSingleton();
			injectionBinder.Bind <CameraPositionSignal>				().ToSingleton();
			injectionBinder.Bind <SmokeBombExplodedSignal> 			().ToSingleton ();
			injectionBinder.Bind <PanicModeStartedOrEndedSignal>	().ToSingleton ();
		}

		protected override void postBindings ()
		{
			// pool for the sound effects
			IPool<GameObject> soundFxAudioSourcePool = injectionBinder.GetInstance<IPool<GameObject>> (GameElement.SOUND_FX_AUDIO_SOURCE);
			soundFxAudioSourcePool.instanceProvider = new ResourceInstanceProvider ("Generic/SoundFx", 0/*LayerMask.NameToLayer ("enemy")*/);

			//Establish our camera. We do this early since it gets injected in places that help us do layout.
			Camera cam = (contextView as GameObject).GetComponentInChildren<Camera> ();
			if (cam == null)
			{
				throw new Exception ("Couldn't find the CameraView");
			}
			injectionBinder.Bind<Camera> ().ToValue (cam).ToName (CrossContextElements.GAME_CAMERA).CrossContext ();


			base.postBindings ();
		}
	}
}