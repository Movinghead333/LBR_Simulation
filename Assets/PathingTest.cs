using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingTest : MonoBehaviour
{
    public RobotController robotController;
    RobotPath path;
    bool running = false;
    float duration = 5f;

    float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3[] pathPoints = new Vector3[4];

        pathPoints[0] = new Vector3(-0.2f, 0.1f, -0.2f);
        pathPoints[1] = new Vector3(-0.2f, 0.4f, -0.2f);
        pathPoints[2] = new Vector3(0.2f, 0.4f, -0.2f);
        pathPoints[3] = new Vector3(0.2f, 0.1f, -0.2f);

        path = new RobotPath(robotController.gameObject.transform.position, pathPoints);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            running = true;
        }

        if (running && currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float progress = currentTime / duration;
            float[] config = new float[7];

            config = path.GetConfiguration(progress);
            for (int i = 0; i < config.Length; i++)
            {
                robotController.jointAngles[i] = config[i];
            }
        }
    }
}
