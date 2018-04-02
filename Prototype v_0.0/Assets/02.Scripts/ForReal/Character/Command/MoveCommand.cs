using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{
    public MoveCommand(Actor actor)
    {
        Execute(actor);
    }
    public void Execute(Actor actor)
    {
        actor.Move();
    }

    public void Execute(Actor actor, float horizInput, float vertInput)
    {
        actor.Move();
        actor.Horizontal = horizInput;
        actor.Vertical = vertInput;
    }
    
    public void GetTurnValue(float turnValues)
    { }
}
