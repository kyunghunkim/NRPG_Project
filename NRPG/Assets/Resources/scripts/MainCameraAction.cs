using UnityEngine;
using System.Collections;

public class MainCameraAction : MonoBehaviour {
    public GameObject player = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(player.transform.position.x + 5f,10f,-43f);
	}
}
