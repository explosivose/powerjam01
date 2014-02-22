using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {

	//Basically a modified/butchered SceneChange.cs

	SpriteRenderer sr;
	int t;
	public Vector2 spawnPosition;
	
	void Start () {
		t=0;
		Dog.spawnPosition = new Vector2(0.25f,0.4f);
		sr = transform.GetComponent<SpriteRenderer>();
	}

	void FixedUpdate() {
		t++;
		if (t<300) {
			transform.Translate (0,0.0015f,0);
		}
		if (t>200) {
			StartCoroutine(fade());
		}
	}

	IEnumerator fade() {
		for (int i=0;i<99;i++) {
			Color c = sr.color;
			c.r -= 0.01f;
			c.g -= 0.01f;
			c.b -= 0.01f;
			sr.color = c;
			yield return new WaitForSeconds(0.03f);
		}
		Application.LoadLevel("scene0");
	}
}
