using UnityEngine;
using System; // i'm using Array class from here 
using System.Collections;

// Each object in the scene that starts off unsaturated has this script.
// As the dog inspects these objects their color saturation returns!
public class SaturatableObject : MonoBehaviour 
{
	
	private SpriteRenderer r;
	private Texture2D currentTexture;
	private Texture2D originalTexture;
	private Color[] originalPixels;
	private Dog dogScript;
	private Transform sniffer;
	
	// Save the original sprite texture 2D information before desaturation
	void Awake() 
	{
		// Save original sprite texture data
		r = GetComponent<SpriteRenderer>();
		
		// reference to the active component
		currentTexture = r.sprite.texture;
		originalPixels = currentTexture.GetPixels();
		
		// a copy of the original texture before game starts
		// 
		originalTexture = new Texture2D(currentTexture.width, currentTexture.height);
		originalTexture.SetPixels( originalPixels );
		originalTexture.Apply();
		
		// Set low initial color saturation
		SetSaturationMultiplier(0.1f);
		
		
		// get reference to player transform
		dogScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Dog>();;
		sniffer = GameObject.FindGameObjectWithTag("Sniffer").transform;
	}
	
	void Update()
	{
		if (Input.GetButton("Fire1"))
			SaturateAroundMousePointer();
			
		SaturateAroundDog();
	}
	
	void SaturateAroundDog()
	{
		Vector3 pos = Camera.main.WorldToScreenPoint(sniffer.position);
		int size = Mathf.RoundToInt(dogScript.smellRadius);
		
		int l = Mathf.RoundToInt(pos.x - size/2);
		int b = Mathf.RoundToInt(pos.y - size/2);
		int h = size;
		int w = size;
		
		Debug.Log ("[" + l + ", " + b + ", " + h + ", " + w  + "]"); 
		
		SetSaturationMultiplier(1f, l, b, w, h);
	}
	
	void SetSaturationMultiplier(float m)
	{
		// GetPixels() has a bunch of useful overloads!
		Color[] current = r.sprite.texture.GetPixels();
		
		for (int i = 0; i < current.Length; i++)
		{
			HSBColor pixel = new HSBColor(originalPixels[i]);
			pixel.s *= m;
			current[i] = pixel.ToColor();
		}
		r.sprite.texture.SetPixels(current);
		r.sprite.texture.Apply();
	}
	
	void SetSaturationMultiplier(float m, int x, int y, int w, int h)
	{
		float heightFactor =  (float)r.sprite.texture.height / (float)Screen.height;
		float widthFactor = (float)r.sprite.texture.width / (float)Screen.width;
		
		//Debug.Log ("[" + x + ", " + y + ", " + h + ", " + w + "]");
		//Debug.Log (w + ", " + h);
		
		if ( x < 0 ) x = 0;
		if ( y < 0 ) y = 0;
		
		if ( x + w  > currentTexture.width) w = currentTexture.width - x;
		if ( y + h > currentTexture.height) h = currentTexture.height - y;
		
		Color[] current = r.sprite.texture.GetPixels(x, y, w, h);
		Color[] original = originalTexture.GetPixels(x, y, w, h);
		
		for (int i = 0; i < current.Length; i++)
		{
			HSBColor px = new HSBColor(original[i]);
			px.s *= m;
			current[i] = px.ToColor();
		}
		r.sprite.texture.SetPixels(x, y, w, h, current);
		r.sprite.texture.Apply();
		
	}
	
	void SaturateAroundMousePointer()
	{
		Vector3 mouse = Input.mousePosition;
		int brushSize = 128;
		int brushLeft = Mathf.RoundToInt(mouse.x - brushSize/2);
		int brushBottom = Mathf.RoundToInt(mouse.y - brushSize/2);
		int brushHeight = brushSize;
		int brushWidth = brushSize;
		
		Vector3 brush = Camera.main.ScreenToWorldPoint(mouse);
		brush.z = transform.position.z;
		Debug.DrawLine(Camera.main.transform.position, brush, Color.red, 2f);
		
		SetSaturationMultiplier(1f, brushLeft, brushBottom, brushHeight, brushWidth);
	}
	
	void OnApplicationQuit()
	{
		Resources.UnloadAsset(currentTexture);
	}
}
