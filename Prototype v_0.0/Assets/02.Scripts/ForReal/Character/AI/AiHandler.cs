using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AiHandler : MonoBehaviour , IAi
{
    private Stack<AiState> stateStack;  //이전의 상태가 필요할때 스택에 넣어서 기억해두는 용도
    private ICommand command;
    public Actor actor;

    private bool isJump = false;
    public Vector3 moveVector;

    public Transform target;
    public float speed = 1;
    Vector3[] path;
    int targetIndex;


    private void Start()
    {
        actor = GetComponent<Actor>();
        PathRequestMgr.RequestPath(transform.position, target.position, OnPathFound);
    }

    private void Update()
    {
        command = GetCommand();
        command.Execute(actor);
    }

    public ICommand GetCommand()
    {

        if (Pattern() == AiState.Move)
            return new MoveCommand(actor, moveVector, isJump, false);
        else if (Pattern() == AiState.Attack)
            return new AttackCommand(0);
        else
            return new IdleCommand(false);

        //return new IdleCommand(false);
    }

    /* 목적 : ai actor 가 갖고있는 패턴 실행자.
     * 
     */
    public AiState Pattern()
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

    //이부분 FollowPath를 Move로 이동시켜야.
    IEnumerator FollowPath()
    {
        Debug.Log("FollowPath");
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                    yield break;
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed);
            yield return null;
        }
    }


}
