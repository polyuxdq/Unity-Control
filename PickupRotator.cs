using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The object keeps self-spinning for display effect.
 * 
 */
public class PickupRotator : MonoBehaviour {
    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    void FixedUpdate() {
        // Rotation of cube
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime * 2);
    }
}
