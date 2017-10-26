using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Door Animation and Control
 * 
 * Open animation and immediate close
 * Control by the static variable
 * 
 */
public class DoorControl : MonoBehaviour {

    public GameObject frontDoor;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public GameObject backDoor;

    public static bool frontOn = false;
    public static bool leftOn = false;
    public static bool rightOn = false;
    public static bool backOn = false;
    public static bool frontAllow = false;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    private void FixedUpdate() {
        DoorLogicControl();
    }

    void DoorLogicControl() {
        if (frontOn) {
            DoorOpenAction("frontDoor");
        } else {
            frontDoor.transform.rotation = Quaternion.Euler(270, 270, 0);
        }

        if (leftOn) {
            DoorOpenAction("leftDoor");
        } else {
            leftDoor.transform.rotation = Quaternion.Euler(270, 0, 0);
        }
        if (rightOn) {
            DoorOpenAction("rightDoor");
        } else {
            rightDoor.transform.rotation = Quaternion.Euler(270, 0, 0);
        }
        if (backOn) {
            DoorOpenAction("backDoor");
        } else {
            backDoor.transform.rotation = Quaternion.Euler(270, 180, 0);
        }

        // hidden test module
        if (Input.GetKeyDown("1")) {
            frontOn = leftOn = rightOn = backOn = true;
        }
    }

    void DoorOpenAction(string whichDoor) {
        Vector3 eulerAngles;
        switch (whichDoor) {
            case "frontDoor":
                // door: (270, 270, 0) -> (270, 360, 0)
                eulerAngles = frontDoor.transform.eulerAngles;
                if (eulerAngles.y < 360 && eulerAngles.y > 10) {
                    eulerAngles.y += Time.deltaTime * 45; // open the door in 2s
                }
                frontDoor.transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
                break;
            case "leftDoor":
                // door: (270, 0, 0) -> (270, 90, 0) -> (270, 45, 0)
                eulerAngles = leftDoor.transform.eulerAngles;
                if (eulerAngles.y < 45) {
                    eulerAngles.y += Time.deltaTime * 45; // open the door in 2s
                }
                leftDoor.transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
                break;
            case "rightDoor":
                // door: (270, 0, 0) -> (270, 90, 0) -> (270, 60, 0)
                eulerAngles = rightDoor.transform.eulerAngles;
                if (eulerAngles.y < 60) {
                    eulerAngles.y += Time.deltaTime * 45; // open the door in 2s
                }
                rightDoor.transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
                break;
            case "backDoor":
                // door: (270, 180, 0) -> (270, 270, 0)
                eulerAngles = backDoor.transform.eulerAngles;
                if (eulerAngles.y < 270) {
                    eulerAngles.y += Time.deltaTime * 45; // open the door in 2s
                }
                backDoor.transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
                break;
            default:
                break;
        }
    }
}
