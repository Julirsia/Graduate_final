using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchSet : StateMachineBehaviour
{
    public Actor player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        if (player == null)
            player = animator.gameObject.GetComponent<Actor>();
        player.SetCrouchState();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        player.SetNormalState();
    }
}
