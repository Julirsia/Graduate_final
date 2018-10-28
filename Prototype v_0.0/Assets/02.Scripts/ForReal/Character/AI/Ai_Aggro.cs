using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai_Aggro : MonoBehaviour
{
    private AiHandler ai;
    public   List<Transform> collisionTr = new List<Transform>();
    private void Start()
    {
        ai = transform.parent.GetComponent<AiHandler>();    
    }

    public void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "PLAYER")
        {
            collisionTr.Add(coll.transform);
            ai.currnetTarget = coll.transform;
            ai.aggroFlag = true;
        }
    }
    public void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "PLAYER")
        {
            if (collisionTr.Contains(coll.transform))
            {
                collisionTr.Remove(coll.transform);
                StartCoroutine(CalmAggro());
            }
           
        }
    }

    IEnumerator CalmAggro()
    {
        yield return new WaitForSeconds(2f);
        ai.aggroFlag = false;
        ai.currnetTarget = ai.finalTarget;
        ai.RefreshPath();
    }
}
