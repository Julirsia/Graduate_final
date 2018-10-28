using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using clientData;
//목적 : 좀비의 Actor을 조정하기 위한 클래스.
//유저의 이동은 Input Handler를 베이스로 하지만 AI는 AI 인터페이스를 오버라이드한 클래스에서 액터를 움직인다.
//stack FSM패턴 기반 설계

public class Ai_Zombie : AiHandler  
{
    public int zombiePhase = 1;
    public bool actionFlag = false;
    private AiState state;
    //AI 제어부. Actor를 실질적으로 제어하는 패턴부분
    public override AiState Pattern()
    {
        if (aggroFlag)
        {
            RefreshPath();
            return AiState.Attack;
        }
        else
        {
            if (path !=null && targetIndex == path.Length)
                return AiState.Idle;

            else if (!actionFlag)
            {
                StartCoroutine(ZombieAction());
            }
            return state;

        }
    }

    IEnumerator ZombieAction()
    {
        actionFlag = true;
        state = AiState.Move;
        yield return new WaitForSeconds(5f);
        state = AiState.Idle;
        yield return new WaitForSeconds(3f);
        actionFlag = false;

    }

}
