using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraControl : MonoBehaviour {

	public GameObject player;

	private Vector3 offsetPosition;
    private Vector3 ballPosition;

    private float initialRotationX;

	// Use this for initialization
	void Start () {
		offsetPosition = transform.position - player.transform.position;
        initialRotationX = transform.rotation.eulerAngles.x;
	}
	
	// Update is called once per frame
	void Update () {
        float rotationY = PlayerController.rotationVector.y;

        float sinValue = (float)Math.Sin((double)rotationY / 180 * Math.PI);
        float cosValue = (float)Math.Cos((double)rotationY / 180 * Math.PI);

        float positionX = offsetPosition.x * cosValue + offsetPosition.z * sinValue;
        float positionZ = offsetPosition.x * -sinValue + offsetPosition.z * cosValue;
        float positionY = offsetPosition.y;
        Debug.Log(positionX + " "+ positionY + " " + positionZ);

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
    float LimitRotation(float rotation, float original)
    {
        float tempY = rotation + original;
        while (tempY > 180)
        {
            tempY -= 360;
        }
        if (tempY > 90) {
            tempY = 90;
        }
        while (tempY < -180)
        {
            tempY += 360;
        }
        if (tempY < initialRotationX - 90)
        {
            tempY = initialRotationX - 90;
        }
        return tempY;
    }
}
