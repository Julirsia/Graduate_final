using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*유저의 입력을 받아서 Command를 통해 버튼 입력을 처리 하는 클래스*/

public class InputHandler : MonoBehaviour //: Photon.PunBehaviour
{
    #region command pattern variables

    public enum userInput { Move, MoveJump, Attack, Skill, None }
    private userInput pressedBtn = userInput.None;

    private ICommand command;

    public Actor actor;

    #endregion

    #region input values
    public Transform camRotPivot;

    public int m_PlayerNumber = 1;
    private float horizInput = 0f;
    private float vertInput = 0f;
    private bool isJump;
    private Vector3 moveVector = Vector3.zero;
    #endregion


    private void Start()
    {
        actor = GetComponent<Actor>();
        //이부분 포톤이랑 연동되면서 무조건 false로 됨
        /*
        if (!photonView.isMine)
        {
            enabled = false;
        }*/
    }

    void Update()
    {
        command = GetCommand();
        command.Execute(actor);

    }

    /*유저의 키 입력값을 받아 그 입력에 해당하는 Command를 리턴하는 메서드
     * IsPressed메서드로 해당 키 입력을 체크
     */
    public ICommand GetCommand()
    {
        if (IsPressed(userInput.Move))
            return new MoveCommand(actor, moveVector, isJump);
        else if (IsPressed(userInput.Attack))
            return new AttackCommand();
        else
            return new IdleCommand();
    }

    /* 함수명 : IsPressed
     * 목적 : - 인풋이 들어왔을때 Command에 해당하는 키가 입력 되었는지 체크하는 메서드
     *        - 해당 키 입력값을 받고 방향키 입력일 경우 방향키 입력의 값을 actor에게 넘겨줌.
     * 리턴 : bool pressed
     * 파라미터 : 유저가 누른 버튼키코드
     */
    public bool IsPressed(userInput btn)
    {
        pressedBtn = userInput.None;

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            horizInput = Input.GetAxis("Horizontal");
            vertInput = Input.GetAxis("Vertical");
            moveVector = camRotPivot.forward * vertInput + camRotPivot.right * horizInput;
            pressedBtn = userInput.Move;
        }
        else if (Input.GetMouseButtonDown(0))
            pressedBtn = userInput.Attack;

        isJump = Input.GetKey(KeyCode.Space);


        bool crouch = Input.GetKey(KeyCode.C);

        return
            (btn == pressedBtn);
    }

}
