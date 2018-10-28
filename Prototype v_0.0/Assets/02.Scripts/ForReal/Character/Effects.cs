using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour {

    public Animator pivotAnim;

    public void PivotMove()
    {
        pivotAnim.SetTrigger("HitEffect");
    }
}
