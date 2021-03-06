﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedIKTest : MonoBehaviour
{

    public bool idleAnimation = false;

    [Range(-1f, 1f)]
    public float xValue = 0f;

    [Range(-1f, 1f)]
    public float yValue = -1f;

    [Range(-1f, 1f)]
    public float zValue = 0f;

    // rotation angles
    [Range(-180f, 180f)]
    public float yawAngle = 0f;

    [Range(-180f, 180f)]
    public float pitchAngle = 0f;

    [Range(-180f, 180f)]
    public float rollAngle = 0f;

    Vector3 targetPosition;
    Vector3 approachVector = new Vector3(0, 1, 0);
    Vector3 slideVector    = new Vector3(0, 0, 1);
    Vector3 normalVector   = new Vector3(1, 0, 0);

    //[Range(-170f, 170f)]
    //public float facingAxisAngle = 0f;

    [Header("Theta 2")]
    [Range(-180f, 180f)]
    public float theta2Input = 0f;

    public bool cycleTheta2 = false;
    public bool cycleOrientationAngles = false;
    public float degreesPerSecond = 90.0f;

    public GameObject targetObject;
    public GameObject targetObject2;

    public RobotController robotController;


    bool tested = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cycleTheta2)
        {
            if (theta2Input < 360.0f)
            {
                theta2Input += Time.deltaTime * degreesPerSecond;
            }
            else
            {
                theta2Input = 0.0f;
            }
        }

        if (idleAnimation)
        {
            xValue = Mathf.Cos(Time.time);
            yValue = Mathf.Sin(Time.time);
            zValue = Mathf.Sin(Time.time);
        }

        if (cycleOrientationAngles)
        {
            yawAngle = Mathf.Cos(Time.time) * 180f;
            pitchAngle = 0.5f * Mathf.Sin(Time.time) * 180f;
            rollAngle =  2f * Mathf.Cos(Time.time) * 180f;
        }

        targetPosition = targetObject.transform.position;

        approachVector = new Vector3(xValue, yValue, zValue);

        Quaternion newRotation =
            Quaternion.Euler(0, yawAngle, 0) *
            Quaternion.Euler(0, 0, pitchAngle) * 
            Quaternion.Euler(0, rollAngle, 0);
        RotationTest.Instance.SetPose(targetPosition, newRotation);

        approachVector = newRotation * new Vector3(0, 1, 0);
        slideVector    = newRotation * new Vector3(0, 0, 1);
        normalVector   = newRotation * new Vector3(1, 0, 0);
        //targetObject2.transform.position = targetObject.transform.position;
        //targetObject2.transform.rotation = Quaternion.AngleAxis(facingAxisAngle, facingVector);

        //if (Input.GetKeyDown(KeyCode.T))
        {
            if (!tested)
            {
                //float[] ikConfig = robotController.CalculateAdvancedIK(
                //    robotController.gameObject.transform.position,
                //    targetPosition,
                //    approachVector,
                //    slideVector,
                //    normalVector);
                float[] ikConfig = robotController.Calculate7AxisIK(
                    robotController.gameObject.transform.position,
                    targetPosition,
                    approachVector,
                    slideVector,
                    normalVector,
                    theta2Input);



                robotController.jointAngles = ikConfig;
            }
            //tested = true;
        }
    }
}
