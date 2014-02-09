using UnityEngine;
using System.Collections;

public class SceneChange : MonoBehaviour {

	public string destinationScene = "";
	public Vector2 spawnPosition;
	SpriteRenderer sr;

	void Start() {
		sr = GetComponent<SpriteRenderer>();
	}


	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			//todo: play animation?
			Dog.spawnPosition = spawnPosition;
			StartCoroutine(fade());
		}
	}

	IEnumerator fade() {
		for (int i=0;i<50;i++) {
			//TODO: move as it fades?
			Color c = sr.color;
			c.r -= 0.02f;
			c.g -= 0.02f;
			c.b -= 0.02f;
			sr.color = c;
			yield return new WaitForSeconds(0.01f);
		}
		Application.LoadLevel(destinationScene);
	}

}
