using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//목적 : 좀비의 Actor을 조정하기 위한 클래스.
//유저의 이동은 Input Handler를 베이스로 하지만 AI는 AI 인터페이스를 오버라이드한 클래스에서 액터를 움직인다.
//stack FSM패턴 기반 설계
public class Ai_Zombie : MonoBehaviour, IAi
{
    private Stack<AiState> stateStack;  //이전의 상태가 필요할때 스택에 넣어서 기억해두는 용도

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
        /*
        if (Pattern() == AiState.Move)
            return new MoveCommand(actor, moveVector, isJump);
        else if (Pattern() == AiState.Attack)
            return new AttackCommand();
        else
            return new IdleCommand();*/
        return new IdleCommand(false);
    }

    /* 목적 : Iai가 갖고있는 패턴 실행자.
     * 
     */
    public AiState Pattern()
    {
        AiState state;
        state = AiState.Idle;

        return state;
    }
}
