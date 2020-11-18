using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{
    #region Properties
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();// Queue of path requests to be done
    PathRequest currentPathRequest;//current working path request

    static PathRequestManager Instance;
    PathFinder pathFinder;

    bool isProcessingPath;//true if currently processing a paths
    #endregion

    #region Initialisation
    void Awake()
    {
        Instance = this;
        pathFinder = GetComponent<PathFinder>();
    }
    #endregion

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Unit requester, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, requester, callback);
        Instance.pathRequestQueue.Enqueue(newRequest);//add to queue
        Instance.TryToProcessNext();//attempt to process next in queue
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
        if (currentPathRequest.requester != null)
        {
            currentPathRequest.callback(path, isSuccess);
        }
        isProcessingPath = false;
        TryToProcessNext();
    }

    // Data structure for a path request
    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Unit requester;
        public Action<Vector3[], bool> callback;

        //constructor
        public PathRequest(Vector3 _start, Vector3 _end, Unit _requester, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            requester = _requester;
            callback = _callback;
        }
    }
}