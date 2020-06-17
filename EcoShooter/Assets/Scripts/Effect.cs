using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{

    void Start()
    {
        Camera.main.GetComponent<CameraController>().ScreenShake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyEffect()
    {
        Destroy(this.gameObject);
    }
}
