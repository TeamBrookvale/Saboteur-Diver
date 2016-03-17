using strange.extensions.command.impl;
using UnityEngine;

namespace TeamBrookvale.UI
{
	public class GuiElemRectRegisterCommand : Command
	{
		[Inject]
		public ITouchModel touchModel {get;set;}

		[Inject]
		public Rect guiRect {get;set;}

		[Inject]
		public bool register {get;set;}

		public override void Execute ()
		{
			touchModel.guiItemRectRegister (guiRect, register);
		}
	}
}