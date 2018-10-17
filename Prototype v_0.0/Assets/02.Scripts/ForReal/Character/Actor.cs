using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 목적 : 게임 캐릭터의 베이스가되는 액터클래스.
 * 역할 : 명령 객체의 지시를 받는 리시버.
 */
public class Actor : Photon.PunBehaviour
{

    [SerializeField] float m_MovingTurnSpeed = 360;
    [SerializeField] float m_StationaryTurnSpeed = 180;
    [SerializeField] float m_RunCycleLegOffset = 0.2f;
    const float k_Half = 0.5f;
    private  float m_AnimSpeedMultiplier = 1f;
    private float m_MoveSpeedMultiplier = 1f;
    private float m_GroundCheckDistance = 0.1f;

    private Vector3 m_GroundNormal;
    private bool m_IsGrounded;

    #region input values
    private float horizontal;
    private float vertical;
    public float Horizontal
    {
        get { return horizontal; }
        set { horizontal = value; }
    }
    public float Vertical
    {
        get { return vertical; }
        set { vertical = value; }
    }
    #endregion

    #region 나중에 DB로 빠질 부분
    public int actorID = 0;//추후 액터 ID값에 따라 데이터를 받아오게 할 것 

    public float moveSpeed;
    public float jumpSpeed;
    #endregion

    #region components
    public Animator anim;
    private Transform myTr;
    float m_ForwardAmount;
    float m_TurnAmount;
    Rigidbody m_Rigidbody;
    #endregion

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();  // Start에서 Awake로 이동
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        myTr = transform;
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

    }

    public void Idle()
    {
        m_Rigidbody.velocity = Vector3.zero;

        anim.SetFloat("Forward", 0f, 0.1f, Time.deltaTime);
        anim.SetFloat("Turn", 0f, 0.1f, Time.deltaTime);
    }


    /* 함수명 : Move
     * 목적 : 캐릭터의 움직임을 담당하는 메서드
     * 리턴값 : null
     * 파라미터 : move - input에서 받은 캐릭터 이동값
     */
    public void Move(Vector3 move)
    {
        if (move.magnitude > 1f)
            move.Normalize();
        if (m_Rigidbody.velocity.magnitude < moveSpeed)
            m_Rigidbody.AddForce(move * moveSpeed);

        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);

        m_TurnAmount = Mathf.Atan2(move.x, move.z);     //바닥에서 유저의 회전값 a = atan move x/movez
        m_ForwardAmount = move.z;
        ApplyExtraTurnRotation();
        anim.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        anim.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);



        float runCycle =
                Mathf.Repeat(
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
        float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;

        if (m_IsGrounded)
        {
            anim.SetFloat("JumpLeg", jumpLeg);
        }


        if (m_IsGrounded && move.magnitude > 0)
        {
            anim.speed = m_AnimSpeedMultiplier;
        }
        else if (m_Rigidbody.velocity.magnitude < moveSpeed - 1)
        {
            anim.speed = 0.8f;
        }
        else if (m_Rigidbody.velocity.magnitude >= moveSpeed - 1)
        {
            anim.speed = 0.9f;
        }
        else
        {
            anim.speed = 1.1f;
        }
    }
    public void Attack()
    {
        Debug.Log("Attack");
        anim.SetBool("IsAttack", true);
        anim.SetTrigger("CloseAttack");

    }
    public void Die()
    {
        anim.SetTrigger("IsDie");
    }

    public void OnDamaged()
    {
        anim.SetTrigger("IsHit");
    }

    /* 함수명 : Apply Extra Turn Rotation
     * 목적 : 캐릭터를 회전시키는 메서드
     * 리턴 : bool pressed
     * 파라미터 : 유저가 누른 버튼키코드
     */
    void ApplyExtraTurnRotation()
    {
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }

    /* 함수명 : OnAnimatorMore() //콜백함수
     * 목적 : 애니메이터가 동작할때 콜되는 함수
     * 리턴 : void
     */
    public void OnAnimatorMove()
    {
        Vector3 v = (anim.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

        v.y = moveSpeed;
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR

        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif

        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
            anim.applyRootMotion = true;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
            anim.applyRootMotion = false;
        }
    }
}