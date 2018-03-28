using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace Com.MyCompany.MyGame
{
    public class Character : Photon.PunBehaviour
    {
        protected Transform myTr;

        //variables about character
        public int HP = 0;
        public int FullHP = 0;
        public int moveSpeed = 0;
        public int runSpeed = 0;
        public float attackRange_short = 0;
        public int attackPower = 1;
        protected GameObject Target;

        //use this for change character state
        public enum State { idle = 0, walk, attack, die, wait };
        public State charState = State.idle;

        public delegate void StateMgr();
        protected StateMgr[] state;

        public Animator anim;
        public Image hpBar;

        //about damaged effect
        protected bool isHit = false;
        public Color originColor;
        public Renderer rend;

        /*public void Awake() {
            updateInfo = new SystemManager.UpdateInfo ("test_id", 0, transform);

            SystemManager.Instance.StartCorutine (updateInfo);
        }*/



        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            
        }

        public void Update()
        {

        }

        public virtual void OnDamaged(int damage)
        {
            HP -= damage;
            if (HP <= 0)
            {
                anim.SetTrigger("IsDie");
                charState = State.die;
            }
            //StartCoroutine(PlayOneShot("IsHit"));
            anim.SetTrigger("IsHit");
            StartCoroutine(ShowDamage());
            hpBar.fillAmount = (float)HP / (float)FullHP;
        }

        protected IEnumerator ShowDamage()
        {
            isHit = true; //make is hit True.
            yield return new WaitForSeconds(0.3f);//go to update to make player red
            isHit = false;// stop showing red
            rend.material.color = originColor;// return to original color
        }

        protected virtual void idle()
        {
            anim.SetBool("IsWalk", false);
            anim.SetBool("IsAttack", false);
        }

        protected virtual void walk()
        {
            anim.SetBool("IsWalk", true);
            anim.SetBool("IsAttack", false);
        }

        //actions when target is in attack range
        protected virtual void attack()
        {
            anim.SetBool("IsAttack", true);
        }

        //actions when character die
        protected virtual void die()
        {
            anim.SetBool("IsWalk", false);
            anim.SetBool("IsAttack", false);
            anim.SetTrigger("IsDie");
        }

        public IEnumerator PlayOneShot(string paramName)
        {
            anim.SetBool(paramName, true);
            yield return new WaitForSeconds(0.1f);
            anim.SetBool(paramName, false);
        }
    }
}