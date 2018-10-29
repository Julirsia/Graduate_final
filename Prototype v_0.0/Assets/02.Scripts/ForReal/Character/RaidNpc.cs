using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidNpc : MonoBehaviour
{
    public int eventLevel = 0;
    public GameObject lv0Panel;

    public void Lv0EventActive()
    {
        lv0Panel.SetActive(true);
    }
    public void Lv0EventDisactive()
    {
        lv0Panel.SetActive(false);
    }
   
}
