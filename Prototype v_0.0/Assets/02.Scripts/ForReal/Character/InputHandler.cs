using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public delegate ICommand CommandMgr();
    public CommandMgr[] commander;

    private float horizontal =0;
    private float vertical =0 ;

    private void Start()
    {
        //commander = new CommandMgr[3] { MoveCommand, AttackCommand, DieCommand };
    }
    void Update()
    {
        //GetInput();
    }

    /* GetInput
     * 목적 : Update에서 작동 
     */

}
