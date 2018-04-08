using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 목적 : 게임 캐릭터의 베이스가되는 액터클래스.
 * 역할 : 명령 객체의 지시를 받는 리시버.
 */
public class Actor : MonoBehaviour
{
    #region input values
    public Transform camRot;
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
    #endregion

    #region 나중에 DB로 빠질 부분
    public int actorID = 0;//추후 액터 ID값에 따라 데이터를 받아오게 할 것 

    public float moveSpeed;
    public float jumpSpeed;
    #endregion

    #region components
    public Animator anim;
    private Transform myTr;
    #endregion

    void Start()
    {
        anim = GetComponent<Animator>();
        myTr = transform;
        camRot = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Idle()
    {
        anim.SetBool("IsWalk", false);
        anim.SetBool("IsAttack", false);
    }
    public void Move()
    {
        anim.SetBool("IsWalk", true);

        Transform forwd = myTr;
        forwd.forward = camRot.forward;
        Vector3 dir = horizontal * Vector3.right + vertical * forwd.forward;
        myTr.position += (dir.normalized * moveSpeed * Time.deltaTime);
    }
    public void Attack()
    {
        Debug.Log("Attack");
        anim.SetBool("IsAttack", true);
        anim.SetTrigger("CloseAttack");

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
