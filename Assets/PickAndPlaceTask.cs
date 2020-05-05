using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAndPlaceTask
{
    public Vector3 from;
    public Vector3 to;
    public bool done = false;
    public float height;

    public GameObject taskObject;

    public string name;



    public PickAndPlaceTask(Vector3 from, Vector3 to, float height, string name)
    {
        this.from = from;
        this.to = to;
        this.height = height;
        this.name = name;

        Vector3 initialPosition = from + new Vector3(0f, -height, 0f);
        switch (name)
        {
            case "red block":
                Debug.Log("loading red block");
                GameObject redBlock = Resources.Load("Prefabs/Red Block") as GameObject;
                taskObject = GameObject.Instantiate(redBlock, initialPosition, Quaternion.identity);
                break;
            case "blue cube":
                Debug.Log("loading red block");
                GameObject blueCube = Resources.Load("Prefabs/Blue Cube") as GameObject;
                taskObject = GameObject.Instantiate(blueCube, initialPosition, Quaternion.identity);
                break;
        }
    }
}
