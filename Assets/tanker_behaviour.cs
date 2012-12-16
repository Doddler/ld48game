using UnityEngine;
using System.Collections;

public class tanker_behaviour : MonoBehaviour {

    GameObject player;
    public float angle;
    public Vector3 velocity;

    float speedvar;
    float turnvar;
    float aimvar;

	private int health;
    private bool spotted; // has it spotted us
    private float spottedTimer;
    private float keepAliveTimer = 100f;

    private Vector3 target;
    private Vector3 forward;
    public float turnCount;
    // Use this for initialization
	void Start () {

        player = PlayerController.getPlayer();

        speedvar = Random.Range(0.4f, 0.6f);
        turnvar = Random.Range(1.5f, 3.5f);
        aimvar = Random.Range(0f, 2f);

        transform.position = new Vector3(transform.position.x, transform.position.y, 1.74f);

        health = 100;
	
        //initial random direction
        var initRot = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0, 180, initRot);

    }
	
	// Update is called once per frame
	void Update () {
               
        if (Mathf.Abs(player.transform.position.magnitude - transform.position.magnitude) < 50)
        {
            spotted = true;
            spottedTimer = 10f;
            keepAliveTimer = 100f;
        }
        else
        {
            if (spottedTimer < 0)
            {
                if (spotted)
                {
                    Debug.Log("not spotted anymore");
                    spotted = false;

                }
            }
            spottedTimer -= Time.deltaTime;
        }
         
        //if we haven't spotted the player just travel forwards in a random direction
        if (!spotted)
        {
            velocity += transform.rotation * (Vector3.up * speedvar * Time.deltaTime);
            if (velocity.magnitude > 8)
                velocity = velocity.normalized * 8;
            transform.position += velocity * Time.deltaTime;
            keepAliveTimer -= Time.deltaTime;
        }
        //when we spot it, try and run away
        else
        {

            turnCount -= Time.deltaTime;
            if (turnCount < 0)
            {
                turnCount = Random.RandomRange(5f, 20f);
                target = player.transform.position - transform.position;
                forward = transform.up;
            }

            angle = Vector3.Angle(target, forward);

            if (target.x > transform.position.x)
                angle *= -1;

            //var n = (target.normalized) - transform.position;
            var newRotation = Quaternion.LookRotation(target, Vector3.back) * Quaternion.Euler(270, 0, 0);
          
            newRotation = Quaternion.Euler(new Vector3(0, 180, newRotation.eulerAngles.z));
            
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, (5 - turnCount) / 800); 
            //transform.rotation = newRotation;
            velocity += transform.rotation * (Vector3.up * speedvar * Time.deltaTime);


            if (velocity.magnitude > 8)
                velocity = velocity.normalized * 8;

            transform.position = new Vector3(transform.position.x, transform.position.y, 1.74f); //zlock
            if (target.magnitude > 5)
                velocity += transform.rotation * (Vector3.up * speedvar * Time.deltaTime * 12);

            if (velocity.magnitude > 12 * speedvar)
                velocity = velocity.normalized * 12 * speedvar;

            transform.position += velocity * Time.deltaTime;
        
        }

        if (keepAliveTimer < 0)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
    
     
    
    }
}
