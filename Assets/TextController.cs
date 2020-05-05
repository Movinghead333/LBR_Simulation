using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    #region Singleton
    private static TextController _instance;

    public static TextController Instance { get { return _instance; } }


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


    }
    #endregion

    public Text textMesh;

    public void setText(string text)
    {
        textMesh.text = text;
    }
}
