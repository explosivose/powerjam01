using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GUIWindow
{
	/* GUIWindow info
	* This class is serializable for the designer to choose window settings
	* from within the unity inspector. Settings include position and size.
	*/
	public enum DimensionMode
	{
		PercentageOfScreen,
		Absolute
	}
	
	public enum Alignment
	{
		UpperLeft,
		UpperCenter,
		UpperRight,
		MiddleLeft,
		MiddleCenter,
		MiddleRight,
		LowerLeft,
		LowerCenter,
		LowerRight
	}
	
	public int DesignHeight;
	public DimensionMode HeightIs = DimensionMode.Absolute;
	
	public int Height 
	{
		get
		{
			if (HeightIs == DimensionMode.Absolute)
				return DesignHeight;
			else
				return Mathf.RoundToInt(Screen.height * DesignHeight / 100);
		}
		set
		{
			DesignHeight = value;
		}
	}
	
	public int DesignWidth;
	public DimensionMode WidthIs = DimensionMode.Absolute;
	
	public int Width
	{
		get
		{
			if (WidthIs == DimensionMode.Absolute)
				return DesignWidth;
			else
				return Mathf.RoundToInt(Screen.width * DesignWidth / 100);
		}
		set
		{
			DesignWidth = value;
		}
	}
	
	
	public Alignment Align = Alignment.UpperLeft;
	
	// Top side of the window in screen pixels
	public int verticalOffset;
	public int Top
	{
		get
		{
			// depends on the alignment mode 
			switch (Align)
			{
			case Alignment.UpperCenter: 
			case Alignment.UpperLeft: 
			case Alignment.UpperRight:
			default:
				return verticalOffset;
				
			case Alignment.MiddleCenter: 
			case Alignment.MiddleLeft: 
			case Alignment.MiddleRight:
				return Mathf.RoundToInt(Screen.height/2 - Height/2) + verticalOffset;
				
			case Alignment.LowerCenter: 
			case Alignment.LowerLeft: 
			case Alignment.LowerRight:
				return Screen.height - Height - verticalOffset;
				
			}
		}
		set
		{
			verticalOffset = value;
		}
	}
	
	// Left side of the window in screen pixels
	public int horizontalOffset;
	public int Left
	{
		get
		{
			// depends on the alignment mode
			switch (Align)
			{
			case Alignment.LowerLeft: 
			case Alignment.MiddleLeft: 
			case Alignment.UpperLeft:
			default:
				return horizontalOffset;
				
			case Alignment.LowerCenter: 
			case Alignment.MiddleCenter: 
			case Alignment.UpperCenter:
				return Mathf.RoundToInt(Screen.width/2 - Width/2) + horizontalOffset;
				
			case Alignment.LowerRight: 
			case Alignment.MiddleRight: 
			case Alignment.UpperRight:
				return Screen.width - Width - horizontalOffset;
				
			}
		}
		set
		{
			horizontalOffset = value;
		}
	}
	
	
}


/// <summary>
/// All the GUI code exists in this class.
/// </summary>
public class GUIManager : MonoBehaviour
{
	public int buttonHeight = 25;
	// these are window properties for the designer to edit 
	// using the unity inspector
	public GUIWindow mainMenu = new GUIWindow();
	public GUIWindow credits = new GUIWindow();

	public static bool GUIManagerSpawned = false;

	private enum GUIState
	{
		MainMenu,
		Credits,
		NoWindows
	}

	public void ShowMainMenu()
	{
		state = GUIState.MainMenu;
	}
	
	public void ShowCredits()
	{
		state = GUIState.Credits;
	}
	
	public void ShowNoWindows()
	{
		state = GUIState.NoWindows;
	}
	
	

	
	private GUIState state = GUIState.MainMenu;
	
	
	private Rect windowSize = new Rect();
	private GUISkin menuSkin;
	
	void Awake()
	{
		menuSkin = (GUISkin)Resources.Load ("Menus", typeof(GUISkin));
		GUIManagerSpawned = true;
	}
	
	/// <summary>
	/// Main Menu GUI function.
	/// </summary>
	void wMainMenu(int windowID)
	{
		// Gap for the header
		GUILayout.Space(15);
		
		// Resume: Hide the main menu
		if (GUILayout.Button ("Resume Game", menuSkin.button, GUILayout.Height (buttonHeight))) 
			state = GUIState.NoWindows;
		
		// New game:  Load the first scene
		GUILayout.Space(5);
		if (GUILayout.Button ("New Game", menuSkin.button, GUILayout.Height (buttonHeight))) 
		{
			Dog.spawnPosition = Vector2.one / 2f;
			Application.LoadLevel(0);
			state = GUIState.NoWindows;
		}
		
		// Credits
		GUILayout.Space(5);
		if (GUILayout.Button ("Credits", menuSkin.button, GUILayout.Height (buttonHeight)))
			state = GUIState.Credits;
		
		// Quit
		GUILayout.Space(5);
		if ( !Application.isWebPlayer )
		{
			if ( GUILayout.Button ("Quit", menuSkin.button, GUILayout.Height (buttonHeight)) )
				Application.Quit();
		}
	}
	

	
	void wOptions(int windowID)
	{
		float audioVol = AudioListener.volume;
		
		// Gap for header
		GUILayout.Space(15);
		
		GUILayout.Label("Audio Volume:");
		AudioListener.volume = GUILayout.HorizontalSlider(audioVol, 0f, 1f);
		
		// Back to main menu button
		GUILayout.Space(5);
		if ( GUILayout.Button ("Main Menu", menuSkin.button, GUILayout.Height(buttonHeight)) )
		{
			PlayerPrefs.SetFloat("audioVolume", audioVol);
			state = GUIState.MainMenu;
		}
	}
	
	void wCredits(int windowID)
	{
		GUILayout.Space (15);
		
		GUILayout.Label ("SuperCore Game Jam February 8th/9th 2014", menuSkin.label);
		GUILayout.Space (5);
		
		// Back to main menu button
		GUILayout.Space(20);
		if ( GUILayout.Button ("Main Menu", menuSkin.button, GUILayout.Height(buttonHeight)) )
			state = GUIState.MainMenu;
	}
	
	
	/// <summary>
	/// Called every fixed update.
	/// Decide which window to draw.
	/// </summary>
	void FixedUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && state == GUIState.NoWindows) 
		{
			state = GUIState.MainMenu;
		}
		else if ( Input.GetKeyDown(KeyCode.Escape) && state != GUIState.NoWindows)
		{
			state = GUIState.NoWindows;
		}
	}
	
	
	
	/// <summary>
	/// Draws the window.
	/// </summary>
	void OnGUI()
	{
		
		GUIWindow thisWindow = new GUIWindow();
		
		// Copy GUIWindow settings to thisWindow
		switch ( state )
		{
		case GUIState.MainMenu:
			thisWindow = mainMenu;
			break;
		case GUIState.Credits:
			thisWindow = credits;
			break;
		default:
			break;
		}
		
		windowSize = new Rect(thisWindow.Left, thisWindow.Top, thisWindow.Width, thisWindow.Height);
		
		// Draw thisWindow (GUILayout.Window)
		switch ( state )
		{
		case GUIState.MainMenu:
			GUILayout.Window (1, windowSize, wMainMenu, "I'm Lost!", menuSkin.window);
			break;
		case GUIState.Credits:
			GUILayout.Window (1, windowSize, wCredits, "Credits", menuSkin.window);
			break;
		default:
			break;
		}
	}
	
}