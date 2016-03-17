namespace TeamBrookvale.UI
{
	public interface ITouchFSM
	{
		TouchFSM.States fireTouchEvent (TouchFSM.Events e, TouchScreenPosition t);
	}
}