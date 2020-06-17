using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    protected GameObject player;
   
    // Start is called before the first frame update
    protected void Start()
    {
        player = GameObject.Find("Character");   
    }

    // Update is called once per frame
    protected void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            PowerUpEffect();
            Destroy(this.gameObject);
        }
    }

    protected virtual void PowerUpEffect()
    {

    }
}
