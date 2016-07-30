using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	public float speed = 0.1f;
	private int frame_counter = 0;
	private GameObject player;


	//public float jumpHeight;
	public float jumpSpeed;
	private float leftToJump;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		/*GameObject.CreatePrimitive(PrimitiveType.Cube);
		player.transform.position = new Vector3(1.0f, 0.11f, 0.0f);
		player.transform.SetParent(gameObject.transform);
		player.AddComponent<Rigidbody>();*/
	}


	bool IsClick() {
		return Input.GetMouseButtonDown(1); //also check phone touch
	}

	void Jump() {
		var rb = player.GetComponent<Rigidbody>();
		rb.AddForce(new Vector3(1.0f, 400.0f, 1.0f));
	}

	// Update is called once per frame
	void Update () {
		var trans = transform.position;

		if (frame_counter++ > 100) {
			var obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
			obstacle.transform.position = new Vector3(trans.x + 10, trans.y, trans.z + 5);
			frame_counter = 0;
		}

		transform.position = new Vector3(trans.x + speed, trans.y, trans.z);

		if (IsClick()) {
			Debug.Log("jumped");
			Jump();
		}
	}
}
