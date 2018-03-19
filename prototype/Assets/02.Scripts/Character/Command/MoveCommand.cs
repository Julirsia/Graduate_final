using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand {

    public void Execute(Actor actor)
    {
        actor.Move();
    }
}
