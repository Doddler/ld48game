using UnityEngine;
using System.Collections;

public class PlayerLaser : MonoBehaviour {

    public float angle;
    public float velocity;
    float life = 3;

	// Use this for initialization
	void Start () {
        velocity = 30;
	}
	
	// Update is called once per frame
	void Update () {
        life -= Time.deltaTime;
        if (life <= 0)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }

        transform.position += transform.up * velocity * Time.deltaTime;
	}
}
