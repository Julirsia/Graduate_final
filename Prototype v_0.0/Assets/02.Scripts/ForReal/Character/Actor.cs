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
    bool m_IsGrounded;
    [SerializeField] float m_AnimSpeedMultiplier = 1f;
    Vector3 m_GroundNormal;
    [SerializeField] float m_GroundCheckDistance = 0.1f;
    [SerializeField] float m_MoveSpeedMultiplier = 1f;

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
    float yaw;
    float pitch;
    float mouseSensitivity = 10;
    public float rotationSmoothTime = .12f;
    Vector3 currentRotation;
    Vector3 rotationSmoothVelocity;
    #endregion

    #region components
    public Animator anim;
    private Transform myTr;
    float m_ForwardAmount;
    float m_TurnAmount;
    Rigidbody m_Rigidbody;
    Vector2 mouseInput;
    InputHandler playerInput;
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

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void LateUpdate()
    {
        yaw += mouseInput.x * mouseSensitivity;
        pitch -= mouseInput.y * mouseSensitivity;

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        //transform.eulerAngles = currentRotation;
    }

    public void Idle()
    {

    }
    public void Move(Vector3 move)
    {

        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);

        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
       // m_TurnAmount = Mathf.Atan2(yaw, move.z);
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
        else
        {

            anim.speed = 1;
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

    void ApplyExtraTurnRotation()
    {
        // 캐릭터가 회전할 때 속도를 보정해 주는 함수
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }

    public void OnAnimatorMove()
    {
        // OnAnimatorMove root모션에 기반하여 움직이는 함수 (공부중)

        {
            Vector3 v = (anim.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;


            v.y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = v;
        }
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