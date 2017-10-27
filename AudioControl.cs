using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Audio Control
 * 
 * Play Music For Event
 * 
 */
public class AudioControl : MonoBehaviour {
    public AudioClip sound;
    private AudioSource source;

    // Use this for initialization
    void Start() {
    }

    private void Awake() {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
    }

    private void FixedUpdate() {
        if (Input.GetKeyDown("3")) {
            PlayAudioOnce();
        }
    }

    public void PlayAudioOnce() {
        float vol = Random.Range(min: 0.5f, max: 1.0f);
        source.PlayOneShot(sound, vol);
    }
}
