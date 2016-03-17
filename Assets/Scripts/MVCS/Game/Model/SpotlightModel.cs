using UnityEngine;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

namespace TeamBrookvale.Game
{
	public class SpotlightModel : ISpotlightModel
	{
		public SpotlightModel()
		{
			isEnabled = true;										// spotlight is working by default
			originalAngularVelocity = angularVelocity = .5f;		// angular velocity of the cone
			scanningAngle = 30; 									// scanning in degrees
			viewRange = 5;											// how far the cone can see
			originalFieldOfViewRange = 								// the cone in degrees originally
			targetFieldOfViewRange = fieldOfViewRange = 60;  		// cone of the spotlight in degrees
			discoveredDrillAirBubbleList = new List<Vector2> ();	// init list
		}

		public bool isEnabled							{get;set;}	// if not enabled then do not draw, managed from the mediator
		public bool isPlayerNoticed						{get;set;}	// true if currently the player is noticed
		public bool isDrillAirBubbleNoticed 			{get;set;}	// true if currently a sunk ship's air bubbles are noticed
		public float playerLastNoticedAt				{get;set;}	// time when this spotlight noticed the diver. It's null when currently not noticed anything.
		public float angularVelocity					{get;set;}	// angular velocity of the cone
		public float originalAngularVelocity			{get;set;}	// original angular velocity of the cone
		public float scanningAngle 						{get;set;}	// scanning in degrees
		public float viewRange 							{get;set;}	// how far the cone can see
		public float fieldOfViewRange 					{get;set;}	// cone of the spotlight in degrees
		public float originalFieldOfViewRange			{get;set;} // the cone in degrees originally
		public float targetFieldOfViewRange 			{get;set;}	// the above one will ultimately reach this value
		public float spotMedianAngle 					{get;set;}	// this is where the spotlight points at start in degrees
		public float currentAngle 						{get;set;}	// current angle of the middle of the cone at any point of time in degrees
		public List<Vector2> discoveredDrillAirBubbleList {get;set;}// Keep a list of the discovered air bubbles so do not stop again at these ones
		public Vector2 cachedPosition					{get;set;}	// used for spawning muzzle fire at game end

		public void updateConeSettingsForZeppelin ()
		{
			originalFieldOfViewRange = targetFieldOfViewRange = fieldOfViewRange /= 2;
			viewRange *= 2f;
		}
	}
}