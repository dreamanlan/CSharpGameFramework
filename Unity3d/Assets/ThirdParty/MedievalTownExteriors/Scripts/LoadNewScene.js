#pragma strict
//This script is used to display the name of the shop, or scene to load.
//The Character is not set up to actually load the scene; but if you wish to use this script, simply have your character call the LoadScene() function, bellow.

var newScene : String; //name of scene to load | Feel free to add spaces. Spaces will be removed when loading scene.
static var title_Background : GameObject;
static var title : GameObject;

function Start() {
	title_Background = GameObject.Find("Title_Background");
	title = GameObject.Find("Title");
}
function DisplayScene() {
	//Set title of new scene
	title.GetComponent.<UI.Text>().text = newScene;
	
	//Display NewScene
	while(title_Background.GetComponent.<UI.Image>().color.a < 0.9f) {
		title_Background.GetComponent.<UI.Image>().color.a += 2.0f * Time.deltaTime;
		title.GetComponent.<UI.Text>().color.a += 2.0f * Time.deltaTime;
		yield;
	}
	title_Background.GetComponent.<UI.Image>().color.a = 1.0f;
	title.GetComponent.<UI.Text>().color.a = 1.0f;
}
function HideScene() {
	//Hide NewScene
	while(title_Background.GetComponent.<UI.Image>().color.a > 0.01f) {
		title_Background.GetComponent.<UI.Image>().color.a -= 2.0f * Time.deltaTime;
		title.GetComponent.<UI.Text>().color.a -= 2.0f * Time.deltaTime;
		yield;
	}
	title_Background.GetComponent.<UI.Image>().color.a = 0.0f;
	title.GetComponent.<UI.Text>().color.a = 0.0f;
}
function LoadScene() {
	//Removes spaces from newScene text!
	var newString = newScene.Replace(" ", "");
	//Load the name of our new string
	Application.LoadLevel(newString);
}