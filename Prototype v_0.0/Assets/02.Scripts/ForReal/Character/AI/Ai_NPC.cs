using System.Collections;
using System.Collections.Generic;
using clientData;
using UnityEngine;

public class Ai_NPC : AiHandler
{
    public RaidNpc npc;
    public bool actionFlag = false;
    private bool wakeupFlag = false;
    public AiState state;
    public void Start()
    {
        npc = GetComponent<RaidNpc>();
    }
    public override AiState Pattern()
    {
        if (npc.eventLevel < 1)
            return AiState.Idle;
        else
        {
            if (!wakeupFlag)
            {
                actor.anim.SetInteger("EventLv", npc.eventLevel);
                currnetTarget = finalTarget;
                RefreshPath();
                wakeupFlag = true;
            }

            if (path != null && targetIndex == path.Length)
                return AiState.Idle;

            else if (!actionFlag)
            {
                StartCoroutine(NPCNormalAction());
            }
            return state;
        }
    }
    IEnumerator NPCNormalAction()
    {
        actionFlag = true;
        state = AiState.Move;
        yield return new WaitForSeconds(5f);
        state = AiState.Idle;
        yield return new WaitForSeconds(2f);
        actionFlag = false;
    }
}
