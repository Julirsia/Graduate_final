using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using clientData;

public class AiHandler : MonoBehaviour , IAi
{
    protected Stack<AiState> stateStack;  //이전의 상태가 필요할때 스택에 넣어서 기억해두는 용도
    private ICommand command;
    public Actor actor;

    private bool isJump = false;
    public Vector3 moveVector;

    public Transform finalTarget;
    public Transform currnetTarget;
    protected Vector3[] path;
    protected int targetIndex=0;

    public bool aggroFlag { get { return aggro; } set { aggro = value; } }
    public bool aggro;

    private void Start()
    {
        actor = GetComponent<Actor>();
        PathRequestMgr.RequestPath(transform.position, finalTarget.position, OnPathFound);
    }

    public void RefreshPath()
    {
        Debug.Log("RefreshPath");
        targetIndex = 0;
        Array.Clear(path,0,path.Length);
        PathRequestMgr.RequestPath(transform.position, currnetTarget.position, OnPathFound);
    }

    private void Update()
    {
        if (!actor.deathSignal)
        {
            command = GetCommand();
            command.Execute(actor);
        }
    }

    public ICommand GetCommand()
    {
        if (Pattern() == AiState.Move)
            return new MoveCommand(actor, moveVector, isJump, false);
        else if (Pattern() == AiState.Attack)
        {
            actor.LockOn(currnetTarget); 
            return new AttackCommand(0);
        }
        else
            return new IdleCommand(false);
    }

    /* 목적 : ai actor 가 갖고있는 패턴 실행자.
     * 
     */
    public virtual AiState Pattern()
    {
        return AiState.Idle;
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            Debug.Log("OnPathFound : Success");
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Debug.Log("FollowPath");
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (Vector3.Distance(transform.position,currentWaypoint) < 1f)
            {
                Debug.Log("Set");
                targetIndex++;
                if (targetIndex >= path.Length)
                    yield break;
                currentWaypoint = path[targetIndex];
            }

            moveVector = currentWaypoint - transform.position;
            yield return null;
        }
    }

    //디버그용 메서드. 길찾기 알고리즘을 통해 찾은 길을 
  /*  public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                    Gizmos.DrawLine(transform.position, path[i]);

                else
                    Gizmos.DrawLine(path[i - 1], path[i]);

            }
        }
    }*/
}
