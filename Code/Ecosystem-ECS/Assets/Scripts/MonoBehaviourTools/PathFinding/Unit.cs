using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    #region Properties
    private Vector3 targetOld;
    private Vector3 target;

    private float moveSpeed = 25;
    private float rotationSpeed = 10;
    private Vector3[] path;
    private int targetIndex;
    #endregion

    #region Initialisation
    void Awake()
    {

    }
    #endregion

    void Update()
    {
        //target = animal.GetTarget();
        //if animal has a target
        if (target != transform.position)
        {
            if (target != targetOld)// if the target position has changed
            {
                targetOld = target;
                PathRequestManager.RequestPath(transform.position, target, this, OnPathFound);
            }
        }
    }

    // 
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath"); //stop first incase already running
            StartCoroutine("FollowPath");
        }
    }

    // follows the path
    IEnumerator FollowPath()
    {
        if (path.Length > 0)//possible fix to index out of bounds error 
        {
            Vector3 currentWaypoint = path[0];
            while (true)
            {
                //reached current waypoint
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length) // finished following path
                    {
                        targetIndex = 0;//reset index for multiple runs
                        path = new Vector3[0];
                        //animal.SetTarget(transform.position); //reset target to self so animal knows it has finished following path
                        yield break;//break out of coroutine
                    }
                    currentWaypoint = path[targetIndex];// set current waypoint to the vector3 in the path at current index
                }

                //Rotate Towards Next Waypoint
                Vector3 targetDir = currentWaypoint - transform.position;
                float step = rotationSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
                transform.rotation = Quaternion.LookRotation(newDir);

                //move towards next waypoint
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, moveSpeed * Time.deltaTime);//go closer to target position
                yield return null;//wait 1 frame before returning
            }
        }
    }

    //draws the path 
    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawCube(path[i], Vector3.one);

                //draw lines between nodes now they are simplified
                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}