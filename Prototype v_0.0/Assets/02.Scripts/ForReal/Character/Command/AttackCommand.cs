using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : ICommand
{
    int attackType;
    public AttackCommand(int typeOfAttack)
    {
        typeOfAttack = attackType;
    }

    public void Execute(Actor actor)
    {
        actor.Attack(attackType);
    }
}