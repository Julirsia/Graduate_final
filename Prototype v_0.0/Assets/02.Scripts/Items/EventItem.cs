using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventItem : Item
{
    EventReceiver receiver;
    /* 파라미터 : 해당 액션이 작용할 actor*/
    public override void Action(Actor actor)
    {
        if(receiver == null)
            receiver = actor.GetComponent<EventReceiver>();

        receiver.lv_0Flag = true;
        UIManager.instance.GetMedicine();
        gameObject.SetActive(false);
    }

}
