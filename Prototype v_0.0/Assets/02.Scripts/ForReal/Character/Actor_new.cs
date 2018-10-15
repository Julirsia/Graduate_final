﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 목적 : 게임 캐릭터의 베이스가되는 액터클래스.
 * 역할 : 명령 객체의 지시를 받는 리시버.
 */
public class Actor_new : Photon.PunBehaviour
{
    [SerializeField] float m_MovingTurnSpeed = 360;
    [SerializeField] float m_StationaryTurnSpeed = 180;
    [SerializeField] float m_RunCycleLegOffset = 0.2f;

    [Range(1f, 4f)] [SerializeField] float m_GravityMultiplier = 2f;
    const float k_Half = 0.5f;
    public int m_PlayerNumber = 1;
    bool m_IsGrounded;
    [SerializeField] float m_AnimSpeedMultiplier = 1f;
    Vector3 m_GroundNormal;
    [SerializeField] float m_GroundCheckDistance = 10f;
    [SerializeField] float m_MoveSpeedMultiplier = 1f;

    [SerializeField] float m_JumpPower = 12f;

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


    float m_CapsuleHeight;
    Vector3 m_CapsuleCenter;
    CapsuleCollider m_Capsule;
    bool m_Crouching;
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

    void LateUpdate()
    {
        /*
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * 0 ;

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;
        */
    }

    public void Idle()
    {
        m_Rigidbody.velocity = Vector3.zero;
    }
    public void Move(Vector3 move, bool jump)
    {

        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        CheckGroundStatus();
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        m_ForwardAmount = move.z;

        ApplyExtraTurnRotation();

        // control and velocity handling is different when grounded and airborne:
        if (m_IsGrounded)
        {
            HandleGroundedMovement(jump);
        }
        else
        {
            HandleAirborneMovement();
        }

        //ScaleCapsuleForCrouching(crouch);
        //	PreventStandingInLowHeadroom();

        // send input and other state parameters to the animator
        UpdateAnimator(move);
    }


    /*  public void Move(Vector3 move)
    {
        if (move.magnitude > 1f)
            move.Normalize();
        move = transform.InverseTransformDirection(move);

        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        //m_TurnAmount = Mathf.Atan2(move.x, move.z);
       m_TurnAmount = Mathf.Atan2(yaw, move.z);
        m_ForwardAmount = move.z;
        ApplyExtraTurnRotation();

        anim.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        anim.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        anim.SetBool("OnGround", true);

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

    }*/


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



    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        anim.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        anim.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        anim.SetBool("Crouch", m_Crouching);
        anim.SetBool("OnGround", m_IsGrounded);
        /* if (!m_IsGrounded)
         {
             anim.SetFloat("Jump", m_Rigidbody.velocity.y);
         }*/

        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        float runCycle =
            Mathf.Repeat(
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
        float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
        if (m_IsGrounded)
        {
            anim.SetFloat("JumpLeg", jumpLeg);
        }

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (m_IsGrounded && move.magnitude > 0)
        {
            anim.speed = m_AnimSpeedMultiplier;
        }
        else
        {
            // don't use that while airborne
            anim.speed = 1;
        }
    }


    void HandleAirborneMovement()
    {
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
        m_Rigidbody.AddForce(extraGravityForce);

        m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
    }


    void HandleGroundedMovement(bool jump)
    {
        // check whether conditions are right to allow a jump:
        if (jump && anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            // jump!
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
            m_IsGrounded = false;
            anim.applyRootMotion = false;
            m_GroundCheckDistance = 0.1f;
        }
    }

    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }


    public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        if (m_IsGrounded && Time.deltaTime > 0)
        {
            Vector3 v = (anim.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = v;
        }
    }


    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
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
/* 
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
    }*/
