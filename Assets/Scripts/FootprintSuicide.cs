using UnityEngine;
using System.Collections;

public class FootprintSuicide : MonoBehaviour {

	//TODO: FADE OUT BEFORE SUICIDE

	// Use this for initialization
	void Start () {
		StartCoroutine(suicide());
	}

	IEnumerator suicide() {
		yield return new WaitForSeconds(2);
		Destroy (gameObject);
	}
}
