using UnityEngine;
using System.Collections;

public class FootprintSuicide : MonoBehaviour {

	SpriteRenderer sr;
	public float startOpacity = 0.15f;
	public int secondsBeforeFade = 5;
	//TODO (optional): fade duration

	// Use this for initialization
	void Start () {
		sr =  GetComponent<SpriteRenderer>();
		//start at 0.5 opacity
		Color c = sr.color;
		c.a = startOpacity;
		sr.color = c;

		StartCoroutine(suicide());
	}

	IEnumerator suicide() {
		yield return new WaitForSeconds(secondsBeforeFade);
		//fade out linearly over 10 seconds
		for (float o = startOpacity;o>0;o-=0.01f) {
			yield return new WaitForSeconds(0.1f);
			//lower opacity of the spriterenderer
			Color c = sr.color;
			c.a = o;
			sr.color = c;
		}

		Destroy (gameObject);
	}
}
