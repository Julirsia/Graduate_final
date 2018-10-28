using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Item
{
    public enum TrapType { attracter, damage };
    public TrapType trap = TrapType.attracter;

    public float buildTime;     //건설시 걸리는 시간
    public float duration;      //필드 지속시간

    public override void Action(Actor actor)
    {
        
    }

    IEnumerator AttracterDuration()
    {

        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
