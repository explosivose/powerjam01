using UnityEngine;
using System.Collections;

public class SceneChange : MonoBehaviour {

	public string destinationScene = "";
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "player")
			//todo: play animation?
			Application.LoadLevel(destinationScene);
	}
}
