using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform robotTransform;
    public float radius = 2f;
    public float height = 1.5f;

    [Range(0.25f, 4f)]
    public float speed = 1f;

    private float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * speed;
        Vector3 newPos = new Vector3(-Mathf.Sin(time) * radius, height, -Mathf.Cos(time) * radius);
        transform.position = newPos;

        transform.LookAt(robotTransform.position, Vector3.up);
    }
}
