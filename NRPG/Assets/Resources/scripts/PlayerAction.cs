using UnityEngine;
using System.Collections;

public class PlayerAction : MonoBehaviour {
    public enum STATESKILL  // 스킬종류
    {
        NO,        
        baseattack,
        skill1,
        skill2,
        skill3,
    };
    public STATESKILL stateskill = STATESKILL.NO;

    public enum STATE  // 캐릭터의 상태
    {
        Idle,
        Move,
        Attack,
        die,
    };
    public STATE state = STATE.Idle;
    
    //캐릭터 속도
    public int speed;

    //캐릭터 체력
    public int PlayerHP = 100;
    int PlayerMaxHP = 0;

    //캐릭터 공격력
    int PlayerATT = 10;
    
    //투사체생성지점
    public GameObject shootpoint;

    //애니메이터
    Animator ani;

	// Use this for initialization
	void Start () {
        PlayerMaxHP = PlayerHP;
        ani = GetComponentInChildren<Animator>();
        StartCoroutine(SkillMain());	
	}
   
	// Update is called once per frame
	void Update () {
        
        if(Input.GetKey(KeyCode.RightArrow))
        {
            state = STATE.Move;
            MoveRight();
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            state = STATE.Move;
            MoveLeft();
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            state = STATE.Move;
            Moveup();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            state = STATE.Move;
            MoveDown();
        }
        else
        {
            state = STATE.Idle;
        }

        
        switch (state)
        {
            case STATE.Idle:
                ani.SetBool("w", false);
                break;
            case STATE.Move:
                ani.SetBool("w", true);
                break;
        }
    }   

    void MoveLeft()
    {        
        if(transform.position.x <= -9)
        {
            transform.position = new Vector3(-9, transform.position.y, transform.position.z);
        }
        transform.Translate(new Vector3(-speed*Time.deltaTime, 0f, 0f));
    }
    void MoveRight()
    {
        if (transform.position.x >= 165)
        {
            transform.position = new Vector3(165, transform.position.y, transform.position.z);
        }        
        transform.Translate(new Vector3(speed * Time.deltaTime, 0f, 0f));
        
    }
    void Moveup()
    {
        if (transform.position.z >= 12)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 12);
        }
        transform.Translate(new Vector3(0f, 0f, speed * Time.deltaTime));
    }
    void MoveDown()
    {
        if (transform.position.z <= -6)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -6);
        }
        transform.Translate(new Vector3(0f, 0f, -speed * Time.deltaTime));
    }    
    IEnumerator SkillMain()
    {
        while(stateskill == STATESKILL.NO)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                ani.SetTrigger("mbaseattack");
                stateskill = STATESKILL.baseattack;
                StartCoroutine(BaseSkill());
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                ani.SetTrigger("attack2");
                stateskill = STATESKILL.skill1;
                StartCoroutine(Skill1());
            }            
            yield return null;
        }            
    }
    IEnumerator BaseSkill()
    {
        yield return new WaitForSeconds(0.45f);
        GameObject Mbase = (GameObject)Resources.Load("prefabs/baseAttack");
        if (Mbase != null)
        {
            GameObject Mbaseattack = (GameObject)Instantiate(Mbase, shootpoint.transform.position+ new Vector3(1,0,0), Quaternion.LookRotation(Vector3.right)); // 직진
            Mbaseattack.GetComponent<MbaseAttack>().setskill(PlayerATT);
        }
        stateskill = STATESKILL.NO;
        StartCoroutine(SkillMain());
        yield return null;
    }    
    IEnumerator Skill1()
    {
        yield return new WaitForSeconds(0.45f);
        GameObject Mbase = (GameObject)Resources.Load("prefabs/icerain");
        if (Mbase != null)
        {
            GameObject Mbaseattack = (GameObject)Instantiate(Mbase, shootpoint.transform.position, Quaternion.LookRotation(Vector3.right)); // 직진               
        }
        stateskill = STATESKILL.NO;
        yield return new WaitForSeconds(5f);
        StartCoroutine(SkillMain());
        yield return null;
    }
    void OnTriggerEnter(Collider col)
    {
        if(col.transform.tag == "monster")
        {
            PlayerHP -= col.GetComponentInParent<MobAction>().mobATT;
            Debug.Log("몹에게 맞았다");
        }
    }
}
