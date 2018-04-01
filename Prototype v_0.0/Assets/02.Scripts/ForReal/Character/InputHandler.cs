using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public enum userInput { Move, MoveJump, Attack , Skill,None}
    userInput pressedBtn = userInput.None;

    ICommand CmdMove, CmdAttack, CmdDie;
    ICommand command;

    Actor actor;

    private void Start()
    {
        actor = GetComponent<Actor>();
    }

    public void SetCommand()
    {
        CmdMove = new MoveCommand();
        CmdAttack = new AttackCommand();
        CmdDie = new DieCommand();
    }

    void Update()
    {
        command = GetCommand();    
    }

    private ICommand GetCommand()
    {
        if (IsPressed(userInput.Move))
            return CmdMove;
        else if (IsPressed(userInput.Attack))
            return CmdAttack;
        //else if(IsPressed(userInput.))

        return null;
    }

    public bool IsPressed(userInput btn)
    {
        pressedBtn = userInput.None;

        if (Input.GetAxis("Horizontal") != 0||Input.GetAxis("Vertical") != 0)
            pressedBtn = userInput.Move;
        else if (Input.GetKey(KeyCode.Space))
            pressedBtn = userInput.MoveJump;
        else if (Input.GetMouseButtonDown(0))
            pressedBtn = userInput.Attack;

        return (btn == pressedBtn);
    }



}
