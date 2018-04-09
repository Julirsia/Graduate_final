using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HY;

public class CloseCombat : StateMachineBehaviour {

    public Player player;
    public PlayerAttackEffect playerEffect;

    public float startTime;
    public float endTime;
    public float moveTime; //움직이기 시작해도 되는 시간

    public bool isActive; //데미지 히트박스가 활성화 되었는가
    public int AttackID;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
            player = animator.gameObject.GetComponent<Player>();
        if (playerEffect == null)
            playerEffect = animator.gameObject.GetComponent<PlayerAttackEffect>();

        player.StartAttack(AttackID);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.DoingAttack();
        
        //데미지 주는 모션 시작시간 <= 현재 시간 < 모션 끝시간이고 히트박스가 비활성화일때
        if (stateInfo.normalizedTime >= startTime && stateInfo.normalizedTime < endTime && !isActive)
        {
            isActive = true;
            player.GiveDamage();
            playerEffect.KnifeAttackStart();
        }
        //모션시간 끝 < 현재시간 && 히트박스 활성화상태 => 비활성화로
        else if (stateInfo.normalizedTime >= endTime && isActive)
        {
            isActive = false;
            player.EndDamage();
            playerEffect.KnifeAttackEnd();
        }
        //모션 끝
        if (stateInfo.normalizedTime > moveTime)
        {
            player.EndAttack();
        }

    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.EndAttack();
    }
}