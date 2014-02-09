using UnityEngine;
using System.Collections;

public class Dog : MonoBehaviour 
{
	private Vector2 targetPosition = Vector2.zero;
	private float boxoffset = 0;
	private int timeSinceFootprint = 0;

	public float moveForce = 10f;			// added force to move the dog
	public float maxSpeed = 0.5f;				// max dog speed
	public Transform footprint;				// footprint prefab/sprite to spawn
	public int footprintInterval = 30;		// distance between each footprint
	public float footprintOffsetY = 0.4f;	// unit vectors downwards

	// Use this for initialization
	void Start () 
	{
		boxoffset = GetComponent<BoxCollider2D> ().center.x;
	}

	void Update ()
	{
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
		if (Input.GetAxis ("Horizontal") < 0) {
			transform.rotation = Quaternion.Euler (0, 0, 0);

			BoxCollider2D rot = GetComponent<BoxCollider2D>();
			Vector2 center = rot.center;
			center.x = boxoffset;
			rot.center = center;
		}
		else if (Input.GetAxis("Horizontal") > 0){
			transform.rotation = Quaternion.Euler(0, 180, 0);

			BoxCollider2D rot = GetComponent<BoxCollider2D>();
			Vector2 center = rot.center;
			center.x = -boxoffset;
			rot.center = center;
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
			timeSinceFootprint = 0;
			SpriteRenderer sr = GetComponent<SpriteRenderer>();
			//get the feet of the sprite (/3.0f is a rough adjustment)
			float yOffset = sr.bounds.size.y * footprintOffsetY;
			//footprint is at the bottom of the transform, and behind it
			Vector3 fPos = transform.position-Vector3.up*yOffset-Vector3.back*2;
			
			Transform f = Instantiate (footprint, fPos,Quaternion.identity) as Transform;
			f.localScale = transform.localScale*2;
		}
	}
}
