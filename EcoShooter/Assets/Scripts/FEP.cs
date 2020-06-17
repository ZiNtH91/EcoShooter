using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FEP : MonoBehaviour
{
    public LayerMask enemyLayer;
    private GameObject target;
    private Animator anim;

    public GameObject explosion;

    private float activeRange = 1.5f;

    private Vector2 faceDirection;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        InvokeRepeating("DetermineTarget", 0, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
        DetermineAction();
    }


    private void DetermineTarget()
    {
        // Reset target
        target = null;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, activeRange, enemyLayer);

        if (hitColliders.Length > 0)
        {
            List<Collider2D> enemies = hitColliders.ToList<Collider2D>();

            float nearestDist = float.MaxValue;
            GameObject nearestObject = null;

            foreach (Collider2D enemy in enemies)
            {
                if (Vector2.Distance(transform.position, enemy.gameObject.transform.position) < nearestDist)
                {
                    nearestDist = Vector2.Distance(transform.position, enemy.gameObject.transform.position);
                    nearestObject = enemy.gameObject;
                }
            }

            target = nearestObject;
        }
    }

    private void DetermineAction()
    {
        if (target != null)
        {
            faceDirection= (target.transform.position - transform.position).normalized;
            transform.up = faceDirection;
            anim.SetBool("isAttacking", true);
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (target != null && collision.gameObject == target) 
        {
            Instantiate(explosion, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }
}
