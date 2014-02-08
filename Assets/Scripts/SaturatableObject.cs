using UnityEngine;
using System.Collections;

// Each object in the scene that starts off unsaturated has this script.
// As the dog inspects these objects their color saturation returns!
public class SaturatableObject : MonoBehaviour 
{
	
	private SpriteRenderer r;
	private Texture2D originalTexture;
	private Color[] originalPixels;
	
	// Save the original sprite texture 2D information before desaturation
	void Awake() 
	{
		// Save original sprite texture data
		r = GetComponent<SpriteRenderer>();
		
		originalTexture = r.sprite.texture;
		originalPixels = originalTexture.GetPixels();
		
		// Set low initial color saturation
		SetSaturationMultiplier(0.2f);
		
		// Start test coroutine
		StartCoroutine( TestRoutine() );
	}
	
	void SetSaturationMultiplier(float m)
	{
		
		int h = r.sprite.texture.height;
		int w = r.sprite.texture.width;
		// GetPixels() has a bunch of useful overloads!
		Color[] current = r.sprite.texture.GetPixels();
		
		if (current.Length != originalPixels.Length)
			Debug.LogError("WOOPS");
		
		for (int i = 0; i < current.Length; i++)
		{
			HSBColor c = new HSBColor(originalPixels[i]);
			c.s *= m;
			current[i] = c.ToColor();
		}
		r.sprite.texture.SetPixels(current);
		r.sprite.texture.Apply();
	}
	
	IEnumerator TestRoutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.05f);
			float input = Input.GetAxis("Horizontal") + 1f;
			SetSaturationMultiplier(input);
		}
	}
	
	void OnApplicationQuit()
	{
		Resources.UnloadAsset(originalTexture);
	}
}
