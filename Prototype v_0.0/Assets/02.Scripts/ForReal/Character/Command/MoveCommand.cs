using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : Photon.PunBehaviour, ICommand 
{
     private Vector3 f_move;

    public MoveCommand(Actor actor, float horizIpt, float vertIpt)
    {
        actor.Horizontal = horizIpt;
        actor.Vertical = vertIpt;
        f_move = vertIpt * Vector3.forward + horizIpt * Vector3.right; 
    }

    public void Execute(Actor actor)
    {
        actor.Move(f_move);
    }
}
