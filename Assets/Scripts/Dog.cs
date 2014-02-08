using UnityEngine;
using System.Collections;

public class Dog : MonoBehaviour 
{
	private Vector2 targetPosition = Vector2.zero;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Take keyboard input and work out a target position
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		
		Vector3 currentPosition3 = Camera.main.WorldToScreenPoint(transform.position);
		Vector2 currentPosition2;
		currentPosition2 = currentPosition3.x;
		currentPosition2 = currentPosition3.y;
	}
}
