using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour 
{
    Animal animal;
    Transform targetOld;

    float speed = 20;
    float rotationSpeed = 10;
    Vector3[] path;
    int targetIndex;

    void Awake()
    {
        //animal = FindObjectOfType<Animal>();
        animal = GetComponent<Animal>();
    }

    void Update()
    {
        //if animal has a target
        if(animal.target != null)
        {
            if(targetOld == null)//if target not set set the target
            {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
                targetOld = animal.target;
                print("transform: "+transform.position.ToString());
                print("animal target:"+animal.target.position.ToString());
                PathRequestManager.RequestPath(transform.position, animal.target.position, OnPathFound);
            }
            if(animal.target.position != targetOld.position)// if the target position has changed or target not set
            {
                targetOld = animal.target;
                PathRequestManager.RequestPath(transform.position, animal.target.position, OnPathFound);
=======
=======
>>>>>>> Stashed changes
                targetOld = target;
                //print("transform: "+transform.position.ToString());
                //print("animal target:"+target.ToString());
                PathRequestManager.RequestPath(transform.position, target, OnPathFound);
>>>>>>> Stashed changes
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
        Vector3 currentWaypoint = path[0];
        while (true) 
        {
            //reached current waypoint
            if (transform.position == currentWaypoint) 
            {
                targetIndex ++;
                if (targetIndex >= path.Length) // finished following path
                {
                    targetIndex = 0;//reset index for multiple runs
                    path = new Vector3[0];
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
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);//go closer to target position
            yield return null;//wait 1 frame before returning
        }
    }

    //draws the path 
    public void OnDrawGizmos() 
    {
        if (path != null) 
        {
            for (int i = targetIndex; i < path.Length; i ++) 
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
                    Gizmos.DrawLine(path[i-1],path[i]);
                }
            }
        }
    }
}