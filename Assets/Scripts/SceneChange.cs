using UnityEngine;
using System.Collections;

public class SceneChange : MonoBehaviour {

	public string destinationScene = "";
	public Vector2 spawnPosition;

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player") {
			//todo: play animation?
			Application.LoadLevel(destinationScene);
		}
	}
}
