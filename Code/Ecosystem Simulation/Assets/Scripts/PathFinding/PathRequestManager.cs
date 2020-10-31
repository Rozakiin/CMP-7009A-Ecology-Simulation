using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour 
{

    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();// Queue of path requests to be done
    PathRequest currentPathRequest;//current working path request

    static PathRequestManager instance;
    PathFinder pathFinder;

    bool isProcessingPath;//true if currently processing a paths

    void Awake() 
    {
        instance = this;
        pathFinder = GetComponent<PathFinder>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) 
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);//add to queue
        instance.TryToProcessNext();//attempt to process next in queue
    }

    //check if currently processing path, if not then process next path request
    void TryToProcessNext() 
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0) 
        {
            currentPathRequest = pathRequestQueue.Dequeue();//get first item in queue and remove it
            isProcessingPath = true;//now processing a path
            pathFinder.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);//start processing the path
        }
    }

    //called by pathfinding script once finished finding the path
    public void FinishedProcessingPath(Vector3[] path, bool isSuccess) 
    {
        currentPathRequest.callback(path, isSuccess);
        isProcessingPath = false;
        TryToProcessNext();
    }

// Data structure for a path request
    struct PathRequest 
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        //constructor
        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) 
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}