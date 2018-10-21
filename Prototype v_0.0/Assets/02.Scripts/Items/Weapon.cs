using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    //무기 타입
    public enum WeaponType { close_Combat, longDist_Combat, throw_item };
    public WeaponType weaponType = WeaponType.close_Combat;
    public int weaponCode;  //캐릭터 애니메이션 파라미터
    public int attackPower =0;


    public override void GoInventory(Actor actor)
    {
        base.GoInventory(actor);
        actor.currentWeaponCode = weaponCode;
        
    }
    /* 목적 : 근접무기가 내가 아닌 타 캐릭터에게 닿을 경우 데미지를 준다
     */
    public override void Action(Actor actor)
    {
        actor.OnDamaged(attackPower);
    }

}
