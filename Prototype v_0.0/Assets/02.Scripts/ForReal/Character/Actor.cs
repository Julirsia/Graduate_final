using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] float m_MovingTurnSpeed = 360;
    [SerializeField] float m_StationaryTurnSpeed = 180;
    [SerializeField] float m_RunCycleLegOffset = 0.2f;

    [Range(1f, 4f)] [SerializeField] float m_GravityMultiplier = 2f;
    const float k_Half = 0.5f;
    public int m_PlayerNumber = 1;

    [SerializeField] float m_AnimSpeedMultiplier = 1f;

    [SerializeField] float m_GroundCheckDistance = 10f;
    [SerializeField] float m_MoveSpeedMultiplier = 1f;

    [SerializeField] float m_JumpPower = 12f;

    Vector3 m_GroundNormal;
    bool m_IsGrounded;

    #region input values

    private float horizontal;
    private float vertical;
    float m_OrigGroundCheckDistance;

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
    
    public int actorID = 0;//추후 액터 ID값에 따라 데이터를 받아오게 할 것 

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    

    #region components
    public Animator anim;
    private Transform myTr;
    private float m_ForwardAmount;
    private float m_TurnAmount;


    private float m_CapsuleHeight;
    private Vector3 m_CapsuleCenter;
    private CapsuleCollider m_Capsule;
    private bool m_Crouching;
    private Rigidbody m_Rigidbody;
    private Vector2 mouseInput;
    private InputHandler playerInput;
    #endregion

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start()
    {
        myTr = transform;
        //m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

    }

    //command and States
    public void Idle()
    {
        m_Rigidbody.velocity = Vector3.zero;
    }

    public void Move(Vector3 move, bool jump)
    {
        if (move.magnitude > 1f)
            move.Normalize();

        
        move = transform.InverseTransformDirection(move);
        //CheckGroundStatus();
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);    //바닥에 투사 시키는

        //m_TurnAmount = Mathf.Atan2(move.x, move.z); //atan 값으로 
        //m_ForwardAmount = move.z;

        UpdateAnimator(move);
        m_Rigidbody.AddForce(move * moveSpeed);
    }
    public void Attack()
    {

    }
    public void Skill()
    { }

    public void Die()
    {
        anim.SetTrigger("IsDie");
    }

    public void OnDamaged()
    {
        anim.SetTrigger("IsHit");
    }


    void UpdateAnimator(Vector3 move)
    {
        anim.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
    }
}