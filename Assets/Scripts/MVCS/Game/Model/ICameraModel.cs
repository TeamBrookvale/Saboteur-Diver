using UnityEngine;

namespace TeamBrookvale.Game
{
	public interface ICameraModel
	{
		Camera camera {get;set;}
		bool getIsFollowingPlayer();
		void setIsFollowingPlayer(bool b);
		bool isWithinBounds (Vector2 p);

		bool isNewProposedCameraOrthographicSize {get; set;}
		bool isNewProposedCameraPosition {get; set;}
		void setProposedCameraOrthographicSize (float proposedCameraOrhographicSize);
		void setProposedCameraPosition (Vector2 proposedCameraPosition);
		float getProposedCameraOrthographicSize ();
		Vector3 getProposedCameraPosition ();
	}
}