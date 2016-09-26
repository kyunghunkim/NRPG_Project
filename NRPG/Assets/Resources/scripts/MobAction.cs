using UnityEngine;
using System.Collections;

public class MobAction : MonoBehaviour {
    //몹의상태
    public enum STATE
    {
        reset,
        idle,
        move,
        attack,
        die,
    };
    STATE state = STATE.idle;
    
    GameObject player = null;
    //이동속도
    public float speed = 3;
    //체력
    public int MobHP = 30;
    int MobMAxHP;
    //공격력
    public int mobATT = 5;
    //리젠장소
    Vector3 oripos = Vector3.zero;
    //애니메이터
    Animator ani;
    //공격타이머
    float attacktimer = 2f;
    bool timercheck = true;

    // Use this for initialization
    void Start () {
        MobMAxHP = MobHP;
        ani = GetComponentInChildren<Animator>();
        player = GameObject.Find("Player");
        oripos = transform.position- new Vector3(1,0,0);
	}
	
	// Update is called once per frame
	void Update () {
        if (timercheck == false)
        {
            attacktimer -= Time.deltaTime;
            if(attacktimer <= 0)
            {
                timercheck = true;
                attacktimer = 2f;
            }
        }  
        switch (state)
        {
            case STATE.reset:                
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
                if (Vector3.Distance(oripos, transform.position) <1)
                {
                    ani.SetBool("walk", false);
                    state = STATE.idle;
                }
                break;            
            case STATE.idle:
                if (Vector3.Distance(player.transform.position, transform.position) < 10)
                {                   
                    state = STATE.move;                    
                }
                break;
            case STATE.move:
                ani.SetBool("walk", true);
                Vector3 a = player.transform.position - transform.position;
                transform.rotation = Quaternion.LookRotation(a);
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
                if (Vector3.Distance(player.transform.position, transform.position) < 1.5)
                {
                    ani.SetBool("walk", false);
                    state = STATE.attack;
                }
                if (Vector3.Distance(player.transform.position, transform.position) > 10)
                {
                    state = STATE.reset;
                    Vector3 b = oripos - transform.position;
                    transform.rotation = Quaternion.LookRotation(b);
                }
                break;
            case STATE.attack:
                if (timercheck == true)
                {
                    ani.SetTrigger("attack");
                    //ani.SetBool("at", true);
                    timercheck = false;                    
                }
                if (Vector3.Distance(player.transform.position, transform.position) > 1.5)
                {
                    ani.SetBool("at", false);
                    state = STATE.move;
                }
                break;
            case STATE.die:
                Destroy(this.gameObject.GetComponent<Collider>());
                Destroy(gameObject, 0.9f);
                break;
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if(col.transform.tag == "playershot")
        {
            ani.SetTrigger("hit");
            MobHP -= col.GetComponent<MbaseAttack>().damege;
            if (MobHP <= 0)
            {
                ani.SetTrigger("death");
                state = STATE.die;
            }
        }
    }
    
}
