using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowControl : MonoBehaviour {

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("front") && !DoorControl.frontAllow) {
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
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
