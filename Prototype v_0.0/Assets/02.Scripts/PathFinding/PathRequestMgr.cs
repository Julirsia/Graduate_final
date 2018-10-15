using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//길찾기를 여러 유닛이 요청할 경우 부하를 방지 하기 위해 Queue로 path request를 관리하는 클래스
public class PathRequestMgr : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestMgr instance;
    PathFindingTest pathfinding;

    bool isProcessingPath;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<PathFindingTest>();
    }
    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }

    //path를 요청하는 메서드. 
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        Debug.Log("RequestPath");
       
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    public void TryProcessNext()
    {
        Debug.Log("TryProcessNext");
        
        //if not processing path and path request queue is not empty
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            Debug.Log("isProcessPath");
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        Debug.Log("FinishProcessingPath");
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }
    
}
