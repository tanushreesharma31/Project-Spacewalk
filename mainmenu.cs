using UnityEngine;
using System.Collections;
using Design; //Loads the custom UI from  Scripts/Library/design.cs
using Mission;

[RequireComponent(typeof(AudioSource))]
public class mainmenu : MonoBehaviour {

	// Font
	private Font futura_font = null;

	// Images
	private Texture2D background_splash = null,
	background_current = null,
	background_about = null,
	background_exit = null, 
	background_login = null, 
	background_play = null,
	background_selectlevel= null,
	background_settings = null,
	button_blue = null,
	button_blue_hover = null,
	button_red = null,
	button_red_hover = null,
	button_yellow = null,
	button_yellow_hover = null,
	button_blank = null,
	button_mainmenu = null,
	button_mainmenu_hover = null,
	button_level01=null,
	button_level01_hover = null,
    button_level02_hover = null,
    button_level02 = null,
	button_level02_lock = null;

	// LogIn string
	private string username = "",
	password= "";

	// Display error message if the user does not login
	private bool isUserID = true;

	// This pivot determines what menu state that the user is current.
	//private int menu_state = 0;

	// Timer for the splash screen
	private float opacity_set = 0.0f;

	// Load audio clips
	private AudioClip sound_error, sound_click, sound_success;
	private bool isSound = false; //This boolean will prevent the sound loop
	private bool isNewState = false;

	private AudioSource music_background;

	// ************************************************************************
	// Use this for initialization
	void Start () {
		//Note: All images, sounds, 3D models, and other objects must be stored in the "Resources" folder.

		//Load background image
		background_splash = (Texture2D)Resources.Load("images/splash-image");
		background_about = (Texture2D)Resources.Load("images/mm_about");
		background_exit = (Texture2D)Resources.Load("images/mm_exit");
		background_login = (Texture2D)Resources.Load("images/mm_login");
		background_play = (Texture2D)Resources.Load("images/mm-menu-play");
		background_selectlevel = (Texture2D)Resources.Load("images/mm_selectlevel");
		background_settings = (Texture2D)Resources.Load("images/mm_setting");
		
		// Load font
		futura_font = (Font)Resources.Load("fonts/FuturaBook BT");

		// Pass font to the library, so other scenes can use this font without declaring
		design.Font_Futura = futura_font;


		//Load button background color
		button_blue = (Texture2D)Resources.Load("images/button-blue");
		button_blue_hover = (Texture2D)Resources.Load("images/button-blue_hover");
		button_red = (Texture2D)Resources.Load("images/button-red");
		button_red_hover = (Texture2D)Resources.Load("images/button-red_hover");
		button_yellow = (Texture2D)Resources.Load("images/button-yellow");
		button_yellow_hover = (Texture2D)Resources.Load("images/button-yellow_hover");
		button_blank = (Texture2D)Resources.Load("images/button-blank");
		button_mainmenu = (Texture2D)Resources.Load("images/button_level_mainmenu");
		button_mainmenu_hover = (Texture2D)Resources.Load("images/buttton_level_mainmenu");
		button_level01 = (Texture2D)Resources.Load("images/sl_button_level01");
		button_level01_hover = (Texture2D)Resources.Load("images/sl_button_level01_hover");
        button_level02_hover = (Texture2D)Resources.Load("images/sl_button_level02_hover");
        button_level02 = (Texture2D)Resources.Load("images/sl_button_level02");
		button_level02_lock = (Texture2D)Resources.Load("images/sl_button_level02_lock");

		// Start with the splash screen
		//menu_state = 0;


		// Opacting set to 0
		opacity_set = 0.0f;

		// By default, set the play background image as a startup (after the user login)
		background_current = background_play;

		// Get audio clips
		sound_error = (AudioClip)Resources.Load("sounds/beep-low");
		sound_click = (AudioClip)Resources.Load("sounds/click");
		sound_success = (AudioClip)Resources.Load("sounds/beep-high");

		// Add background Music to the main menu
		music_background = (AudioSource)gameObject.AddComponent <AudioSource>();
		AudioClip music_source;
		music_source = (AudioClip)Resources.Load ("sounds/mm_theme");
		music_background.clip = music_source;
		music_background.Play ();
		music_background.loop = true;


	}

	// ************************************************************************
	// Update is called once per frame
	void Update () {

		// Display the splash screen from the beginning of the game
		if(((Time.timeSinceLevelLoad) > 3)&&((Time.timeSinceLevelLoad) <= 5)) {
			if(opacity_set < 1.0f)
				opacity_set += Time.deltaTime;
		}

		//Between 5-6 seconds, go to the login menu
		if ((Time.timeSinceLevelLoad >= 5)&&(Time.timeSinceLevelLoad <= 6))
			mission.STATEPIVOT = 1;

		// Play an error sound
		if ((isUserID == false)&&(isSound == false)) {
			GetComponent<AudioSource>().clip = sound_error;
			GetComponent<AudioSource>().Play();
			isSound = true;
		}

		if((mission.STATEPIVOT == 2)&&(isSound == false))
		{
			GetComponent<AudioSource>().clip = sound_success;
			GetComponent<AudioSource>().Play();
			isSound = true;
		}

		if (isNewState == true) {
			GetComponent<AudioSource>().clip = sound_click;
			GetComponent<AudioSource>().Play();
			isNewState = false;
		}
	}

	//************************************************************************
	// Awake is called when the script instance is being loaded.
	void Awake() {
		background_current = background_play;
	}


	// ************************************************************************
	// OnGUI is called for rendering and handling GUI events.
	void OnGUI() {

		// Background is set to black
		GetComponent<Camera>().backgroundColor = Color.black;

		//Switch menu
		switch (mission.STATEPIVOT) {
		case 0:
			// Splash Screen
			mission.STATEPIVOT = 0;
				SPLASH_SCREEN();
				break;
			case 1:
				//Login
				LOGIN();
				opacity_set = 0.0f; // Reset the opacity level to 0
				break;
			case 2:
				//Main
				MAINMENU();
				break;
			case 3:
				//Settings
				SETTINGS();
				break;
			case 4:
				//About
				ABOUT();
				break;
			case 5:
				//Selects a level
				SELECT_LEVEL();
				break;
			default:
				break;
		}

    }




	// ************************************************************************
	// SPLASH_SCREEN
	private void SPLASH_SCREEN(){
		//Display the UHCL logo
		GUI.DrawTexture(new Rect(0, 0, background_splash.width, background_splash.height), background_splash);

		// Black background will fade the UHCL logo
		design.DrawRectangle (new Rect (0, 0, Screen.width, Screen.height), Color.black, opacity_set);
	}

	// ************************************************************************
	// LOGIN MENU
	private void LOGIN(){
		GUI.DrawTexture(new Rect(Screen.width-background_login.width, Screen.height-background_login.height, background_login.width, background_login.height), background_login);
		
		//Title of the game
		GUI.Label (new Rect (20, 50, Screen.width/2, 40), "P R O J E C T :", design.StyleText(design.Font_Futura, 35, TextAnchor.MiddleCenter, Color.white));
		GUI.Label (new Rect (20, 90, Screen.width/2, 70), "SPACEWALK", design.StyleText(futura_font, 65, TextAnchor.MiddleCenter, Color.white));
		
		//Login instructions
		GUI.Label (new Rect (20, 200, Screen.width/2, 50), "Enter your username and password.", design.StyleText(design.Font_Futura, 25, TextAnchor.MiddleLeft, Color.white));
		GUI.Label (new Rect (100, 280, Screen.width/2, 50), "USERNAME:", design.StyleText(design.Font_Futura, 25, TextAnchor.MiddleLeft, Color.white));
		GUI.Label (new Rect (100, 340, Screen.width/2, 50), "PASSWORD:", design.StyleText(design.Font_Futura, 25, TextAnchor.MiddleLeft, Color.white));
		
		//Register new user label
		GUI.Label (new Rect (190, Screen.height-75, 200, 30), "NEW USER?",design.StyleText(design.Font_Futura, 20, TextAnchor.MiddleLeft, Color.white));
		GUI.Label (new Rect (190, Screen.height-50, 350, 30), "Press the yellow button to register",design.StyleText(design.Font_Futura, 20, TextAnchor.MiddleLeft, Color.white));
		
		//Creates a style of the GUI input fields
		var input_style = new GUIStyle();
		input_style.fontSize = 20;
		input_style.font = futura_font;
		
		design.DrawRectangle (new Rect (250, 275, 260, 50), Color.white, 1f); //Draws a white rectangle box
		
		//Username input field
		username = GUI.TextField(new Rect(255, 290, 260, 40), username, 25,input_style);
		
		design.DrawRectangle (new Rect (250, 340, 260, 50), Color.white, 1f); //Draws a white rectangle box
		
		//Password input field
		password = GUI.PasswordField(new Rect(260, 355, 260, 40), password,"*"[0], 15,input_style);
		
		//EXIT BUTTON - TERMINATES THE PROGRAM
		if(GUI.Button (new Rect (Screen.width-150, 0, 150, 60), new GUIContent("EXIT", "EXIT"), design.StyleButton(futura_font, 24, Color.white, button_red, button_red_hover))){
			Application.Quit ();
		}
		
		//LOGIN BUTTON
		if(GUI.Button (new Rect (360, 410, 150, 60), new GUIContent("LOGIN", "LOGIN"), design.StyleButton(futura_font, 24, Color.white, button_blue, button_blue_hover))){
			
			if((username.ToString() == "") || (password.ToString() == "")){
				//Display an error screen
				isUserID = false;
			}
			
			if((username.ToString() != "") && (password.ToString() != "")){
				//The player enters to the main menu and resets the username & password
				mission.STATEPIVOT = 2;
				username = "";
				password= "";
			}
		}
		
		
		//REGISTER BUTTON
		if(GUI.Button (new Rect (20, Screen.height-80, 150, 60), new GUIContent("REGISTER", "REGISTER"), design.StyleButton(futura_font, 24, Color.black, button_yellow, button_yellow_hover))){
			//Later, include the registration scene
		}
		
		if(isUserID == false){
			//If the user does not enter the username or password, then display the login error
			design.DrawRectangle (new Rect (0,0,Screen.width, Screen.height), Color.black, 0.9f);
			GUI.Label (new Rect (50, Screen.height/4, 300, 120), "ALERT!", design.StyleText(futura_font, 70, TextAnchor.MiddleLeft, Color.white));
			GUI.Label (new Rect (50, Screen.height/4, Screen.width, 240), "Enter your correct username or password.", design.StyleText(futura_font, 30, TextAnchor.MiddleLeft, Color.white));
			
			if(GUI.Button (new Rect (620, 280, 150, 60), new GUIContent("OKAY", "OKAY"), design.StyleButton(futura_font, 24, Color.white, button_red, button_red_hover))){
				isUserID = true;
				isSound = false;
			}
		}
	}



	//************************************************************************
	//Main Menu
	private void MAINMENU(){
		//background_current = background_play;
		GUI.DrawTexture(new Rect(0, 0, background_current.width, background_current.height), background_current);
		
		design.DrawRectangle(new Rect (0, 0, Screen.width/2, Screen.height), Color.black, 0.75f);
		
		//Title of the game
		GUI.Label (new Rect (20, 50, Screen.width/2, 40), "P R O J E C T :", design.StyleText(futura_font, 35, TextAnchor.MiddleCenter, Color.white));
		GUI.Label (new Rect (20, 90, Screen.width/2, 70), "SPACEWALK", design.StyleText(futura_font, 65, TextAnchor.MiddleCenter, Color.white));
		
		var menu_buttons = new GUIStyle();
		menu_buttons = new GUIStyle(GUI.skin.button);
		menu_buttons.normal.textColor = Color.white;
		menu_buttons.normal.background = button_blank;
		menu_buttons.hover.textColor  = Color.cyan;
		menu_buttons.hover.background = button_blue;
		menu_buttons.font = futura_font;
		menu_buttons.fontSize = 44;
		
		//Play
		if(GUI.Button (new Rect (0, 190, Screen.width/2, 100), new GUIContent("PLAY", "PLAY"), menu_buttons)){
			mission.STATEPIVOT = 5;
			isNewState = true;
		}
		
		//Settings
		if(GUI.Button (new Rect (0, 290, Screen.width/2, 100), new GUIContent("SETTINGS", "SETTINGS"), menu_buttons)){
			mission.STATEPIVOT = 3;
			isNewState = true;
		}
		
		//About
		if(GUI.Button (new Rect (0, 390, Screen.width/2, 100), new GUIContent("ABOUT", "ABOUT"), menu_buttons)){
			mission.STATEPIVOT = 4;
			isNewState = true;
		}
		
		//Exit
		if(GUI.Button (new Rect (0, 490, Screen.width/2, 100), new GUIContent("EXIT", "EXIT"), menu_buttons)){
			mission.STATEPIVOT = 1;
			isNewState = true;
		}
		
		switch (GUI.tooltip) {
		case "PLAY":
			background_current = background_play;
			break;
		case "SETTINGS":
			background_current = background_settings;
			break;
		case "ABOUT":
			background_current = background_about;
			break;
		case "EXIT":
			background_current = background_exit;
			break;
		}
	}
	
	//************************************************************************
	//SETTINGS
	private void SETTINGS(){
		GUI.DrawTexture(new Rect(Screen.width-background_settings.width, Screen.height-background_settings.height, background_settings.width, background_settings.height), background_settings);
		design.DrawRectangle(new Rect (Screen.width-background_settings.width, Screen.height-background_settings.height, background_settings.width, background_settings.height), Color.black, 0.75f); //field: class code
		
		GUI.Label (new Rect (10, 10, 400, 20), "P R O J E C T : S P A C E W A L K", design.StyleText(futura_font, 14, TextAnchor.MiddleLeft, Color.white));
		GUI.Label (new Rect (10, 25, 400, 50), "S E T T I N G S", design.StyleText(futura_font, 44, TextAnchor.MiddleLeft, Color.white));
		
		GUI.Label (new Rect (10, 55, 600, 100), "You can change your password or class code.",design.StyleText(futura_font, 16, TextAnchor.MiddleLeft, new Color(94/255.0f, 240/255.0f, 255/255.0f, 1f)));
		
		GUI.Label (new Rect (0, 150, 600, 100), "Password\nChange",design.StyleText(futura_font, 24, TextAnchor.MiddleCenter, Color.white));
		GUI.Label (new Rect (300, 150, 600, 100), "Class Code\nChange",design.StyleText(futura_font, 24, TextAnchor.MiddleCenter, Color.white));
		
		//RESET CLASS CODE
		if(GUI.Button (new Rect ((Screen.width/2)-250, 320, 150, 60), new GUIContent("RESET", "RESET"), design.StyleButton(futura_font, 24, Color.white, button_blue, button_blue_hover))){
			
		}
		
		//CHANGE PASSWORD
		if(GUI.Button (new Rect ((Screen.width/2)+50, 320, 150, 60), new GUIContent("CHANGE", "CHANGE"), design.StyleButton(futura_font, 24, Color.white, button_blue, button_blue_hover))){
			
		}
		
		//RETURN BUTTON
		if(GUI.Button (new Rect (10, Screen.height-80, 150, 60), new GUIContent("RETURN", "RETURN"), design.StyleButton(futura_font, 24, Color.white, button_blue, button_blue_hover))){
			mission.STATEPIVOT = 2;
		}
	}
	
	//************************************************************************
	//ABOUT
	private void ABOUT(){
		GUI.DrawTexture(new Rect(Screen.width-background_about.width, Screen.height-background_about.height, background_about.width, background_about.height), background_about);
		design.DrawRectangle(new Rect (Screen.width-background_about.width, Screen.height-background_about.height, background_about.width, background_about.height), Color.black, 0.75f);
		
		GUI.Label (new Rect (10, 10, 400, 20), "P R O J E C T : S P A C E W A L K", design.StyleText(futura_font, 14, TextAnchor.MiddleLeft, Color.white));
		GUI.Label (new Rect (10, 25, 400, 50), "A B O U T", design.StyleText(futura_font, 44, TextAnchor.MiddleLeft, Color.white));
		
		
		string version = "Project: SpaceWalk\nVersion: 1.00.0\nUniversity of Houston-Clear Lake Copyright 2014\n";
		string credits = "Reginald Leathers, Program Manager\n" +
			"Bernard Cannariato, Animator\n" +
				"Joseph Carroll, Animator\n" +
				"Carlos Lacayo, User Interface Designer and Lead Programmer\n" +
				"Zhiyi Chen, Programmer\n" +
				"Zhuo Qi, Programmer\n" +
				"Ann M. Henry, Principal Investigator\n\n";
		
		GUI.Label (new Rect (10, 155, 600, 500), credits + version,design.StyleText(futura_font, 16, TextAnchor.MiddleLeft, Color.white));
		
		
		//RETURN BUTTON
		if(GUI.Button (new Rect (10, Screen.height-80, 150, 60), new GUIContent("RETURN", "RETURN"), design.StyleButton(futura_font, 24, Color.white, button_blue, button_blue_hover))){
			mission.STATEPIVOT = 2;
		}
		
	}
	
	//************************************************************************
	//SELECT LEVEL
	private void SELECT_LEVEL(){
		GUI.DrawTexture(new Rect(Screen.width-background_selectlevel.width, Screen.height-background_selectlevel.height, background_selectlevel.width, background_selectlevel.height), background_selectlevel);
		design.DrawRectangle(new Rect (Screen.width-background_selectlevel.width, Screen.height-background_selectlevel.height, background_selectlevel.width, background_selectlevel.height), Color.black, 0.15f);
		
		GUI.Label (new Rect (10, 10, 400, 20), "P R O J E C T : S P A C E W A L K", design.StyleText(futura_font, 14, TextAnchor.MiddleLeft, Color.white));
		GUI.Label (new Rect (10, 25, 400, 50), "S E L E C T  L E V E L", design.StyleText(futura_font, 44, TextAnchor.MiddleLeft, Color.white));
		
		
		//LEVEL 01 BUTTON
		if(GUI.Button (new Rect (10, 200, button_level01.width, button_level01.height), new GUIContent("", "L01"), design.DrawLevelButton(button_level01, button_level01_hover, 1f))){
			Application.LoadLevel("level_01");
		}
		
		//LEVEL 02 BUTTON
		if(GUI.Button (new Rect (10, 350, button_level01.width, button_level01.height), new GUIContent("", "L01"), design.DrawLevelButton(button_level02, button_level02_hover, 1f))){
            Application.LoadLevel("level_02");
        }
		
		//Return Main menu
		if(GUI.Button (new Rect (40, (Screen.height - (button_mainmenu.height + 50)), button_mainmenu.width, button_mainmenu.height), new GUIContent("", "L01"), design.DrawLevelButton(button_mainmenu, button_mainmenu_hover, 1f))){
			mission.STATEPIVOT = 2;
		}
	}
	
	//************************************************************************
	//REGISTER NEW USERS 
	private void REGISTER(){
		//THIS FUNCTION IS WHERE THE USER REGISTERS
	}
	
	//************************************************************************
	//NETWORK ERROR MENU
	private void ERROR_NETWORK(){
		//THIS FUNCTION DISPLAYS THE NETWORK ERROR
	}
	
	//************************************************************************
	//DETECTS NETWORK CONNECTION
	private void IS_NETWORK(){
		//THIS FUNCTION DETECTS IF THIS APP CAN CONNECT TO THE DATABASE
	}


}
