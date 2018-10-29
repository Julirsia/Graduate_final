using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using clientData;

public abstract class Item : MonoBehaviour
{
    public int ItemCode;
    public ItemType type;

    protected int itemID;
    protected ItemStatus status;

    public Actor owner;

    private void Start()
    {
        if (type == ItemType.weapon)
        {
            owner = transform.root.GetComponent<Actor>();
            status = ItemStatus.equiped;
        }
    }
    public void GetData()
    { }

    public virtual void GoInventory(Actor actor)
    {
        //임시 테스트용
        if (type == ItemType.weapon)
        {
            owner = actor;
            actor.currentWeapon = transform;
            status = ItemStatus.equiped;
            //status = ItemStatus.onInventory;
        }
        else
        {
            owner = actor;
            status = ItemStatus.onInventory;
        }
    }

    /* 파라미터 : 해당 액션이 작용할 actor*/
    public virtual void Action(Actor actor)
    { }

    public void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "PLAYER" )
        {
            if (status == ItemStatus.onField)
                GoInventory(coll.gameObject.GetComponent<Actor>());
            //인벤토리에 들어가게 되면 근접무기 외엔 이 부분이 활성화 되지 않슴
            else if (coll.gameObject.GetComponent<Actor>() != owner)
                Action(coll.gameObject.GetComponent<Actor>());
            else if(type == ItemType.eventItem)
                Action(coll.gameObject.GetComponent<Actor>());
        }
        else if (coll.tag == "ACTOR")
        {
            if (coll.gameObject.GetComponent<Actor>() != owner)
                Action(coll.gameObject.GetComponent<Actor>());
        }
    }
}
