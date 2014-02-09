using UnityEngine;
using System.Collections;

public class HighlightSprite : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	private BoxCollider2D boxCollider2D;
	private Color startColor;
	private string label;
	private Vector3 guiXPos;
	private Vector3 guiYPos;
	private Vector3 screenPos;
	private bool showLabel;

	//Use this for initialization
	void Start ()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
		boxCollider2D = GetComponent<BoxCollider2D> ();
		startColor = spriteRenderer.color;
		label = transform.name;
		screenPos = Vector3.zero;
		showLabel = false;
	}

	void OnMouseEnter()
	{
		spriteRenderer.color = Color.yellow;
		showLabel = true;
	}

	void OnMouseExit()
	{
		spriteRenderer.color = startColor;
		showLabel = false;
	}

	void OnGUI()
	{
		if (showLabel)
		{
		screenPos = Camera.main.WorldToScreenPoint(transform.position);
		GUI.Label(new Rect((screenPos.x - (boxCollider2D.size.x * 9)), (screenPos.y - (boxCollider2D.size.x * 7)), transform.name.Length*10, 22), label);
		}
	}
}
