using UnityEngine;
using System.Collections;

public class PlayerLaser : MonoBehaviour {

    public float angle;
    public float velocity = 30;
    float life = 2;

	// Use this for initialization
	void Start () {
        //velocity = 30;
        if (velocity <= 0)
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

    static bool isShuttingDown = false;

    void OnApplicationQuit()
    {

        isShuttingDown = true;

    }

    void OnDestroy()
    {
        if(life > 0 && !isShuttingDown)
            GameObject.Instantiate(Resources.Load("explosions/Hitsplosion"), transform.position, transform.rotation);
    }
}
