using UnityEngine;
using System.Collections;

public class SelfSuicide : MonoBehaviour {

    public float life = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        life -= Time.deltaTime;
        if (life < 0)
            GameObject.Destroy(gameObject);
	}
}
