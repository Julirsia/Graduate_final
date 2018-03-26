using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 목적 : 게임 캐릭터의 베이스가되는 액터클래스.
 * 역할 : 명령 객체의 지시를 받는 리시버.
 */
public class Actor : MonoBehaviour
{
    //public ICommand command = InputHandler.GetInput();
    public Animator anim;
    private Transform myTr;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Idle()
    { }
    public void Move()
    {
        Transform forwd = myTr;
        //forwd.forward = camRot.forward;
        //Vector3 dir = HorizontalWrapMode * forwd.right + 
    }
    public void Attack()
    { }
    public void Die()
    { }

    public void OnDamaged()
    { }
}
