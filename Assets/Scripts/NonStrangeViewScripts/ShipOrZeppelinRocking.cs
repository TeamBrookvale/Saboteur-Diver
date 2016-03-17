using UnityEngine;
using System.Collections;

public class ShipOrZeppelinRocking : MonoBehaviour
{

	public float rockingSpeed = 5;
	public float rockingAmplitude = 1;

	float randomness, deflectionY, prevdeflectionY;

	void Start ()
	{
		randomness = Random.Range(0f, 2f * Mathf.PI);
		prevdeflectionY = 0;
	}
	
	void Update ()
	{
		deflectionY = rockingAmplitude * Mathf.Sin((randomness + Time.realtimeSinceStartup) * rockingSpeed);
		transform.position += Vector3.up * (deflectionY - prevdeflectionY);
		prevdeflectionY = deflectionY;
	}
}
