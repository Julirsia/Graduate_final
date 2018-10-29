using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpNPCEvent : StateMachineBehaviour
{
    public Actor actor;

    public float startTime;
    public float endTime;

    public bool isActive; //데미지 히트박스가 활성화 되었는가

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actor == null)
            actor = animator.gameObject.GetComponent<Actor>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //데미지 주는 모션 시작시간 <= 현재 시간 < 모션 끝시간이고 히트박스가 비활성화일때
        if (!isActive && stateInfo.normalizedTime >= startTime && stateInfo.normalizedTime < endTime)
        {
            isActive = true;
            actor.CharacterStopEvent();
        }
        //모션시간 끝 < 현재시간 && 히트박스 활성화상태 => 비활성화로
        else if (stateInfo.normalizedTime >= endTime && isActive)
        {
            isActive = false;
            actor.CharacterMoveEvent();
        }
    }
}

