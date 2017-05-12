#pragma strict
var speed : float = 7; //player's movement speed
var gravity : float = 10; //amount of gravitational force applied to the player
private var controller : CharacterController; //player's CharacterController component
private var moveDirection : Vector3 = Vector3.zero;

function Start () {
	controller = transform.GetComponent(CharacterController);
}

function Update () {
	//APPLY GRAVITY
	if(moveDirection.y > gravity * -1) {
		moveDirection.y -= gravity * Time.deltaTime;
	}
	controller.Move(moveDirection * Time.deltaTime);
	var left = transform.TransformDirection(Vector3.left);
	
	if(controller.isGrounded) {
		if(Input.GetKeyDown(KeyCode.Space)) {
			moveDirection.y = speed;
		}
		else if(Input.GetKey("w")) {
			controller.SimpleMove(transform.forward * speed);
		}
		else if(Input.GetKey("s")) {
			controller.SimpleMove(transform.forward * -speed);
		}
		else if(Input.GetKey("a")) {
			controller.SimpleMove(left * speed);
		}
		else if(Input.GetKey("d")) {
			controller.SimpleMove(left * -speed);
		}
	}
	else {
		if(Input.GetKey("w")) {
			var relative : Vector3;
			relative = transform.TransformDirection(0,0,1);
			controller.Move(relative * Time.deltaTime * speed / 1.5);
		}
	}
}
function OnTriggerEnter(hit : Collider) {
	if(hit.transform.tag == "LoadNewScene") {
		hit.transform.GetComponent(LoadNewScene).DisplayScene();
	}
}
function OnTriggerExit(hit : Collider) {
	if(hit.transform.tag == "LoadNewScene") {
		hit.transform.GetComponent(LoadNewScene).HideScene();
	}
}
function OnControllerColliderHit (hit : ControllerColliderHit) {
	if(hit.transform.GetComponent.<Rigidbody>()) {
		hit.transform.GetComponent.<Rigidbody>().AddForce(10 * transform.forward);
	}
}