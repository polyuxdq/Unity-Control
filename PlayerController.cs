using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class PlayerController : MonoBehaviour {

	public float speed;
	public Text countText;
	public Text winText;

    public static Vector3 rotationVector;

	private Rigidbody rb;
	private int count;


	// Use this for initialization
	void Start () {
        rotationVector = new Vector3(0, 0, 0);
		rb = GetComponent<Rigidbody> ();
		count = 0;
		SetCountText ();
		winText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		// Debug.Log ("Update time" + Time.deltaTime);
	}

	void FixedUpdate() {
		// Get form keyboard
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		float moveJump = Input.GetAxis ("Jump") * 5;
        float moveRotation = Input.GetAxis("Rotation") * 10;

        // Rotation Effect
        float rotationY = UpdateRotation(moveRotation);
        // Debug.Log("rotationY " + rotationY);
        float sinValue = (float)Math.Sin((double)rotationY/180*Math.PI);
        float cosValue = (float)Math.Cos((double)rotationY/180*Math.PI);
        // Transform
        float tempX = moveHorizontal * cosValue + moveVertical * sinValue;
        float tempZ = moveHorizontal * -sinValue + moveVertical * cosValue;
        // Update
        moveHorizontal = tempX;
        moveVertical = tempZ;

        // constrain of height(y axis), no force in the air
        float yPosition = transform.position.y;
		if (yPosition > 0.75) {
			moveJump = 0;
			moveHorizontal = 0;
            moveVertical = 0;
		}

		// constrain of horizontal(x axis), vertical(z asix)
        // Slow down the speed
		float coefficient = 0.2f;
		float xForce = - rb.velocity.x * coefficient;
		float zForce = - rb.velocity.z * coefficient;
        // if no force applied, at the ground
        // What if some surface at the desk
		if (Math.Abs(moveHorizontal) < 0.05 && yPosition < 0.75)
			moveHorizontal = xForce;
		if (Math.Abs(moveVertical) < 0.05 && yPosition < 0.75)
			moveVertical = zForce;

		Vector3 movement = new Vector3 (moveHorizontal, moveJump, moveVertical);
		rb.AddForce (movement * speed);
		// Debug.Log ("FixedUpdate time:" + Time.deltaTime);
	}

    // Pick up element
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Pick")) {
			other.gameObject.SetActive(false);
			count++;
			SetCountText ();
		}
	}

    // Format the Count Text All the time
	void SetCountText () {
		countText.text = "Count: " + count.ToString ();
		if (count >= 5)
			winText.text = "You Win!";			
	}

    // limit the rotation value
    float UpdateRotation(float rotation) {
        float tempY = rotationVector.y + rotation;
        while (tempY > 180) {
            tempY -= 360;
        }
        while (tempY < - 180) {
            tempY += 360;
        }
        rotationVector.y = tempY;
        return rotationVector.y;
    }
}
