using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    #region Singleton
    private static RotationTest _instance;

    public static RotationTest Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public void SetPose(Vector3 newPos, Quaternion rotation)
    {
        transform.position = newPos;
        transform.rotation = rotation;
    }
}
