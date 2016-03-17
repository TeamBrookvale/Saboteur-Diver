using UnityEngine;
using System.Collections;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.pool.api;
using strange.extensions.pool.impl;
using System;

namespace TeamBrookvale.SelectLevel
{
	public class SelectLevelContext : SignalContext
	{
		public SelectLevelContext (MonoBehaviour view) : base(view) {}

		protected override void mapBindings()
		{
			base.mapBindings();

			// IRoutineRunner binding should be implicit but it does not work maybe due to not using namespaces?
			injectionBinder.Bind<IRoutineRunner> ().To<RoutineRunner> ().CrossContext ();

			// Bind Interfaces to Models
			injectionBinder.Bind<ISelectLevelModel>().To<SelectLevelModel>().ToSingleton();

			// Bind Mediators
			mediationBinder.Bind<SelectLevelButtonsView>().To<SelectLevelButtonsMediator>();

			//Bind Commands
			commandBinder.Bind<LoadLevelSignal>().To<LoadLevelCommand>();

			//StartSignal is now fired instead of the START event.
			commandBinder.Bind<StartSignal>().To<SelectLevelStartCommand>().Once();
		}

		protected override void postBindings ()
		{
				base.postBindings ();
		}
	}
}