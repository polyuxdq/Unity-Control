﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* The object is keep rotating for display effect */
public class PickupRotator : MonoBehaviour {
    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        // Rotation of cube each frame
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }
}
