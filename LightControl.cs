using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Light Auto On Off with distance measure
 */
public class LightControl : MonoBehaviour {
    public Light light1;
    public Light light2;
    public Light light3;
    public Light light4;
    public Light light5;
    public Light light6;
    public Light light7;
    public Light light8;

    public Light directionalLight;

    public static bool[] lightEnable;
    public static new Light[] light;

    public GameObject player;
    private float distanceThreshold = 7.5f;
    private bool hiddenTrigger;

    // Use this for initialization
    void Start() {
        light = new Light[] {
            null, light1, light2, light3, light4, light5, light6, light7, light8
        };
        lightEnable = new bool[] {
            false, false, false, false, false, false, false, false, false
        };
    }

    // Update is called once per frame
    void Update() {
    }

    private void FixedUpdate() {
        LightLogicControl();

        if (Input.GetKeyDown("z")) {
            directionalLight.enabled = !directionalLight.enabled;
        }

        // hidden test module
        if (Input.GetKeyDown("2")) {
            hiddenTrigger = true;
            for (int i = 1; i <= 8; i++) {
                lightEnable[i] = true;
            } // Some problem here?
        }
    }

    void LightLogicControl() {
        Vector3 playerPosion = player.transform.position;
        for (int i = 1; i <= 8; i++) {
            if (!hiddenTrigger) {
                float distance = Vector3.Distance(playerPosion, light[i].transform.position);
                if (distance < distanceThreshold) {
                    lightEnable[i] = true;
                } else {
                    lightEnable[i] = false;
                }
            }
            light[i].enabled = lightEnable[i];
        }
    }
}
