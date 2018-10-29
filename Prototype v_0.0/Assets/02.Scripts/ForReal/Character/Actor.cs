using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using clientData;

/* 목적 : 게임 캐릭터의 베이스가되는 액터클래스.
 * 역할 : 명령 객체의 지시를 받는 리시버.
 */
public class Actor : Photon.PunBehaviour
{
    public ActorType type;
    public bool deathSignal = false;

    [SerializeField] float m_MovingTurnSpeed = 360;
    [SerializeField] float m_StationaryTurnSpeed = 180;
    [SerializeField] float m_RunCycleLegOffset = 0.2f;
    const float k_Half = 0.5f;
    private  float m_AnimSpeedMultiplier = 1f;
    private float m_MoveSpeedMultiplier = 1f;
    private float m_GroundCheckDistance = 0.1f;
    public float speed;

    private bool isGrounded;
    private Vector3 m_GroundNormal;

    private bool isCrouching;
    //캡슐콜라이더의 오리지널 값
    private float m_CapsuleHeight;
    private Vector3 m_CapsuleCenter;
    private float crouchCapsuleHeight;
    private Vector3 crouchCapsuleCenter;
    private CapsuleCollider m_capsule;

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

    public int fullHp = 100;
    public int currentHp;

    public float moveSpeed;
    public float crouchSpeed;
    public float jumpSpeed;
    #endregion


    #region components
    public Animator anim;
    private Transform myTr;
    [SerializeField] float m_ForwardAmount;
    [SerializeField] float m_TurnAmount;
    Rigidbody m_Rigidbody;
    #endregion


    #region Attack values
    private bool isHit = false;
    public int currentWeaponCode = 0;   //0 : 맨손, 1 :나이프,  2 :리볼버,  3 :다이너마이트
    private int pressedAttackType = 0;
    public Transform bareFist;
    public Transform currentWeapon;
    public Collider curWeaponCol;

    public Color originColor;
    public Renderer rend;
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
        currentHp = fullHp;
        OnWeaponChanged(currentWeaponCode);

        m_capsule = GetComponent<CapsuleCollider>();
        m_CapsuleCenter = m_capsule.center;
        m_CapsuleHeight = m_capsule.height;
        crouchCapsuleCenter = new Vector3(0f, 0.695f, 0f);
        crouchCapsuleHeight = 1.39f;

        speed = moveSpeed;
        UIManager.instance.hpChange(fullHp, currentHp, actorID, type);
    }

    #region 캐릭터 상태제어

    public void Idle( bool crouch)
    {
        isCrouching = crouch;

        m_Rigidbody.velocity = Vector3.zero;

        m_ForwardAmount = 0f;
        m_TurnAmount = 0f;

        UpdateMoveAnimator();
    }

    /* 함수명 : Move
     * 목적 : 캐릭터의 움직임을 담당하는 메서드
     * 리턴값 : null
     * 파라미터 : move - input에서 받은 캐릭터 이동값
     */
    public void Move(Vector3 move, bool jump, bool crouch)
    {
        isCrouching = crouch;
       
        
        
        if (move.magnitude > 1f)
            move.Normalize();

        if (type == ActorType.NPC)
            myTr.position += move.normalized * speed * Time.deltaTime;
        else
           if (m_Rigidbody.velocity.magnitude < speed)
                m_Rigidbody.AddForce(move * speed);

        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);

        m_TurnAmount = Mathf.Atan2(move.x, move.z);     //바닥에서 유저의 회전값 a = atan move x/movez
        m_ForwardAmount = move.z;
        ApplyExtraTurnRotation();

        UpdateMoveAnimator();

        float runCycle =
                Mathf.Repeat(
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
        float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;

        if (isGrounded)
        {
            anim.SetFloat("JumpLeg", jumpLeg);
        }
        else
        {

        }
        
    }
    public void Attack(int attackType)
    {
        pressedAttackType = attackType;

        anim.SetInteger("WeaponType", currentWeaponCode);
        anim.SetInteger("AttackType", pressedAttackType);
        anim.SetTrigger("Attack");
    }
    public void LockOn(Transform target)
    {
        myTr.LookAt(target);
    }
    public void Die()
    {
        anim.SetTrigger("Dead");
    }
    #endregion

    #region 캐릭터 움직임 관련

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
    private void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR

        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif

        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            m_GroundNormal = hitInfo.normal;
            isGrounded = true;
            anim.applyRootMotion = true;
        }
        else
        {
            isGrounded = false;
            m_GroundNormal = Vector3.up;
            anim.applyRootMotion = false;
        }
    }


    /*애니메이션 전환시 호출부분*/
    //크라우칭 상태에서 콜라이더와 속도를 바꿔준다.
    public void SetCrouchState()
    {
        speed = crouchSpeed;
        m_capsule.center = crouchCapsuleCenter;
        m_capsule.height = crouchCapsuleHeight;
    }
    //크라우칭 상태가 끝나고 콜라이더와 속도를 돌려놓는다.
    public void SetNormalState()
    {
        speed = moveSpeed;
        m_capsule.height = m_CapsuleHeight;
        m_capsule.center = m_CapsuleCenter;
    }


    #endregion

    #region 캐릭터 애니메이터 제어

    /* 함수명 : OnAnimatorMore() //콜백함수
     * 목적 : 애니메이터가 동작할때 콜되는 함수
     * 리턴 : void
     */
    public void OnAnimatorMove()
    {
        Vector3 v = (anim.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;
        //v.y = speed;

        if (isHit)
            TurnRed();
    }
    /* 함수명 : Update Animator 
     * 목적 : 애니메이터를 제어하는 함수.
     * 리턴 : void
     */
    private void UpdateMoveAnimator()
    {
        anim.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        anim.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        anim.SetBool("Crouch", isCrouching);
        //anim.SetBool("OnGround", isGrounded);

        if (isGrounded)
        {
            if (m_ForwardAmount > 0)
            {
                anim.speed = m_AnimSpeedMultiplier;
            }
            else if (m_Rigidbody.velocity.magnitude <  speed - 1)
            {
                anim.speed = 0.8f;
            }
            else if (m_Rigidbody.velocity.magnitude >= speed - 1)
            {
                anim.speed = 0.9f;
            }
            else
            {
                anim.speed = 1.1f;
            }
        }
    }

    #endregion

    #region 캐릭터 전투
    public void CloseCombatActive()
    {
        curWeaponCol.enabled = true;
    }

    public void CloseCombatEnd()
    {
        curWeaponCol.enabled = false;
    }
    
    public void OnDamaged(int damage)
    {
        if (currentHp > 0)
        {
            currentHp -= damage;
            if (currentHp <= 0)
            {
                //dieAction
            }
            anim.SetTrigger("IsHit");
            StartCoroutine(ShowDamage());

            if (currentHp <= 0)
            {
                deathSignal = true;
                if (type == ActorType.NPC || type == ActorType.player)
                    UIManager.instance.ActiveGameOverPanel();
                else if(type == ActorType.boss)
                    UIManager.instance.ActiveVictoryPanel();
                Die();
            }
            UIManager.instance.hpChange(fullHp, currentHp, actorID, type);
        }
    }

    public void OnWeaponChanged(int changeWeaponCode)
    {
        //획득한 무기를 손에 장착
        if (changeWeaponCode == 0)
            currentWeapon = bareFist;
        currentWeaponCode = changeWeaponCode;
        curWeaponCol = currentWeapon.GetComponent<Collider>();
        curWeaponCol.enabled = false;
    }

    public void TurnRed()
    {
        float lerp = Mathf.PingPong(Time.time, 0.1f) / 0.1f;
        rend.material.color = Color.Lerp(originColor, Color.red, lerp);
    }
    public IEnumerator ShowDamage()
    {
        isHit = true; //make is hit True.
        yield return new WaitForSeconds(0.3f);//go to update to make player red
        isHit = false;// stop showing red
        rend.material.color = originColor;// return to original color
    }

    public void CharacterStopEvent()
    {
        speed = 0f;
    }
    public void CharacterMoveEvent()
    {
        speed = moveSpeed;
    }
    #endregion
}