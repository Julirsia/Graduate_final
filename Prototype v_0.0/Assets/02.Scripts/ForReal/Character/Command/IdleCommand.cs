using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleCommand : ICommand
{
    public void Execute(Actor actor)
    {
        actor.Idle();
    }
}
