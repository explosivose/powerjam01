﻿using UnityEngine;
using System.Collections;

public class Dog : MonoBehaviour 
{
	private Animator animator;
	private Vector2 targetPosition = Vector2.zero;
	private float boxoffset = 0;
	private int timeSinceFootprint = 0;
	private bool leftFootForwards = false;

	public float moveForce = 10f;			// added force to move the dog
	public float maxSpeed = 0.5f;			// max dog speed
	public Transform footprint;				// footprint prefab/sprite to spawn
	public int footprintInterval = 30;		// distance between each footprint
	public float footprintOffsetY = 0.4f;	// unit vectors downwards
	public static Vector2 spawnPosition;	// as a percentage of screen height/width (0 to 1)
	
	void OnLevelWasLoaded()
	{
		if (spawnPosition != null)
		{
			float height = spawnPosition.y * Screen.height;
			float width = spawnPosition.x * Screen.width;
			Vector3 screenPos = new Vector3(width, height);
			Vector3 newWorldPos = Camera.main.ScreenToWorldPoint(screenPos);
			newWorldPos.z = transform.position.z;
			transform.position = newWorldPos;
		}
	}
	
	// Use this for initialization
	void Start () 
	{
		boxoffset = GetComponent<BoxCollider2D> ().center.x;
	}

	void Update ()
	{
		animator = GetComponent<Animator>();
		Vector3 dogScale = transform.localScale;
		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		float PCHeight = 1-(screenPos.y / Screen.height);
		dogScale = PCHeight * Vector3.one;
		transform.localScale = dogScale;
	}


	// Update is called once per frame
	void FixedUpdate () 
	{
		// Take keyboard input and work out a target position
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		
//		Vector3 currentPosition3 = Camera.main.WorldToScreenPoint(transform.position);
//		Vector2 currentPosition2;
//		currentPosition2 = currentPosition3.x;
//		currentPosition2 = currentPosition3.y;

		//left-right rotation of dog
		if (x < 0) {
			transform.rotation = Quaternion.Euler (0, 0, 0);

			BoxCollider2D rot = GetComponent<BoxCollider2D>();
			Vector2 center = rot.center;
			center.x = boxoffset;
			rot.center = center;
		}
		else if (x > 0){
			transform.rotation = Quaternion.Euler(0, 180, 0);

			BoxCollider2D rot = GetComponent<BoxCollider2D>();
			Vector2 center = rot.center;
			center.x = -boxoffset;
			rot.center = center;
		}
		if (y > 0.5) {
			animator.SetInteger("Direction", 0);
		}
		else if (y < -0.5) {
			animator.SetInteger("Direction", 2);
		}
		else if (x > 0.5) {
			animator.SetInteger("Direction", 1);
			animator.SetInteger("Direction", 3);
		}

		//dog changing left-right direction (and hasnt hit max speed)
		if(x * rigidbody2D.velocity.x < maxSpeed)
			rigidbody2D.AddForce(Vector2.right * x * moveForce);

		// if dog velocity is greater than his max speed then set dog velocity to maxspeed
		if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);

		//dog changing up-down direction (and hasnt hit max speed)
			if(y * rigidbody2D.velocity.y < maxSpeed)
				rigidbody2D.AddForce(Vector2.up * y * moveForce);

		// if dog velocity is greater than his max speed then set dog velocity to maxspeed
		if(Mathf.Abs(rigidbody2D.velocity.y) > maxSpeed)
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.y) * maxSpeed, rigidbody2D.velocity.x);

		//after moving far enough, will spawn a footprint
		timeSinceFootprint += Mathf.FloorToInt(rigidbody2D.velocity.magnitude);
		if (timeSinceFootprint >= footprintInterval) {

			SpriteRenderer sr = GetComponent<SpriteRenderer>();

			//get the feet of the sprite
			float yOffset = sr.bounds.size.y * footprintOffsetY;
			//TODO: two sets of legs, once animation is added check if X offsets must be added
			//float xOffset = sr.bounds.size.x /4;
			//Vector3 fPos = transform.position-Vector3.up*yOffset+Vector3.left*xOffset;
			Vector3 fPos = transform.position-Vector3.up*yOffset;
			sr.sortingLayerName = "Betsy";
			sr.sortingOrder = 1;

			Quaternion r = Quaternion.Euler(transform.position);
			Transform f = Instantiate (footprint, fPos,r) as Transform;
			//scale up
			f.localScale = transform.localScale*2;

			//rotates 50 degrees along X so it looks like it's on the ground
			//rotates to face the direction that the player is moving
			if (rigidbody2D.velocity.x<0)
				f.Rotate(50,0,Vector2.Angle(Vector2.up,rigidbody2D.velocity));
			else f.Rotate(50,0,180+Vector2.Angle(Vector2.up,-rigidbody2D.velocity));

			//TODO: 

			//alternating footsteps are left then right
			if (leftFootForwards)
				f.Translate(Vector3.left*0.2f);
			else f.Translate(Vector3.right*0.2f);

			timeSinceFootprint = 0;
			leftFootForwards ^= true; //alternate between true and false
		}
	}
}
