using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventChecker : MonoBehaviour
{
    public RaidNpc npc;
    EventReceiver receiver;
    private void Start()
    {
        npc = transform.root.GetComponent<RaidNpc>();
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "PLAYER" && npc.eventLevel == 0)
        {
            if (!coll.GetComponent<EventReceiver>().lv_0Flag)
                npc.Lv0EventActive();
            else
                npc.eventLevel++;
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "PLAYER" && npc.eventLevel == 0)
        {
            npc.Lv0EventDisactive();

        }
    }
}
