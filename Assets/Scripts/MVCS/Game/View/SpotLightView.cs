using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using System.Collections.Generic;

namespace TeamBrookvale.Game
{
	public class SpotLightView : View
	{
		public SpotlightPlayerNoticedSignal _spotlightPlayerNoticedSignal = new SpotlightPlayerNoticedSignal();
		public SpotlightSunkShipNoticedSignal _spotlightSunkShipNoticedSignal = new SpotlightSunkShipNoticedSignal();

		// caching the mesh object that is used at every update to rebuild the spotlight mesh
		Mesh mesh = new Mesh();

		// Populated by XML, loaded to the model by the mediator. This is where the spotlight points at start in degrees, however use the model instead of this one
		public float spotMedianAngle;

		MeshFilter meshFilter; // local cached variable
		MeshRenderer meshRenderer;
		PolygonCollider2D polygonCollider2D;

		// this spotlight's model is sent by the mediator in the init method
		ISpotlightModel m;

		// playermodel and gamemodel sent by the mediator in the init method
		IPlayerModel playerModel;
		IGameModel gameModel;

		// list of air bubble positions referencing to IShipDrillableModel set by mediator in the init method
		IList<Vector2> drillAirBubblePositionList;

		// last update time, this is used to detect when the last trigger was
		float lastUpdateTime;

		// Lerp color from green to red and back so we need a timer. 1 is the finished state and we do not want lerp on startup.
		float lerpColorTimer = 1;

		// keep a list of objects that the spotlight is looking at, i.e. smokebomb
		class TargetObject
		{
			public float registeredTime {get; private set;}
			public float noticeTime {get; private set;}
			public Vector2 position {get; private set;}

			public TargetObject (Vector2 position)
			{
				registeredTime = Time.time;
				noticeTime = registeredTime + Const.SpotlightLookAtTargetLag + Random.Range (0f, 1.5f);
				this.position = position;
			}
		}

		IList<TargetObject> targetObjects = new List<TargetObject>();

		public void init (ISpotlightModel spotlightModel, IList<Vector2> drillAirBubblePositionList, IPlayerModel playerModel, IGameModel gameModel)
		{
			// get the model from the mediator
			this.m = spotlightModel;
			this.playerModel = playerModel;
			this.gameModel = gameModel;

			// get the reference to the list of airbubbles from the mediator;
			this.drillAirBubblePositionList = drillAirBubblePositionList;

			// call View object's start
			base.Start();

			// position well above everything else
			transform.position = new Vector3 (transform.position.x, transform.position.y, -9);

			// start at the median angle
			m.currentAngle = m.spotMedianAngle - m.fieldOfViewRange / 2;

			// Add required Mesh components
			meshRenderer = gameObject.AddComponent<MeshRenderer>();
			meshRenderer.material = Resources.Load<Material>("Materials/Transparent-VertexLit");
			meshRenderer.castShadows = meshRenderer.receiveShadows = false;
			meshRenderer.sortingLayerName = "SpotLight";
			meshFilter = gameObject.AddComponent<MeshFilter>();

			// if the parent is a zeppelin, adjust the cone of the spotlight
			if (transform.parent.gameObject.GetComponent<ZeppelinView>() != null)
				m.updateConeSettingsForZeppelin ();
		}
		
		void Update ()
		{
			// given we use lerp and follow nothing should be done during pause
			if (gameModel.pauseStatus == PauseStatus.PAUSE) return;

			// update cached position
			m.cachedPosition = (Vector2) transform.position;

			// Only enable mesh if within camera bounds
			meshRenderer.enabled = m.isEnabled;

			// return if not enabled
			if (!m.isEnabled) return;

			// calculate angle if the spotlight is scanning
			float scanningSpotLightAngle = m.spotMedianAngle - m.fieldOfViewRange / 2
				+ Mathf.Sin (Time.time * m.angularVelocity) * m.fieldOfViewRange;

			// calculate the field of view
			m.fieldOfViewRange = Mathf.Lerp(m.fieldOfViewRange, m.targetFieldOfViewRange, .01f);

			// Get target object's position that the spotlight should look at. It's null if doesn't exist.
			TargetObject targetObject = getNoticedTargetObject ();

			// if there is no target to look at then look at the normal scanning direction
			if (targetObject == null)
			{
				m.currentAngle = Mathf.MoveTowardsAngle (
					m.currentAngle,
					scanningSpotLightAngle,
					.1f / Const.SpotlightLookAtRotateSpeed);
			}

			// else look at the target
			else
			{
				Vector2 n = (targetObject.position - (Vector2) transform.position).normalized;

				float targetObjectAngle = Mathf.Atan2(n.y, n.x) * 180 / Mathf.PI - m.fieldOfViewRange / 2;

				m.currentAngle = Mathf.LerpAngle (
					m.currentAngle,
					targetObjectAngle,
					(Time.time - targetObject.noticeTime) * Const.SpotlightLookAtRotateSpeed);
			}


			// Regenerate the spotlight mesh
			UpdateMeshAndCollider();

			// check if player within collider polygon
			CheckPlayerOrAirBubbleSeen();

			// Lerp Color Spotlight
			LerpColorSpotlight();
		}

		internal void OnSmokeBombExplodedSignal (TouchScreenPosition t)
		{
			OnSmokeBombExplodedSignal (t.world, Const.SpotlightReturnToNormalTime);
		}

		internal void OnSmokeBombExplodedSignal (Vector2 p, float removeTime)
		{
			targetObjects.Add (new TargetObject (p));
			
			// FIFO remove this object later so the spotlight will not look at it anymore
			StartCoroutine (RemoveFirstTargetObjectDelayed (removeTime));
		}

		IEnumerator RemoveFirstTargetObjectDelayed (float removeTime)
		{
			yield return new WaitForSeconds (removeTime + Random.Range (0f, 3f));

			// FIFO remove first object that the spotlight will not look at it anymore
			targetObjects.RemoveAt(0);
		}

		TargetObject getNoticedTargetObject ()
		{
			// if there are targetobjects in the list and enough time passed for the spotlight to notice and close enough then return it
			foreach (TargetObject to in targetObjects)
				if (to.noticeTime < Time.time && (to.position - (Vector2) transform.position).magnitude < Const.SpotlightMaxTargetDistance)
					return to;
			
			return null;
		}

		void CheckPlayerOrAirBubbleSeen ()
		{
			// check if airbubbles noticed and only dispatch once
			if (!m.isDrillAirBubbleNoticed)
			{
				foreach (Vector2 drillAirBubblePosition in drillAirBubblePositionList)
				{
					// do not discover the same bubble again
					if (m.discoveredDrillAirBubbleList.Contains (drillAirBubblePosition))
						continue;

					// check if the bubble is within the spotlight's mesh
					if (IsPointInPolygon (mesh.vertices, drillAirBubblePosition - (Vector2) transform.position))
					{
						// add to the list of discovered air bubble list
						m.discoveredDrillAirBubbleList.Add (drillAirBubblePosition);

						// make sure this only runs once for now
						StartCoroutine (DoNotNoticeDrillAirBubbleTemporarily());

						// dispatch the signal
						_spotlightSunkShipNoticedSignal.Dispatch (drillAirBubblePosition);

						// slow down the parent patrol boat if it's a patrol boat
						if (transform.parent.GetComponent<ShipPatrolView>() != null)
							transform.parent.GetComponent<ShipPatrolView>().spotLightSunkShipNoticedSignalDirectCall();

						// look at bubbles
						OnSmokeBombExplodedSignal (drillAirBubblePosition, Const.ShipPatrolAlertSlowLength);
					}
				}
			}

			// check if player is within the spotlight and not hidden under a bridge or dock or embarked uboat
			bool newIsPlayerNoticed = 
				IsPointInPolygon (mesh.vertices, playerModel.getCachedPosition () - (Vector2) transform.position)
				&& playerModel.isHiddenUnderDockOrBridge == 0
				&& !playerModel.isEmbarkedUBoat;

			if (newIsPlayerNoticed != m.isPlayerNoticed)
			{
				m.isPlayerNoticed = newIsPlayerNoticed;
				_spotlightPlayerNoticedSignal.Dispatch (GetInstanceID());

				// restart color lerp-ing
				lerpColorTimer = 0;
			}

			if (newIsPlayerNoticed)
			{
				m.playerLastNoticedAt = Time.time;
			}
		}

		IEnumerator DoNotNoticeDrillAirBubbleTemporarily ()
		{
			m.isDrillAirBubbleNoticed = true;
			yield return new WaitForSeconds (Const.ShipPatrolDoNotNoticeDrillAirBubbleTemporarilyTime);
			m.isDrillAirBubbleNoticed = false;
		}


		bool IsPointInPolygon(Vector3[] polygon, Vector2 point)
		{
			bool isInside = false;
			for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
			{
				if (((polygon[i].y > point.y) != (polygon[j].y > point.y)) &&
				    (point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) + polygon[i].x))
				{
					isInside = !isInside;
				}
			}
			return isInside;
		}

		// Generating the spotlight mesh in runtime
		void UpdateMeshAndCollider ()
		{
			int numberOfVerticesOnArc = (int) (m.fieldOfViewRange / Const.SpotlightDetail) + 1;

			// VERTICES
			
			Vector3[] vertices = new Vector3[numberOfVerticesOnArc + 1];

			vertices[0] = new Vector3(0,0); // centre
			
			for (int i = 0; i < numberOfVerticesOnArc; i++)
			{
				// initialize spotlight direction, first vertex has the lowest degree
				float d = Mathf.PI / 180f * m.currentAngle;
				
				// if this is the last vertex then add the fieldOfViewRange angle, otherwise at i * dfi
				d += Mathf.PI / 180f * ((i == numberOfVerticesOnArc - 1) ? m.fieldOfViewRange : i * Const.SpotlightDetail);
				
				// create vertex
				vertices [i + 1] = m.viewRange * new Vector3 (Mathf.Cos(d), Mathf.Sin(d) * Const.SpotlightDistortion);
			}
			
			// TRIANGLES
			
			// generate triangles, something similar to this: int[] triangles = new int[6]{0,2,1,0,3,2};
			
			int[] triangles = new int[ (numberOfVerticesOnArc - 1) * 3 ];
			
			for (int t = 0; t < numberOfVerticesOnArc - 1; t++)
			{
				triangles [ t * 3 ] 	= 0;
				triangles [ t * 3 + 1 ]	= t + 2;
				triangles [ t * 3 + 2 ]	= t + 1;
			}
			


			// UVS
			
			Vector2[] uvs = new Vector2[vertices.Length];
			
			// center has the bottom left texture
			uvs[0] = new Vector2(0,0); //top-left
			
			// everything on the arc have the top right of the texture
			for (int u = 1; u < vertices.Length; u++)
				uvs[u] = new Vector2(1,1);


			// FINAL STEPS
			mesh.Clear ();
			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.uv = uvs;

			// finally get vertices' normal
			mesh.RecalculateNormals();
			
			//grab our filter.. set the mesh
			meshFilter.mesh = mesh;

		}

		void LerpColorSpotlight ()
		{
			const float lerpSpeed = 5;
			
			// cap lerpColorTimer at 1 and use speed of 2
			lerpColorTimer = Mathf.Min (lerpColorTimer + Time.deltaTime * lerpSpeed, 1);
			
			// set color with Lerp
			meshRenderer.material.color = Color.Lerp(
				m.isPlayerNoticed || m.isDrillAirBubbleNoticed ? Const.SpotLightColorGreen : Const.SpotLightColorRed,
				m.isPlayerNoticed || m.isDrillAirBubbleNoticed ? Const.SpotLightColorRed : Const.SpotLightColorGreen,
				lerpColorTimer);
		}
	}
}