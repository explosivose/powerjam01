using UnityEngine;
using System.Collections;

public class Dog : MonoBehaviour 
{
	private Animator animator;
	private Vector2 targetPosition = Vector2.zero;
	private float boxoffset = 0;
	private int timeSinceFootprint = 0;
	private bool leftFootForwards = false;
	private float PCHeight = 0f;
	
	public float moveForce = 100f;			// added force to move the dog
	public float maxSpeed = 50f;			// max dog speed
	public Transform footprint;				// footprint prefab/sprite to spawn
	public int footprintInterval = 30;		// distance between each footprint
	public float footprintOffsetY = 0.4f;	// unit vectors downwards
	public static Vector2 spawnPosition;	// as a percentage of screen height/width (0 to 1)
	
	public float smellRadius = 0f;
	public float maxSmellRadius = 10f;
	public float minSmellRadius = 4f;
	
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
	
	void Awake()
	{
	// Spawn the gui manager (main menus and stuff)
		if (!GUIManager.GUIManagerSpawned)
		{
			GameObject GUIManagerPrefab = (GameObject)Resources.Load ("GUIManager", typeof(GameObject));
			GUIManagerPrefab = Instantiate(GUIManagerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			DontDestroyOnLoad(GUIManagerPrefab);
		}
	}
	
	
	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator>();
		boxoffset = GetComponent<BoxCollider2D> ().center.x;
		// calculate how far the doggy can smell (smells further when he's sitting still)
		StartCoroutine( SmellRadius() );
	}

	void Update ()
	{
		Debug.Log (rigidbody2D.velocity.magnitude);
		Vector3 dogScale = transform.localScale;
		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		PCHeight = 1-(screenPos.y / Screen.height);
		if (PCHeight < 0.2) //min height 0.5
			PCHeight = 0.2f;
		dogScale = PCHeight * Vector3.one;
		transform.localScale = dogScale;

	}


	// Update is called once per frame
	void FixedUpdate () 
	{
		// Take keyboard input and work out a target position
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

		if (x == 0 && y == 0)
			animator.SetInteger ("animate", 0);
		else
			animator.SetInteger ("animate", 1);

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

		//dog changing left-right direction (and hasnt hit max speed)
		if(x * rigidbody2D.velocity.x < maxSpeed) {
			rigidbody2D.AddForce(Vector2.right * x * moveForce);
		}

		// if dog velocity is greater than his max speed then set dog velocity to maxspeed
		if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);

		//dog changing up-down direction (and hasnt hit max speed)
		if(y * rigidbody2D.velocity.y < maxSpeed) {
			rigidbody2D.AddForce(Vector2.up * y * moveForce);
		}

		// if dog velocity is greater than his max speed then set dog velocity to maxspeed
		if(Mathf.Abs(rigidbody2D.velocity.y) > maxSpeed) {
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.y) * maxSpeed, rigidbody2D.velocity.x);
		}

		//after moving far enough, will spawn a footprint
		timeSinceFootprint += Mathf.FloorToInt(rigidbody2D.velocity.magnitude);
		if (timeSinceFootprint >= footprintInterval) {

			SpriteRenderer sr = GetComponent<SpriteRenderer>();

			//get the feet of the sprite
			float yOffset = sr.bounds.size.y * footprintOffsetY;
			//TODO: does it need two sets of footprints or will animation cover it up
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
	
	
	
	IEnumerator SmellRadius()
	{
		while (true)
		{
			float startTime = Time.time;
			bool moving = false;
			while(moving)
			{
				float t = (Time.time - startTime)/ 5f;
				smellRadius = Mathf.Lerp(maxSmellRadius, minSmellRadius, t);
				smellRadius *= PCHeight;
				//Debug.DrawLine(transform.position, transform.position + Vector3.up * smellRadius);
				yield return new WaitForFixedUpdate();
				if (rigidbody2D.velocity.magnitude < 0.1f){
					moving = false;
				}
			}
			
			while(!moving)
			{
				float t = (Time.time - startTime)/ 3f;
				smellRadius = Mathf.Lerp(minSmellRadius, maxSmellRadius, t);
				smellRadius *= PCHeight;
				//Debug.DrawLine(transform.position, transform.position + Vector3.up * smellRadius);
				yield return new WaitForFixedUpdate();
				if (rigidbody2D.velocity.magnitude > 0.1f)
					moving = true;
			}

		}
	}
	
	void OnGUI()
	{
		GUI.Label (new Rect(5,5,100,100), Application.loadedLevelName);
	}
}
