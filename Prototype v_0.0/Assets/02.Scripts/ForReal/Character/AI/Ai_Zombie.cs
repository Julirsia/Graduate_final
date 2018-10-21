using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//목적 : 좀비의 Actor을 조정하기 위한 클래스.
//유저의 이동은 Input Handler를 베이스로 하지만 AI는 AI 인터페이스를 오버라이드한 클래스에서 액터를 움직인다.
//stack FSM패턴 기반 설계
public class Ai_Zombie : MonoBehaviour, IAi
{
    private Stack<AiState> stateStack;  //이전의 상태가 필요할때 스택에 넣어서 기억해두는 용도

    public int zombiePhase = 1;

    private ICommand command;
    public Actor actor;

    private bool isJump = false;
    public Vector3 moveVector;

    private void Start()
    {
        actor = GetComponent<Actor>();
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
        AiState state;
        state = AiState.Idle;


        return state;
    }

    PathRequestMgr requestMgr;
    Grid grid;

    private void Awake()
    {
        requestMgr = GetComponent<PathRequestMgr>();
        grid = GetComponent<Grid>();

    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        Debug.Log("StartFindingPath");
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Debug.Log("FindPath");
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node targetNode = grid.GetNodeFromWorldPoint(targetPos);

        Debug.Log("start : " + startNode.gridX + "," + startNode.gridY);
        Debug.Log("target : " + targetNode.gridX + "," + targetNode.gridY);

        startNode.parent = startNode;

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                        continue;

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movementPenalty;

                    if (newMovementCostToNeighbour < currentNode.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);

                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
            waypoints = RetracePath(startNode, targetNode);
        requestMgr.FinishedProcessingPath(waypoints, pathSuccess);
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        Debug.Log("RetracePath");
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);

        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            //두 벡터의 방향이 다르면 새 웨이포인트로 저장. 같을경우 이 다음 웨이포인트까지 같은 방향으로 이동
            if (directionNew != directionOld)
                waypoints.Add(path[i].worldPosition);
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    public int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);

        return 14 * distX + 10 * (distY - distX); ;
    }

}
