using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Throw Collision to Collect Key
 * Throw to Open Doors
 * 
 */
public class ThrowControl : MonoBehaviour {

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("front") && !DoorControl.frontAllow) {
            // Perform nothing for front door collision with disable
        } else {
            if (other.gameObject.CompareTag("front") && DoorControl.frontAllow) {
                DoorControl.frontOn = true;
            } else if (other.gameObject.CompareTag("left")) {
                DoorControl.leftOn = true;
            } else if (other.gameObject.CompareTag("right")) {
                DoorControl.rightOn = true;
            } else if (other.gameObject.CompareTag("back")) {
                DoorControl.backOn = true;
            } else if (other.gameObject.CompareTag("Pick")) {
                PlayerControl.count++;
            }
            // Duplicate Door Objects are buit in the model
            // One for detection, one for physical constrain
            other.gameObject.SetActive(false);
            gameObject.SetActive(false); // Throw object
        }
    }
}
