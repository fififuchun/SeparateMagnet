using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    // private Vector3 kentoRotation= new Vector3(0, 0, 45);
    public void PushRotateButton()
    {
        KentoManager.instance.gameObject.transform.Rotate(new Vector3(0, 0, 45));
    }
}
