using UnityEngine;
using System.Collections;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.pool.api;
using strange.extensions.pool.impl;

namespace TeamBrookvale.UI
{
	public class UIContext : SignalContext
	{
		public UIContext (MonoBehaviour view) : base(view) {}

		protected override void mapBindings()
		{
			base.mapBindings();

			// Bind Models
			injectionBinder.Bind<ITouchModel>		().To<TouchModel>		().ToSingleton ();
			injectionBinder.Bind<ITouchFSM>			().To<TouchFSM>			().ToSingleton ();
			injectionBinder.Bind<IUIFSM>			().To<UIFSM>			().ToSingleton ();
			injectionBinder.Bind<IStatusBarModel>	().To<StatusBarModel>	().ToSingleton ();
			injectionBinder.Bind<ILevelTextModel>	().To<LevelTextModel>	().ToSingleton ();
			injectionBinder.Bind<IInGameMenuModel> 	().To<InGameMenuModel> 	().ToSingleton ();

			// Bind Mediators
			mediationBinder.Bind<CursorView> 				().To<CursorMediator> ();
			mediationBinder.Bind<TouchScreenView>			().To<TouchScreenMediator> ();
			mediationBinder.Bind<TouchPinchZoomPanView> 	().To<TouchPinchZoomPanMediator> ();
			mediationBinder.Bind<InventoryIconView>			().To<InventoryIconMediator> ();
			mediationBinder.Bind<PauseIconView>				().To<PauseIconMediator> ();
			mediationBinder.Bind<StatusBarMessageView>		().To<StatusBarMediator> ();
			mediationBinder.Bind<InventoryCounterIconView>	().To<InventoryCounterIconMediator> ();
			mediationBinder.Bind<TitleTextView>				().To<TitleTextMediator> ();
			mediationBinder.Bind<InGameMenuBackgroundView>	().To<InGameMenuBackgroundMediator> ();
			mediationBinder.Bind<InGameMenuIconsView>		().To<InGameMenuIconsMediator> ();
			mediationBinder.Bind<ProgressCounterIconView>	().To<ProgressCounterIconMediator> ();
			mediationBinder.Bind<TutorialView> 				().To <TutorialMediator> ();

			//StartSignal is now fired instead of the START event.
			//Note how we've bound it "Once". This means that the mapping goes away as soon as the command fires.
			commandBinder.Bind <StartSignal> 				().To <UIStartCommand> 				().Once();
			commandBinder.Bind <TouchPinchZoomPanSignal>	().To <CameraPinchZoomPanCommand> 	().Pooled ();
			commandBinder.Bind <UIPanBackToPlayerSignal> 	().To <CameraFollowPlayerCommand> 	().Pooled ();
			commandBinder.Bind <FireTouchEventSignal> 		().To <FireTouchEventCommand> 		().Pooled ();
			commandBinder.Bind <GUIElemRectRegisterSignal> 	().To <GuiElemRectRegisterCommand> 	();
			commandBinder.Bind <UIPlayAreaPressSignal> 		().To <UIPlayAreaPressCommand> 		().Pooled ();
			commandBinder.Bind <UIPlayerActionSignal> 		().To <UIPlayerActionCommand> 		().Pooled ();
			commandBinder.Bind <PauseResumeLevelPassedFailedSignal> ().To <PauseResumeLevelPassedFailedCommand> 			();
			commandBinder.Bind <InGameMenuPushSignal>		().To <InGameMenuPushCommand> 		();

			// Bind singleton signals
			injectionBinder.Bind <InGameMenuFadeSignal>		().ToSingleton (); // fade first
			injectionBinder.Bind <InGameMenuShowSignal>		().ToSingleton (); // then show
		}
		
		protected override void postBindings ()
		{
			//IPool<GameObject> soundFxAudioSourcePool = injectionBinder.GetInstance<IPool<GameObject>> (GameElement.SOUND_FX_AUDIO_SOURCE);
			//soundFxAudioSourcePool.instanceProvider = new ResourceInstanceProvider ("Generic/SoundFx", 0/*LayerMask.NameToLayer ("enemy")*/);
			base.postBindings ();
		}
	}
}