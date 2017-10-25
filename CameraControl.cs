using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* Camera keeps track of the object and changes angle */
public class CameraControl : MonoBehaviour {
    public static float relatedCameraUpDown;
    public GameObject player; // The player object

    private Vector3 initialOffsetPosition; // The start of the game
    private Vector3 offsetPosition; // Each time
    private Vector3 playerPosition;
    private float initialUpDown; // Rotation up and down
    private float cameraDistanceRatio;

    // Use this for initialization
    void Start() {
        relatedCameraUpDown = 0;
        initialOffsetPosition = transform.position - player.transform.position;
        offsetPosition = initialOffsetPosition;
        initialUpDown = transform.rotation.eulerAngles.x; // X axis for up & down
        cameraDistanceRatio = 1.0f;
    }

    // Update is called once per frame
    void Update() {
        PerformCameraPositionTransformation();
        PerformCameraRotationTransformation();
    }

    void PerformCameraPositionTransformation() {
        // Control Camera Distance by coefficient
        cameraDistanceRatio = GetUpdateDistanceRatio(Input.GetAxis("CameraDistance") * 0.2f);
        offsetPosition = initialOffsetPosition * cameraDistanceRatio;

        // Position transform due to oritentation of player: Left & Right Angle
        float orientationLeftRight = PlayerController.orientationVector.y; // A Static to indicating the Left Right
        // Typical Transformation from x,z axis to x',z' axis
        float sinValue = (float)Math.Sin((double)orientationLeftRight / 180 * Math.PI);
        float cosValue = (float)Math.Cos((double)orientationLeftRight / 180 * Math.PI);
        float offsetX = offsetPosition.x * cosValue + offsetPosition.z * sinValue;
        float offsetZ = offsetPosition.x * -sinValue + offsetPosition.z * cosValue;
        float offsetY = offsetPosition.y; // Height unchanged
        offsetPosition = new Vector3(offsetX, offsetY, offsetZ);

        // For better feeling of jumping: only consider x, z asix
        playerPosition = player.transform.position;
        playerPosition.y = 0.5f; // This is the initilial position of the player
        // TODO: Further improvement with desk

        // Assign camera position
        transform.position = playerPosition + offsetPosition;
    }

    float GetUpdateDistanceRatio(float distance) {
        float tempRatio = cameraDistanceRatio * (distance + 1);
        if (tempRatio > 3) {
            tempRatio = 3f;
        } else if (tempRatio < 0.5) {
            tempRatio = 0.5f;
        }
        return tempRatio;
    }

    void PerformCameraRotationTransformation() {
        // Camera Rotation: orientation of the camera
        Vector3 eulerAngles = transform.rotation.eulerAngles; // Get the current rotation or orientation

        // X axis: camera up and down
        float cameraUpDown = Input.GetAxis("CameraUpDown") * 5;
        eulerAngles.x = GetLimitedRotationAngle(cameraUpDown, eulerAngles.x);
        relatedCameraUpDown = initialUpDown - eulerAngles.x; // Positive direction goes down

        // Y axis: camera left and right
        eulerAngles.y = PlayerController.orientationVector.y;
        transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
    }

    // limit the rotation value
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
        if (tempRotation < initialUpDown - 90) {
            tempRotation = initialUpDown - 90; // Negative to go up
        }
        return tempRotation;
    }
}