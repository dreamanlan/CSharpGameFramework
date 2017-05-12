using UnityEngine;
using System.Collections;

public class Play_animations : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public  void OnGUI()
	{
		if (GUI.Button(new Rect((float)50, (float)50, (float)80, (float)20), "Idle"))
		{
			this.GetComponent<Animation>().CrossFade("idle");
		}
		if (GUI.Button(new Rect((float)50, (float)75, (float)80, (float)20), "Walk"))
		{
			this.GetComponent<Animation>().CrossFade("walk_loop");
		}
		if (GUI.Button(new Rect((float)50, (float)100, (float)80, (float)20), "Run"))
		{
			this.GetComponent<Animation>().CrossFade("Run");
		}
		if (GUI.Button(new Rect((float)50, (float)125, (float)80, (float)20), "Eat"))
		{
			this.GetComponent<Animation>().CrossFade("Eat");
		}
		if (GUI.Button(new Rect((float)50, (float)150, (float)80, (float)20), "Roar"))
		{
			this.GetComponent<Animation>().CrossFade("Roar");
		}
		if (GUI.Button(new Rect((float)50, (float)175, (float)80, (float)20), "Snap"))
		{
			this.GetComponent<Animation>().CrossFade("Snap");
		}
		if (GUI.Button(new Rect((float)50, (float)200, (float)80, (float)20), "Hit"))
		{
			this.GetComponent<Animation>().CrossFade("hit");
		}
		if (GUI.Button(new Rect((float)50, (float)225, (float)80, (float)20), "Dodge"))
		{
			this.GetComponent<Animation>().CrossFade("Dodge");
		}
		if (GUI.Button(new Rect((float)50, (float)250, (float)80, (float)20), "Jump"))
		{
			this.GetComponent<Animation>().CrossFade("Jump");
		}
		if (GUI.Button(new Rect((float)50, (float)275, (float)80, (float)20), "Look Right"))
		{
			this.GetComponent<Animation>().CrossFade("Look_right");
		}
		if (GUI.Button(new Rect((float)50, (float)300, (float)80, (float)20), "Look Left"))
		{
			this.GetComponent<Animation>().CrossFade("Look_left");
		}
		if (GUI.Button(new Rect((float)50, (float)325, (float)80, (float)20), "Death"))
		{
			this.GetComponent<Animation>().CrossFade("Death");
		}
		if (GUI.Button(new Rect((float)50, (float)350, (float)80, (float)20), "Talk"))
		{
			this.GetComponent<Animation>().CrossFade("Talk");
		}
		if (GUI.Button(new Rect((float)50, (float)375, (float)80, (float)20), "Fart"))
		{
			this.GetComponent<Animation>().CrossFade("Fart");
		}
		if (GUI.Button(new Rect((float)50, (float)400, (float)80, (float)20), "Dance"))
		{
			this.GetComponent<Animation>().CrossFade("Dance");
		}
		if (GUI.Button(new Rect((float)50, (float)425, (float)80, (float)20), "Burp"))
		{
			this.GetComponent<Animation>().CrossFade("Burp");
		}
	}
}
