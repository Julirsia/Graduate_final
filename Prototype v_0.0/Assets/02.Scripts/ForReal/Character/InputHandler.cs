using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*유저의 입력을 받아서 Command를 통해 버튼 입력을 처리 하는 클래스*/

public class InputHandler : MonoBehaviour
{
    #region command pattern variables

    public enum userInput { Move, MoveJump, Attack , Skill,None}
    private userInput pressedBtn = userInput.None;

    private ICommand CmdMove, CmdAttack, CmdDie;
    private ICommand command;

    public Actor actor;

    #endregion

    #region input values
    private float horizInput= 0f;
    private float vertInput= 0f;
    #endregion

    
    private void Start()
    {
        actor = GetComponent<Actor>();
    }

    public void SetCommand()
    {
        CmdMove = new MoveCommand(actor);
        CmdAttack = new AttackCommand(actor);
    }

    void Update()
    {
        command = GetCommand();    
    }

    public ICommand GetCommand()
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

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            pressedBtn = userInput.Move;
            horizInput = Input.GetAxis("Horizontal");
            vertInput = Input.GetAxis("Vertical");
        }
        else if (Input.GetKey(KeyCode.Space))
            pressedBtn = userInput.MoveJump;
        else if (Input.GetMouseButtonDown(0))
            pressedBtn = userInput.Attack;

        return (btn == pressedBtn);
    }



}
