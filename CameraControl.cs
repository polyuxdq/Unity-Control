using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraControl : MonoBehaviour {

	public GameObject player;

    private Vector3 initialOffsetPosition;
	private Vector3 offsetPosition;
    private Vector3 ballPosition;

    private float initialRotationX;
    private float cameraDistanceRatio;

	// Use this for initialization
	void Start () {
        initialOffsetPosition = transform.position - player.transform.position;
        offsetPosition = initialOffsetPosition;
        initialRotationX = transform.rotation.eulerAngles.x;
        cameraDistanceRatio = 1;
	}
	
	// Update is called once per frame
	void Update () {
        // Distance Ratio
        UpdateDistance(Input.GetAxis("CameraDistance") * 0.2f);
        offsetPosition = initialOffsetPosition * cameraDistanceRatio;
        Debug.Log(cameraDistanceRatio);

        // Rotation
        float rotationY = PlayerController.rotationVector.y;

        float sinValue = (float)Math.Sin((double)rotationY / 180 * Math.PI);
        float cosValue = (float)Math.Cos((double)rotationY / 180 * Math.PI);

        float positionX = offsetPosition.x * cosValue + offsetPosition.z * sinValue;
        float positionZ = offsetPosition.x * -sinValue + offsetPosition.z * cosValue;
        float positionY = offsetPosition.y;
        //Debug.Log(positionX + " "+ positionY + " " + positionZ);

        // Only consider x, z asix
		ballPosition = player.transform.position;
		ballPosition.y = 0;

        transform.position = ballPosition + new Vector3(positionX, positionY, positionZ);

        Vector3 eulerAngles = transform.rotation.eulerAngles;

        float cameraRotation = Input.GetAxis("CameraRotation") * 5;
        eulerAngles.x = LimitRotation(cameraRotation, eulerAngles.x);
        transform.rotation = Quaternion.Euler(eulerAngles.x, rotationY, eulerAngles.z);

	}

    // limit the rotation value
    float LimitRotation(float rotation, float original) {
        float tempY = rotation + original;
        while (tempY > 180) {
            tempY -= 360;
        }
        if (tempY > 90) {
            tempY = 90;
        }
        while (tempY < -180) {
            tempY += 360;
        }
        if (tempY < initialRotationX - 90) {
            tempY = initialRotationX - 90;
        }
        return tempY;
    }

    int UpdateDistance(float distance) {
        cameraDistanceRatio = cameraDistanceRatio * distance + cameraDistanceRatio;
        if (cameraDistanceRatio > 3) {
            cameraDistanceRatio = 3;
        }
        if (cameraDistanceRatio < 0.5) {
            cameraDistanceRatio = 0.5f;
        }
        return 0;
    }
}
