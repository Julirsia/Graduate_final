using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCommand : ICommand {

    /**/
    public void Execute(Actor actor)
    {
        actor.Die();
    }
}
