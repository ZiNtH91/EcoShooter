using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    private float nextWayPointDistance = 0.05f;
    private float updateRepeatRate = 0.5f;

    private Path path;
    private int currentWayPoint = 0;
    private float reachedEndOfPathDistance = 0.25f;
    private bool reachedEndOfPath = false;

    private Seeker seeker;

    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, updateRepeatRate);
    }

    // Update is called once per frame
    void Update()
    {

        // Reset at the start of Update to ensure a "fresh" direction
        direction = Vector2.zero;

        // Stop if there is no path
        if (path == null)
        {
            return;
        }

        // Stop if the End of the Path is reached
        if(currentWayPoint >= path.vectorPath.Count || Vector2.Distance(target.position, transform.position) < reachedEndOfPathDistance)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }


        // Calculate
        direction = (path.vectorPath[currentWayPoint] - transform.position).normalized;

        // Check if the next Waypoint is to be called - Do this last to avoid problems with index range
        if (Vector2.Distance(path.vectorPath[currentWayPoint], transform.position) < nextWayPointDistance)
        {
            currentWayPoint += 1;
        }

    }

    /*
     * Path initialization if seeking is successfull
     */
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    /*
     * Invoke Regular Path Updates 
     */
    private void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
    }

    /*
     * Return the Intended Direction;
     */
    public Vector2 GetDirection()
    {
        return direction;
    }
}
