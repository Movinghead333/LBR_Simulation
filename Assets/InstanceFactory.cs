using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceFactory : MonoBehaviour
{
    #region Singleton
    private static InstanceFactory _instance;

    public static InstanceFactory Instance { get { return _instance; } }


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

    public void FixedUpdate()
    {
        
    }

    public GameObject CreatInstance(string name, Vector3 position)
    {
        return Instantiate(GameObject.Find("Red Block"), position, Quaternion.identity);
    }
}
