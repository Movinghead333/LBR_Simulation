using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotIdle : MonoBehaviour
{
    public RobotController robotController;

    public float threshold = 0f;
    public float increment = 0f;
    float currentAngle = 0f;
    bool up = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (up)
        {
            if (currentAngle < threshold)
            {
                currentAngle += increment;
                SetAngles(currentAngle);
            }
            else
            {
                up = false;
            }
        }
        else
        {
            if (currentAngle > -threshold)
            {
                currentAngle -= increment;
                SetAngles(currentAngle);
            }
            else
            {
                up = true;
            }
        }
    }

    private void SetAngles(float angle)
    {
        for (int i = 0; i < 7; i++)
        {
            robotController.jointAngles[i] = angle;
        }
    }
}
