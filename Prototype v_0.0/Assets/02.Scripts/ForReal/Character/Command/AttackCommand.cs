using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCommand : ICommand
{
    public AttackCommand(Actor actor)
    {
        Execute(actor);
    }
    public void Execute(Actor actor)
    {
        actor.Attack();
    }
}