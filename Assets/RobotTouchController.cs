using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotTouchController : MonoBehaviour
{
    public float duration = 3f;
    private float currentTime = 0f;
    public RobotController robotController;
    public GameObject cube;

    private RobotPath currentPath;

    bool moving = false;


    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                float progress = currentTime / duration;
                robotController.jointAngles = currentPath.GetConfiguration(progress);
            }
            else
            {
                moving = false;
                currentTime = 0f;
            }
            return;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Construct a ray from the current touch coordinates
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            // Create a particle if hit
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //TextController.Instance.setText(hit.point.ToString());
                cube.transform.position = hit.point + new Vector3(0f, 0.05f, 0f);

                Vector3[] pointsOnPath = new Vector3[4];
                pointsOnPath[0] = pointsOnPath[1] = robotController.currentFlunchPosition;
                pointsOnPath[1].y = 0.4f;
                pointsOnPath[2] = pointsOnPath[3] = hit.point + new Vector3(0f, 0.1f, 0f);
                pointsOnPath[2].y = 0.4f;


                currentPath = new RobotPath(robotController.gameObject.transform.position, pointsOnPath);

                moving = true;
            }
        }
    }
}
