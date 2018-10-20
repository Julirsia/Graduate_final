using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public int ItemCode;
    public enum ItemType { heal, weapon, construction, };
    public ItemType type;

    private int itemID;

    public void GetData()
    { }
    
    public void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "PLAYER")
        { }
    }
}
