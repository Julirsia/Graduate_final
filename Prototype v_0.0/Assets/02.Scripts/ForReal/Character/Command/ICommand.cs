using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 명령 인터페이스.
 * 
 */
public interface ICommand 
{
    //이 인터페이스를 상속 받은 클래스의 명령을 실행함.
    void Execute(Actor actor);
    //void SetValue(Actor actor, float Vaue);
}
