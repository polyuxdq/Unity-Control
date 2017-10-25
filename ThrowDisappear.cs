using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowDisappear : MonoBehaviour {
    public static int bang = 0;
    // Use this for initialization
    void Start() {
    }

    private void Awake() {
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Pick")) {
            // other.gameObject.SetActive(false);
            bang++;
            Debug.Log("Bang" + bang);
        }
    }
}
