using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : Character {

    #region Player Input values
    private float vertical;
    private float horizontal;
    private float rotX;
    private float rotY;

    private bool doAttack;
    private bool run = false;

    public int floorMask;

    //things for movement
    public float Vertical
    {
        get { return vertical; }
        set { vertical = value; }
    }
    public float Horizontal
    {
        get { return horizontal; }
        set { horizontal = value; }
    }

    //about Rotation
    public float RotateSpeed = 5.0f;
    public float RotateY
    {
        get { return rotX; }
        set { rotX = value; }
    }
    public float RotateX
    {
        get { return rotY; }
        set { rotY = value; }
    }
    private Rigidbody userRbody;

    public bool Attack
    {
        get { return doAttack; }
        set { doAttack = value; }
    }
    public bool GotHit
    {
        get { return isHit; }
        set { isHit = value; }
    }
    public bool Run
    {
        get { return run; }
        set { run = value; }
    }

    #endregion

    #region Player Attack Values
    //get bullet prefab
    //public GameObject Bullet;
    //make bullet pool
    //public static Queue<GameObject> BulletPool = null;

    //for long range attack
    public int attackRange_long = 7;
    public bool isCloseWeapon = true;
    public bool haveLongWeapon = false;
    public Transform FirePivot;

    //public int MaxBullet = 20;

    //Weapon Visual change
    public GameObject ShortWeapon;
//    public KnifeAttack knifeatt;
    public GameObject KnifeIcon;
    public GameObject GunIcon;
    public GameObject LongWeapon;
 //   private PlayerAttackEffect effect;

    public GameObject aimPoint;
    #endregion

    public Text HPtxt;

    void Awake()
    {
		//base.Awake ();
        floorMask = LayerMask.GetMask("FLOOR");
    }

    // Use this for initialization
    void Start ()
    {
        //ShortWeapon.GetComponent<BoxCollider>().enabled = false;
        state = new StateMgr[4] { idle, walk, attack, die };
        //LongWeapon.SetActive(!isCloseWeapon);
        //ShortWeapon.SetActive(isCloseWeapon);
        //KnifeIcon.SetActive(false);
        //GunIcon.SetActive(false);
        //aimPoint.SetActive(false);
//        knifeatt = ShortWeapon.GetComponent<KnifeAttack>();
        FullHP = HP;
        //HPtxt.text = (HP*10).ToString() + "/" + (FullHP*10).ToString();

        myTr = transform;
        userRbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        RotateY = myTr.eulerAngles.y;
        RotateX = myTr.eulerAngles.x;
        originColor = Color.white;
 //       effect = GetComponent<PlayerAttackEffect>();
    }

    // Update is called once per frame
    void Update()
	{
		//base.Update();
        state[(int)charState]();
        //damaged 
        if (isHit)
        {
            float lerp = Mathf.PingPong(Time.time, 0.1f) / 0.1f;
            rend.material.color = Color.Lerp(originColor, Color.red, lerp);
        }

        if (!isCloseWeapon)
        {
            Debug.DrawRay(FirePivot.position, myTr.forward * 10.0f, Color.blue);
            aimPoint.transform.position = FirePivot.position + myTr.forward * 10.0f;
        }

    }

    protected override void walk()
    {
        base.walk();

        //change speed with runkey
        int speed;
        if (run)
            speed = runSpeed;
        
        else
            speed = moveSpeed;

        Vector3 dir = horizontal * Vector3.right + vertical * Vector3.forward;
        myTr.position += (dir.normalized * speed* Time.deltaTime);
        
    }

    public void Rotate(Vector3 hitPoint)
    {
        Vector3 PlayerToMouse = hitPoint - myTr.position;
        PlayerToMouse.y = 0f;

        Quaternion newRotation = Quaternion.LookRotation(PlayerToMouse);
        userRbody.MoveRotation(newRotation);
    }

    #region 공격
    protected override void attack()
    {
        //if Player do long range attack
        if (!isCloseWeapon)
        {
            Shooting();
            return;
        }
        anim.SetTrigger("CloseAttack");
        base.attack();
        
    }
    public void StartAttack(int AttackID)
    {
        //ATTACkID를 통해 공격 스태미너 코스트를 DB에서 불러옴.
        //스태미너 시스템은 일단 추후구현
    }
    public void DoingAttack()
    {
        Attack = true;
    }
    public void EndAttack()
    {
        Attack = false;
    }

    protected void Shooting()
    {
        
        FirePivot.forward = myTr.forward;
        //effect.GunShootStart();
        //effect.GunShootEffect.transform.position = FirePivot.position;
        //effect.GunShootEffect.transform.forward = FirePivot.forward;
        //effect.GunShootEffect.SetActive(true);
        StartCoroutine(ShowShootEffect());
        RaycastHit hit;

        anim.SetBool("IsAttack", true);
        
        if (Physics.Raycast(FirePivot.position, myTr.forward, out hit, 10.0f, 1 << 11 ))
        {
            Debug.Log("hitOBJ : " + hit.collider.gameObject);
            //hit.collider.transform.GetComponent<Enemy>().OnDamaged(1);
        }
        else if (Physics.Raycast(FirePivot.position, myTr.forward, out hit, 10.0f, 1 << 9))
        {
            Debug.Log("hitOBJ : " + hit.collider.gameObject);

        }
        else
        {
            //TODO : 벽에 맞은 이펙트 등
        }
            
    }
    IEnumerator ShowShootEffect()
    {
        yield return new WaitForSeconds(1.0f);
        //effect.GunShootEnd();
    }

    #endregion

    #region Damage
    public override void OnDamaged(int damage)
    {
        base.OnDamaged(damage);

        HPtxt.text = (HP * 10).ToString() + "/" + (FullHP * 10).ToString();
    }
    public void GiveDamage()
    {
        ShortWeapon.GetComponent<BoxCollider>().enabled = true;
  
        {
            //knifeatt.enemy.GetComponent<Character>().OnDamaged(1);
            //knifeatt.hit = false;
        }
    }
    public void EndDamage()
    {
        ShortWeapon.GetComponent<BoxCollider>().enabled = false;
    }
    #endregion
    protected override void die()
    {
        base.die();
        this.GetComponent<PlayerInput>().enabled = false;  
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<CapsuleCollider>().enabled = false;
        this.enabled = false;

 //       GameManager.instance.gmState = GameManager.GameState.gameOver;
    }
   
}
