using UnityEngine;
using System.Collections;

public class Highlightclickable : MonoBehaviour {

	private Color startColor;

	void OnMouseEnter()
	{
		startColor = transform.renderer.material.color;
		transform.renderer.material.color = Color.yellow;
	}
	void OnMouseExit()
	{
		transform.renderer.material.color = startColor;
	}

	// Use this for initialization
	void Start () {
	 startColor = transform.renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
