using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public float angleVelocity = 1;
    float xzAngle = 0f;
    public float radius = 0.5f;
    public float yScale = 0.1f;
    public float yOffset = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xzAngle += Time.deltaTime * angleVelocity;

        float curRad = xzAngle * Mathf.Deg2Rad;

        Vector3 pos = new Vector3(radius * Mathf.Cos(curRad), yOffset + yScale * (1 + Mathf.Sin(curRad)), radius * Mathf.Sin(curRad));
        transform.position = pos;
    }
}
