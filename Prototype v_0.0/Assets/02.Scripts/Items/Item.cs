using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public int ItemCode;
    public enum ItemType { heal, weapon, construction, };
    public ItemType type;

    protected int itemID;
    public enum ItemStatus { onField, onInventory, equiped}
    protected ItemStatus status;

    protected Actor owner;

    public void GetData()
    { }

    public virtual void GoInventory(Actor actor)
    {
        actor.currentWeapon = transform;

    }

    /* 파라미터 : 해당 액션이 작용할 actor*/
    public virtual void Action(Actor actor)
    { }

    public void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "PLAYER")
        {
            if (status == ItemStatus.onField)
                GoInventory(coll.gameObject.GetComponent<Actor>());
            //인벤토리에 들어가게 되면 근접무기 외엔 이 부분이 활성화 되지 않슴
            else if (coll.gameObject.GetComponent<Actor>() != owner)
                Action(coll.gameObject.GetComponent<Actor>());
        }
    }
}
