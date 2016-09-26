using UnityEngine;
using System.Collections;

public class MbaseAttack : MonoBehaviour {
    public int damege = 0;
    public float lifetime = 3;
    public float speed = 5;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, lifetime);	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.forward*Time.deltaTime*speed);
	
	}
    void OnTriggerEnter(Collider col)
    {
        if(col.transform.tag == "monster")
        {
            Destroy(gameObject);            
        }
    }
    public void setskill(int dam)
    {
        damege = dam;
    }
}
