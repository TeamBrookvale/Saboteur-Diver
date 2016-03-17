using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using strange.extensions.context.api;

namespace TeamBrookvale.UI
{
	public class FireTouchEventCommand : Command
	{
		[Inject]
		public ITouchModel touchModel {get;set;}

		[Inject]
		public TouchFSM.Events e {get;set;}

		[Inject]
		public TouchScreenPosition p {get;set;}

		public override void Execute ()
		{
			touchModel.fireTouchEvent (e, p);
		}
	}
}