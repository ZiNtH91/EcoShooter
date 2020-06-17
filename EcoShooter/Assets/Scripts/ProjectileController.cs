using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    
    private float moveSpeed=5f;
    private Vector2 moveDirection;
    private Vector2 faceDirection;

    public GameObject explosion;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PerformMovement();
    }

    public void SetMoveDirection(Vector2 _moveDirection){
        moveDirection=_moveDirection;
    }

    
    void PerformMovement(){

        transform.position+=moveSpeed*(Vector3)moveDirection*Time.deltaTime;

        if(moveDirection!=Vector2.zero){
            faceDirection=moveDirection;
        }

        transform.up=(Vector3)faceDirection;
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().Die();
            Destroy(this.gameObject);
        }
        else if (other.transform.tag != "Player") 
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }       
    }
}
