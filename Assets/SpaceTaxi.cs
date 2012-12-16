using UnityEngine;
using System.Collections;

public class SpaceTaxi : MonoBehaviour {

    GameManager gm;

    GameObject player;
    public float angle;
    public float newangle;
    public Vector3 velocity;

    public bool isIndependant = false;
    float time = 0f;

    int health = 4;

	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!isIndependant)
            return;

        time += Time.deltaTime;
        
        transform.rotation = Quaternion.Slerp(Quaternion.Euler(new Vector3(0f, 180f, transform.rotation.eulerAngles.z)), Quaternion.Euler(new Vector3(0f, 180f, newangle)), Time.deltaTime/2f);
        Debug.Log(transform.rotation.eulerAngles.z);

        velocity += transform.rotation * (Vector3.up * Time.deltaTime * 10);

        if (velocity.magnitude > 6f)
        {
            if (time > 1f)
            {
                float min = Mathf.Clamp((time-2) * 3f, 0, 7);
                velocity = velocity.normalized * (11f - min);
            }
        }


        transform.position += velocity * Time.deltaTime;

        Debug.DrawLine(transform.position, transform.up.normalized * 6f + transform.position);
	}

    void OnTriggerEnter(Collider other)
    {

        PlayerLaser oth = other.gameObject.GetComponent<PlayerLaser>();
        if (oth != null)
        {
            GameObject.Destroy(other.gameObject);
            health -= 1;

            if (health <= 0)
            {
                GameObject.Instantiate(Resources.Load("explosions/deathsmall"), transform.position, transform.rotation);
                GameObject.Destroy(gameObject);
            }
        }
    }
}
