﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour {
    public static Vector3 orientationVector;

    public GameObject projectile;
    public float moveSpeed;
    public Text countText;
    public Text winText;

    private Rigidbody rb;
    private int count;
    private float throwSpeed;

    // Use this for initialization
    void Start() {
        orientationVector = new Vector3(0, 0, 0);
        rb = GetComponent<Rigidbody>();
        count = 0;
        throwSpeed = 1000f;
        SetDisplayText();
    }

    // Update is called once per frame
    void Update() {
    }

    void FixedUpdate() {
        // wsad control
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        // space jumping and view adjust
        float moveJump = Input.GetAxis("Jump") * 5;
        float moveLeftRightView = Input.GetAxis("LeftRightView") * 10;

        // orientation left right
        orientationVector.y = GetLimitedRotationAngle(moveLeftRightView, orientationVector.y);

        AxisTransformationDueToOrientationChange(ref moveHorizontal, ref moveVertical);
        ConstrainActionInAir(ref moveHorizontal, ref moveVertical, ref moveJump);
        SpeedSlowDownIfNoForceApplied(ref moveHorizontal, ref moveVertical);

        Vector3 movement = new Vector3(moveHorizontal, moveJump, moveVertical);
        rb.AddForce(movement * moveSpeed);

        ThrowProjectileLogicAction();

        if (transform.position.y < -10) {
            transform.position = new Vector3(0, 0.75f, -4.25f);
            rb.velocity = new Vector3(0, 0, 0);
            rb.angularVelocity = new Vector3(0, 0, 0);
            rb.rotation = Quaternion.Euler(0, 0, 0);
            //rb.AddForce(new Vector3(0, 0, 0));
            //rb.AddTorque(new Vector3(0, 0, 0));
            Debug.Log("Don't do silly things.");
        }
    }

    // Pick up element
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Pick")) {
            other.gameObject.SetActive(false);
            count++;
            SetDisplayText();
        }
    }

    // Format the Count Text All the time
    void SetDisplayText() {
        countText.text = "Count: " + count.ToString();
        if (count >= 5) {
            winText.text = "You Win!";
        } else {
            winText.text = "";
        }
    }

    // limit the rotation value
    float GetLimitedRotationAngle(float rotation, float original) {
        float tempRotation = rotation + original;
        while (tempRotation > 180) {
            tempRotation -= 360;
        }
        while (tempRotation < -180) {
            tempRotation += 360;
        }
        return tempRotation;
    }

    void ConstrainActionInAir(ref float moveHorizontal, ref float moveVertical, ref float moveJump) {
        // constrain of height(y axis), no force in the air
        float height = transform.position.y; // init = 0.5
        if (height > 0.75) {
            moveJump = 0;
            moveHorizontal = 0;
            moveVertical = 0;
        }
    }

    void SpeedSlowDownIfNoForceApplied(ref float moveHorizontal, ref float moveVertical) {
        // constrain of horizontal(x axis), vertical(z asix)
        // Slow down the speed
        float coefficient = 0.2f;
        float height = transform.position.y;
        float xForce = -rb.velocity.x * coefficient;
        float zForce = -rb.velocity.z * coefficient;
        // if no force applied, at the ground
        // TODO: what if some surface at the desk
        if (Math.Abs(moveHorizontal) < 0.05 && height < 0.75)
            moveHorizontal = xForce;
        if (Math.Abs(moveVertical) < 0.05 && height < 0.75)
            moveVertical = zForce;
    }

    void AxisTransformationDueToOrientationChange(ref float moveHorizontal, ref float moveVertical) {
        float orientationLeftRight = orientationVector.y;
        // wsad control from x,z axis to x',z' axis due to orientation
        float sinValue = (float)Math.Sin((double)orientationLeftRight / 180 * Math.PI);
        float cosValue = (float)Math.Cos((double)orientationLeftRight / 180 * Math.PI);
        // Transform, must use temp variable
        float newHorizontal = moveHorizontal * cosValue + moveVertical * sinValue;
        float newVertical = moveHorizontal * -sinValue + moveVertical * cosValue;
        moveHorizontal = newHorizontal;
        moveVertical = newVertical;
    }

    void ThrowProjectileLogicAction() {
        if (Input.GetButtonDown("Fire1")) {
            Vector3 position = transform.position;
            position.y = position.y + 1f;
            GameObject throwThis = Instantiate(projectile, position, new Quaternion(x: 0, y: 0, z: 0, w: 0)) as GameObject;

            float speedHorizontal = 0;
            float speedVertical = throwSpeed;
            float speedUpward = (float)Math.Tan((double)CameraControl.relatedCameraUpDown / 180 * Math.PI) * throwSpeed;
            AxisTransformationDueToOrientationChange(ref speedHorizontal, ref speedVertical);
            throwThis.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(speedHorizontal, speedUpward, speedVertical));
        }
    }
}
