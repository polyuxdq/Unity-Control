﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Player Collision For Collection
 * 
 */
public class Player : MonoBehaviour {

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Pick")) {
            other.gameObject.SetActive(false);
            PlayerControl.count++;
        }
    }
}
