using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{
    public MoveCommand()
    { }

    public MoveCommand(Actor actor, float horizIpt, float vertIpt)
    {
        actor.Horizontal = horizIpt;
        actor.Vertical = vertIpt;
    }

    public void Execute(Actor actor)
    {
        actor.Move();
    }
}
