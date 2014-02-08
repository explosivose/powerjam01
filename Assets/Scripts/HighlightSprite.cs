using UnityEngine;
using System.Collections;

public class HighlightSprite : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	private Color startColor;

	// Use this for initialization
	void Start ()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
		startColor = spriteRenderer.color;
	}

	void OnMouseEnter()
	{
		spriteRenderer.color = Color.yellow;
	}

	void OnMouseExit()
	{
		spriteRenderer.color = startColor;
	}
}
