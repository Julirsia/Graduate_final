using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleCommand : ICommand
{
    private bool isCrouch;

    public IdleCommand(bool crouch)
    {
        isCrouch = crouch;
    }

    public void Execute(Actor actor)
    {
        actor.Idle(isCrouch);
    }
}
