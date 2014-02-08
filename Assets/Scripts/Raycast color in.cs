using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	Vector3 v;
	Vector3 mousePos;
	RaycastHit hit;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
	{

		if(Input.GetButtonDown ("Fire1"))
		{
			mousePos.x = Input.mousePosition.x;
			mousePos.y = Input.mousePosition.y;
			mousePos.z = 100;
			v = Camera.main.ScreenToWorldPoint(mousePos);
			Debug.Log (Input.mousePosition);
			Debug.Log (v);

			if (Physics.Raycast (Camera.main.transform.position, v, out hit) && hit.transform.name == "Cube")
			{
				Debug.Log ("CUBE HIT");
				hit.transform.renderer.material.color = Color.yellow;
			}
		}
		Debug.DrawRay(Camera.main.transform.position, v, Color.green);
	}
}
