using System.Collections;
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

    Vector3 targetPosition;
    Vector3 facingVector;

    [Range(-170f, 170f)]
    public float facingAxisAngle = 0f;

    public GameObject targetObject;

    public RobotController robotController;


    bool tested = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (idleAnimation)
        {
            xValue = Mathf.Cos(Time.time);
            yValue = Mathf.Sin(Time.time);
            zValue = Mathf.Sin(Time.time);
        }

        targetPosition = targetObject.transform.position;
        facingVector = new Vector3(xValue, yValue, zValue);

        //if (Input.GetKeyDown(KeyCode.T))
        {
            if (!tested)
            {
                float[] ikConfig = robotController.CalculateAdvancedIK(
                    robotController.gameObject.transform.position,
                    targetPosition,
                    facingVector,
                    facingAxisAngle);

                robotController.jointAngles = ikConfig;
            }
            //tested = true;
        }
    }
}
