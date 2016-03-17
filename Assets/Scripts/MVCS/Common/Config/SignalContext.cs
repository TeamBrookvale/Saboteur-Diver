//Base Class for all our Contexts
//We do two jobs here:
//1. Provide the important bindings for a Signals-based app (see http://strangeioc.wordpress.com/2013/09/18/signals-vs-events-a-strange-smackdown-part-2-of-2/)
//2. Scan for Implicit Bindings (see http://strangeioc.wordpress.com/2013/12/16/implicitly-delicious/)

using System;
using strange.extensions.context.impl;
using UnityEngine;
using strange.extensions.command.api;
using strange.extensions.command.impl;

namespace TeamBrookvale
{
	public class SignalContext : MVCSContext
	{
		public SignalContext (MonoBehaviour contextView) : base(contextView)
		{
		}

		protected override void addCoreComponents ()
		{
			base.addCoreComponents ();
			injectionBinder.Unbind<ICommandBinder> ();
			injectionBinder.Bind<ICommandBinder> ().To<SignalCommandBinder> ().ToSingleton ();
		}

		public override void Launch ()
		{
			base.Launch ();
			StartSignal startSignal = (StartSignal)injectionBinder.GetInstance<StartSignal>(); 
			startSignal.Dispatch();
		}

		protected override void mapBindings ()
		{
			// Only run once when this is the first context
			if (Context.firstContext != this) return;

			// use the root namespace here
			implicitBinder.ScanForAnnotatedClasses (new string[]{"TeamBrookvale"});

			//The Signal isn't bound to any Command,
			//so we map it as an injection so a Command can fire it, and a Mediator can receive it

		}
	}
}

