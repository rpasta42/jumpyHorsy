using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/*
 * TODO:
 * 	- [ ] UI
 * 	- [ ] check if player lost
 * 	- [ ] animate jumps
 * 	- [ ] jumps, bend down to get lower
 * 
 * Features:
 * 	- [ ] double jumps for high obstacles
 * 	- [ ] sometimes ground dissapears, and player jumps from 1 square to another one
 * 	- [ ] collecting gems for reward
 * 	- [ ] highscore local and global
 * 	- [ ] PvP mode automatic pairing
 * 	- [ ] achievements, highscore, store, no ads, volume
 */

/*public class MiniGestureRecognizer : MonoBehaviour {
	public enum SwipeDirection{
		Up,
		Down,
		Right,
		Left
	}

	public static event Action<SwipeDirection> Swipe;
	private bool swiping = false;
	private bool eventSent = false;
	private Vector2 lastPosition;

	void Update () {
		if (Input.touchCount == 0) 
			return;

		if (Input.GetTouch(0).deltaPosition.sqrMagnitude != 0) {
			if (swiping == false) {
				swiping = true;
				lastPosition = Input.GetTouch(0).position;
				return;
			}
			else {
				if (!eventSent) {
					if (Swipe != null) {
						Vector2 direction = Input.GetTouch(0).position - lastPosition;

						if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
							if (direction.x > 0) 
								Swipe(SwipeDirection.Right);
							else
								Swipe(SwipeDirection.Left);
						}
						else{
							if (direction.y > 0)
								Swipe(SwipeDirection.Up);
							else
								Swipe(SwipeDirection.Down);
						}

						eventSent = true;
					}
				}
			}
		}
		else {
			swiping = false;
			eventSent = false;
		}
	}
}*/

public class Game : MonoBehaviour {
	private int frame_counter = 0;
	private GameObject player;
	private List<GameObject> obstacles;
	private bool done;
	private int score;

	//public float jumpHeight;
	public float jumpSpeed;
	public float speed = 0.1f;

	// Use this for initialization
	void Start () {
		done = false;
		obstacles = new List<GameObject>();
		score = 0;

		player = GameObject.Find("Player");
		player.AddComponent<PlayerCollider>();
		PlayerCollider pc = player.GetComponent<PlayerCollider>();

		obstacles.Add(mkObstacle());

		pc.callback = col => {
			if (col.gameObject.name != "Ground") {
				Debug.Log("Touching");
				GameObject.Find("ScoreText").GetComponent<Text>().text = "You lost! You score is: " + score;

				done = true;
			}

		};

		/*GameObject.CreatePrimitive(PrimitiveType.Cube);
		player.transform.position = new Vector3(1.0f, 0.11f, 0.0f);
		player.transform.SetParent(gameObject.transform);
		player.AddComponent<Rigidbody>();*/
	}

	bool IsClick() {
		return (frame_counter++ > 20) && (Input.GetMouseButtonDown(1) || (Input.touchCount >= 1)) && (player.transform.position.y < 0.1); //also check phone touch
	}

	void Jump() {
		var rb = player.GetComponent<Rigidbody>();
		rb.AddForce(new Vector3(1.0f, 400.0f, 1.0f));
	}

	GameObject mkObstacle() {
		var obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
		var pos = transform.position;
		obstacle.transform.position = new Vector3(pos.x + 16, pos.y, pos.z + 5); //pos.x + 10
		return obstacle;
	}

	// Update is called once per frame
	void Update () {
		if (done)
			return;
		var rb = player.GetComponent<Rigidbody>();
		var pos = player.transform.position;

		//if (rb.velocity.z > 0) {
			Vector3 v = rb.velocity;
			v = new Vector3(v.x, v.y, 0);
			rb.velocity = v;
		//}
		var e = player.transform.eulerAngles;
		player.transform.eulerAngles = new Vector3(0, e.y, e.z);

		var last = obstacles[obstacles.Count-1];
		var last_trans = last.transform;
		var last_pos = last_trans.position;

		if (pos.x > last_pos.x && frame_counter++ > 4) {
			score++;
			var t = GameObject.Find("ScoreText").GetComponent<Text>();
			t.text = score.ToString();

			var obstacle = mkObstacle();
			obstacles.Add(obstacle);
			frame_counter = 0;
		}

		pos = transform.position;
		transform.position = new Vector3(pos.x + speed, pos.y, pos.z);

		if (IsClick()) {
			frame_counter = 0;
			Debug.Log("jumped");
			Jump();
		}
	}
}


public class PlayerCollider : MonoBehaviour {
	public delegate void CallBack(Collision col);
	public CallBack callback;

	void OnCollisionEnter(Collision col) {
		callback(col);
	}
}