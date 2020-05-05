using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionProbe : MonoBehaviour
{
    #region Singleton
    private static PositionProbe _instance;

    public static PositionProbe Instance { get { return _instance; } }


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

    public void SetPositionProbe(Vector3 newPos)
    {
        transform.position = newPos;
    }
}
