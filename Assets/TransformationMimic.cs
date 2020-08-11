using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformationMimic : MonoBehaviour
{
    public Transform flangeTransform;

    // Update is called once per frame
    void Update()
    {
        transform.position = flangeTransform.position;
        transform.rotation = flangeTransform.rotation;
    }
}
