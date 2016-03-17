namespace TeamBrookvale.UI
{
	public interface IStatusBarModel
	{
		void addNewMessage (string s);
		void removeMessage (string message);
		string getCurrentMessage ();
		bool isStatusBarVisible ();
		float statusBarToggledTime {get;set;}
	}
}