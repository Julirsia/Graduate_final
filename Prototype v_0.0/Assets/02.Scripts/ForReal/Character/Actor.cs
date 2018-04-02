using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 목적 : 게임 캐릭터의 베이스가되는 액터클래스.
 * 역할 : 명령 객체의 지시를 받는 리시버.
 */
public class Actor : MonoBehaviour
{
    private float horizontal;
    private float vertical;

    public float Horizontal
    {
        get { return horizontal;}
        set { horizontal = value; }
    }
    public float Vertical
    {
        get { return vertical; }
        set { vertical = value; }
    }

    public int actorID = 0;//추후 액터 ID값에 따라 데이터를 받아오게 할 것 

    public Animator anim;
    private Transform myTr;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Idle()
    {
        anim.SetBool("IsWalk", true);
        anim.SetBool("IsAttack", false);
    }
    public void Move()
    {
        anim.SetBool("IsWalk", true);
        
    }
    public void Attack()
    {
        anim.SetBool("IsAttack", false);

    }
    public void Die()
    {
        anim.SetTrigger("IsDie");
    }

    public void OnDamaged()
    {
        anim.SetTrigger("IsHit");
    }
}
