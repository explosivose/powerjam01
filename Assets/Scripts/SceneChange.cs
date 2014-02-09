using UnityEngine;
using System.Collections;

public class SceneChange : MonoBehaviour {

	public string destinationScene = "";
	public Vector2 spawnPosition;
	private bool dogIsClose = false;
	private Transform doggydogg;

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			//todo: play animation?
			Dog.spawnPosition = spawnPosition;
			Application.LoadLevel(destinationScene);
		}
	}

	void Update() {
		if (dogIsClose) {
			SpriteRenderer sr = GetComponent<SpriteRenderer>();
			Color c = sr.color;
			Debug.Log(Vector2.Distance(transform.position,doggydogg.position));
			c.a = 5-Vector2.Distance (transform.position,doggydogg.position);
			sr.color = c;
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{	
		if (coll.tag == "Player") {
			dogIsClose = true;
			doggydogg = coll.transform;
		}
	}
	void OnTriggerLeave2D(Collider2D coll)
	{	
		if (coll.tag == "Player") {
			SpriteRenderer sr = GetComponent<SpriteRenderer>();
			dogIsClose = false;
			Color c = sr.color;
			Debug.Log(Vector2.Distance(transform.position,coll.transform.position));
			c.a = 0;
			sr.color = c;
		}
	}
}
