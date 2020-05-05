using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickAndPlaceController : MonoBehaviour
{
    public RobotController robotController;
    public Transform target;

    public float duration;
    [SerializeField]
    private float currentTime = 0f;

    private Plan currentPlan;

    public PickAndPlaceTask currentTask;

    private Vector3 robotPosition;
    private RobotPath currentPath;

    private float[] fromConfig;
    private float[] toConfig;


    private float[] oldConfig;

    private ActionState actionState;
    private bool running = false;

    enum ActionState
    {
        SELECTING_TASK,
        EXECUTING_TASK,
        FINISHED
    }

    // Start is called before the first frame update
    void Start()
    {
        currentPlan = new Plan("Plans/plan");
        if (currentPath == null)
        {
            Debug.Log("plan is null");
        }


        
    }

    

    // Update is called once per frame
    void Update()
    {
        robotPosition = robotController.gameObject.transform.position;

        if (Input.GetKeyDown(KeyCode.E) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            running = !running;
        }

        if (!running)
        {
            return;
        }

        switch (actionState)
        {
            case ActionState.SELECTING_TASK:
                Debug.Log("Selecting Action");
                currentTask = null;
                for (int i = 0; i < currentPlan.tasks.Length; i++)
                {
                    if (!currentPlan.tasks[i].done)
                    {
                        currentTask = currentPlan.tasks[i];
                        break;
                    }
                }

                if (currentTask == null)
                {
                    actionState = ActionState.FINISHED;
                    return;
                }

                if (!currentTask.done)
                {
                    Vector3[] pointsOnPath = new Vector3[7];

                    pointsOnPath[0] = robotController.currentFlunchPosition;

                    pointsOnPath[1] = pointsOnPath[2] = pointsOnPath[3] = currentTask.from;
                    pointsOnPath[4] = pointsOnPath[5] = pointsOnPath[6] = currentTask.to;
                    pointsOnPath[1].y = pointsOnPath[3].y = pointsOnPath[4].y = pointsOnPath[6].y = 0.4f;

                    for (int i = 0; i < 5; i++)
                    {
                        pointsOnPath[i + 1] += robotPosition;
                    }

                    currentPath = new RobotPath(robotPosition, pointsOnPath);
                }

                actionState = ActionState.EXECUTING_TASK;
                break;
            case ActionState.EXECUTING_TASK:
                if (currentTime < duration)
                {
                    currentTime += Time.deltaTime;
                    float progress = currentTime / duration;
                    float[] currentConfig = currentPath.GetConfiguration(progress);
                    for(int i = 0; i < 7; i++)
                    {
                        robotController.jointAngles[i] = currentConfig[i];
                    }
                }
                else
                {
                    currentTime = 0f;
                    currentTask.done = true;
                    actionState = ActionState.SELECTING_TASK;
                }
                break;
            case ActionState.FINISHED:
                Debug.Log("Plan finished");
                running = false;
                break;
        }
    }

    void LateUpdate()
    {
        if (currentTask == null)
        {
            return;
        }

        if (currentPath.currentPathSegment >= 3 && currentPath.currentPathSegment <= 5)
        {
            currentTask.taskObject.transform.position =
                robotController.currentFlunchPosition +
                new Vector3(0f, -currentTask.height, 0f);
        }
        
        if (currentPath.currentPathSegment == 6)
        {
            currentTask.taskObject.transform.position =
                        currentTask.to +
                        new Vector3(0f, -currentTask.height, 0f);
        }
    }
}
