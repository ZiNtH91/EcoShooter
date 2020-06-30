using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, transform.position.z);
    }

    public void ScreenShake()
    {
        // Triggers slight screen shake animation. Alters the rotation of the camera, so the "Follow-Player-Behavior" is not affected by animations
        //anim.SetTrigger("shake");
    }

}
