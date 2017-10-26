using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraControl : MonoBehaviour {

    public static float relatedCameraUpDownAngle;
    public static float cameraDistanceRatio;

    public GameObject player;
    public new Camera camera; // Problem? TODO

    private Vector3 initialOffsetBetweenCameraPlayer;
    private Vector3 offsetBetweenCameraPlayer; // Each time
    private float initialUpDownAngle; // Rotation up and down
    private String cameraMode;

    // Use this for initialization
    void Start() {
        relatedCameraUpDownAngle = 0;
        cameraDistanceRatio = 1.0f;

        initialOffsetBetweenCameraPlayer = camera.transform.position - player.transform.position;
        offsetBetweenCameraPlayer = initialOffsetBetweenCameraPlayer;
        initialUpDownAngle = camera.transform.rotation.eulerAngles.x; // X axis for up & down
        cameraMode = "DistanceAngle";
    }

    // Update is called once per frame
    void Update() {
    }

    void FixedUpdate() {
        if (Input.GetKeyDown("c")) {
            cameraMode = cameraMode == "DistanceAngle" ? "InitialPosition" : "DistanceAngle";
        }
        float cameraUpDownInput;
        float cameraDistanceInput;
        if (cameraMode == "DistanceAngle") {
            cameraUpDownInput = Input.GetAxis("CameraUpDown") * 5;
            cameraDistanceInput = Input.GetAxis("CameraDistance") * 0.2f;
        } else {
            cameraUpDownInput = 0;
            cameraDistanceInput = 0;
            float positionUpDownInput = Input.GetAxis("CameraUpDown") * 0.2f;
            float positionFrontBackInput = Input.GetAxis("CameraDistance") * 0.2f;
            initialOffsetBetweenCameraPlayer.y += positionUpDownInput;
            initialOffsetBetweenCameraPlayer.z -= positionFrontBackInput;
        }

        PerformCameraPositionTransformation(ref cameraDistanceInput);
        PerformCameraRotationTransformation(ref cameraUpDownInput);

        // Rigid body for further imporvement
        Rigidbody rb = camera.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = new Vector3(0, 0, 0);
    }

    void PerformCameraPositionTransformation(ref float cameraDistance) {
        // Control Camera Distance by coefficient
        CameraControl.cameraDistanceRatio = GetUpdateDistanceRatio(cameraDistance);
        offsetBetweenCameraPlayer = initialOffsetBetweenCameraPlayer * cameraDistanceRatio;

        // Position transform due to oritentation of player: Left & Right Angle
        float orientationLeftRightAngle = PlayerControl.orientationVectorForLeftRight.y; // A Static to indicating the Left Right
        // Typical Transformation from x,z axis to x',z' axis
        float sinValue = (float)Math.Sin((double)orientationLeftRightAngle / 180 * Math.PI);
        float cosValue = (float)Math.Cos((double)orientationLeftRightAngle / 180 * Math.PI);
        float offsetX = offsetBetweenCameraPlayer.x * cosValue + offsetBetweenCameraPlayer.z * sinValue;
        float offsetZ = offsetBetweenCameraPlayer.x * -sinValue + offsetBetweenCameraPlayer.z * cosValue;
        float offsetY = offsetBetweenCameraPlayer.y; // Height unchanged
        offsetBetweenCameraPlayer = new Vector3(offsetX, offsetY, offsetZ);

        // For better feeling of jumping: only consider x, z asix
        Vector3 playerPosition = player.transform.position;
        // playerPosition.y = 0.125f; // This is the initilial position of the player
        // TODO: Further improvement with desk

        // Assign camera position
        camera.transform.position = playerPosition + offsetBetweenCameraPlayer;
    }

    float GetUpdateDistanceRatio(float distance) {
        float tempRatio = CameraControl.cameraDistanceRatio * (distance + 1);
        if (tempRatio > 3) {
            tempRatio = 3f;
        } else if (tempRatio < 0.5) {
            tempRatio = 0.5f;
        }
        return tempRatio;
    }

    void PerformCameraRotationTransformation(ref float cameraUpDown) {
        // Camera Rotation: orientation of the camera
        Vector3 eulerAngles = camera.transform.rotation.eulerAngles; // Get the current rotation or orientation

        // X axis: camera up and down
        eulerAngles.x = GetLimitedRotationAngle(cameraUpDown, eulerAngles.x);
        CameraControl.relatedCameraUpDownAngle = initialUpDownAngle - eulerAngles.x; // Positive direction goes down

        // Y axis: camera left and right
        eulerAngles.y = PlayerControl.orientationVectorForLeftRight.y;
        camera.transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
    }

    float GetLimitedRotationAngle(float rotation, float original) {
        float tempRotation = rotation + original;
        while (tempRotation > 180) {
            tempRotation -= 360;
        }
        if (tempRotation > 90) {
            tempRotation = 90;
        }
        while (tempRotation < -180) {
            tempRotation += 360;
        }
        if (tempRotation < initialUpDownAngle - 90) {
            tempRotation = initialUpDownAngle - 90; // Negative to go up
        }
        return tempRotation;
    }
}
