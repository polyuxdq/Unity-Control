using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour {
    // Class that can be displayed in the inspector
    [System.Serializable]
    public class DataClass {
        public int myInt;
        public float myFloat;
    }

    public DataClass myClass;
    public Light myLight;
    public AudioClip lightSound;

    private AudioSource source;

    // Use this for initialization
    void Start() {
        myClass = new DataClass();
    }

    // Init
    void Awake() {
        Debug.Log("Awake!");
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("z")) {
            myLight.enabled = !myLight.enabled;
            float vol = Random.Range(min: 0.5f, max: 1.0f);
            source.PlayOneShot(lightSound, vol);
        }
    }
}
