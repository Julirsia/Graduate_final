using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 목적 : 게임 캐릭터의 베이스가되는 액터클래스.
 * 역할 : 명령 객체의 지시를 받는 리시버.
 */
public class Actor : MonoBehaviour
{

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Idle() { }
    public void Move() { }
    public void Attack() { }
    public void Die() { }

    public void OnDamaged()
    { }
}
