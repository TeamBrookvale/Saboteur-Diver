using System.Collections.Generic;
using UnityEngine;

namespace TeamBrookvale.Game
{
	public interface ISpotlightModel
	{
		bool isEnabled  		{get;set;}  // if not enabled then do not draw, managed from the mediator
		bool isPlayerNoticed	{get;set;}	// true if currently the player is noticed
		bool isDrillAirBubbleNoticed {get;set;} // true if currently a sunk ship's air bubbles are noticed
		float playerLastNoticedAt 	{get;set;}  // time when this spotlight noticed the diver. It's null when currently not noticed anything.
		float angularVelocity	{get;set;}	// angular velocity of the cone
		float originalAngularVelocity	{get;set;} // original angular velocity of the cone
		float scanningAngle  	{get;set;}  // scanning in degrees
		float viewRange   		{get;set;}	// how far the cone can see
		float fieldOfViewRange  {get;set;}  // cone of the spotlight in degrees
		float originalFieldOfViewRange{get;set;} // the cone in degrees originally
		float targetFieldOfViewRange {get;set;} // the above one will ultimately reach this value
		float spotMedianAngle  	{get;set;}  // this is where the spotlight points at start in degrees
		float currentAngle  	{get;set;}  // current angle of the middle of the cone at any point of time in degrees
		List<Vector2> discoveredDrillAirBubbleList {get;set;} // Keep a list of the discovered air bubbles so do not stop again at these ones
		Vector2 cachedPosition	{get;set;}	// used for spawning muzzle fire at game end

		void updateConeSettingsForZeppelin ();
	}

}