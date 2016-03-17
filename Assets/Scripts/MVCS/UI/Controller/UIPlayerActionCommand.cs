using TeamBrookvale;
using strange.extensions.command.impl;

namespace TeamBrookvale.UI
{
	public class UIPlayerActionCommand : Command
	{
		[Inject]
		public TouchScreenPosition touchScreenPosition {get;set;}
		
		[Inject]
		public bool startOrStop {get;set;}


		[Inject]
		public PlayerActionSignal playerActionSignal {get;set;}

		[Inject]
		public ITouchModel touchModel {get;set;}
		
		public override void Execute ()
		{
			if (startOrStop)
			{
				if (!touchModel.isGuiItem(touchScreenPosition))
					playerActionSignal.Dispatch (touchScreenPosition, true);
			}
			else
			{
				playerActionSignal.Dispatch (touchScreenPosition, false);
			}
		}
	}
}