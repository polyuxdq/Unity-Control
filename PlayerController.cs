using System.Collections;
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
        throwSpeed = 100f;
        SetDisplayText();
    }

    //// Update is called once per frame
    //void Update() {
    //    if (Input.GetButtonDown("Fire1")) {
    //        Vector3 position = transform.position;
    //        position.y = position.y + 0.75f;
    //        GameObject throwThis = Instantiate(projectile, position, new Quaternion(x: 0, y: 0, z: 0, w: 0)) as GameObject;
    //        throwThis.GetComponent<Rigidbody>().AddRelativeForce(orientationVector * 2000);
    //    }
    //}

    void FixedUpdate() {
        // wsad control
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        // space jumping and view adjust
        float moveJump = Input.GetAxis("Jump") * 5;
        float moveLeftRightView = Input.GetAxis("LeftRightView") * 10;

        // orientation left right
        float orientationLeftRight = GetLimitedRotationAngle(moveLeftRightView, orientationVector.y);
        orientationVector.y = orientationLeftRight;

        // wsad control from x,z axis to x',z' axis due to orientation
        float sinValue = (float)Math.Sin((double)orientationLeftRight / 180 * Math.PI);
        float cosValue = (float)Math.Cos((double)orientationLeftRight / 180 * Math.PI);
        moveHorizontal = moveHorizontal * cosValue + moveVertical * sinValue;
        moveVertical = moveHorizontal * -sinValue + moveVertical * cosValue;

        ConstrainActionInAir(ref moveHorizontal, ref moveVertical, ref moveJump);
        SpeedSlowDownIfNoForceApplied(ref moveHorizontal, ref moveVertical);

        Vector3 movement = new Vector3(moveHorizontal, moveJump, moveVertical);
        rb.AddForce(movement * moveSpeed);
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
    float GetLimitedRotationAngle(float rotation, float original)  {
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
}
