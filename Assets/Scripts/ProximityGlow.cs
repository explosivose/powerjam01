using UnityEngine;
using System.Collections;

public class ProximityGlow : MonoBehaviour {

	private bool dogIsClose = false;
	private Transform doggydogg;
	
	void Update() {
		if (dogIsClose) {
			SpriteRenderer sr = GetComponent<SpriteRenderer>();
			Color c = sr.color;
			c.a = GetComponent<CircleCollider2D>().radius-Vector2.Distance (transform.position,doggydogg.position);
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
			c.a = 0;
			sr.color = c;
		}
	}
}
