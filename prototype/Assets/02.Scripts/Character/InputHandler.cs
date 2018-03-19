using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public delegate ICommand CommandMgr();
    public CommandMgr[] commander;

    private void Start()
    {
        //commander = new CommandMgr[3] { MoveCommand, AttackCommand, DieCommand };
    }
    void Update()
    {
        GetInput();
    }

    /* GetInput
     * 목적 : Update에서 작동 
     */
    public void GetInput()
    {
     //   if()
    }
}
