using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*유저의 입력을 받아서 Command를 통해 버튼 입력을 처리 하는 클래스*/

public class InputHandler : MonoBehaviour
{
    #region command pattern variables

    public enum userInput { Move, MoveJump, Attack , Skill, None}
    private userInput pressedBtn = userInput.None;

    private ICommand CmdMove, CmdAttack, CmdIdle, CmdDie;
    private ICommand command;

    public Actor actor;

    #endregion

    #region input values
    public Transform camRotation;

    private float horizInput= 0f;
    private float vertInput= 0f;
    #endregion

    
    private void Start()
    {
        actor = GetComponent<Actor>();
        SetCommand();
    }

    void Update()
    {
        command = GetCommand();
        command.Execute(actor);
    }

    /* Command초기화 메서드*/
    public void SetCommand()
    {
        CmdMove = new MoveCommand();
        CmdAttack = new AttackCommand();
        CmdIdle = new IdleCommand();
    }

    /*유저의 키 입력값을 받아 그 입력에 해당하는 Command를 리턴하는 메서드
     * IsPressed메서드로 해당 키 입력을 체크
     */
    public ICommand GetCommand()
    {
        if (IsPressed(userInput.Move))
            return CmdMove;
        else if (IsPressed(userInput.Attack))
            return CmdAttack;
        //else if(IsPressed(userInput.))
        else
            return CmdIdle;
    }

    /* 인풋이 들어왔을때 Command에 해당하는 키가 입력 되었는지 체크하는 메서드
     * 해당 키 입력값을 받고 방향키 입력일 경우 방향키 입력의 값을 actor에게 넘겨줌.
     * 리턴 형은 bool
     */
    public bool IsPressed(userInput btn)
    {
        pressedBtn = userInput.None;

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            actor.Horizontal = Input.GetAxisRaw("Horizontal");
            actor.Vertical = Input.GetAxisRaw("Vertical");
            pressedBtn = userInput.Move;
        }
        else if (Input.GetKey(KeyCode.Space))
            pressedBtn = userInput.MoveJump;
        else if (Input.GetMouseButtonDown(0))
            pressedBtn = userInput.Attack;

        return 
            (btn == pressedBtn);
    }



}
