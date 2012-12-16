using UnityEngine;
using System.Collections;

public class BadDuder : MonoBehaviour {

    GameManager gm;

    GameObject player;
    public float angle;
    public Vector3 velocity;

    float speedvar;
    float turnvar;
    float aimvar;
    int angryvar;

    float health = 10;

    GameObject sirenred;
    GameObject sirenblue;
    public GameObject bullet;
    public GameObject deathsplosion;
    public GameObject hitsplosion;

    float sirentimer = 1f;
    bool sirenstate = false;

    public float firedelay = 0.1f;
    float fdelay = 0;
    bool fire2 = false;

    bool isdying = false;
    float dyingtime = 3f;

    GameObject explosion;
    GameObject explosion2;
    GameObject explosion3;

    float firetime;
    float firecooldown;

	// Use this for initialization
	void Start () 
    {
        player = PlayerController.getPlayer();

        gm = GameManager.getGameManager();

        angryvar = Random.Range(0, 15);
        if (angryvar > 5)
            angryvar = 1;
        else if (angryvar > 1)
            angryvar = 3;
        else
            angryvar = 10;

        int superangry = Random.Range(0, 100);
        if (superangry > 98 && gm.nefarious > 180)
            angryvar = 20;

        speedvar = Random.Range(0.8f - (float)angryvar * 0.02f, 1.2f - (float)angryvar * 0.02f);
        turnvar = Random.Range(1.5f - (float)angryvar * 0.1f, 3.5f - (float)angryvar * 0.2f);
        aimvar = Random.Range(0f, 5f);

        if (angryvar == 1)
        {
            speedvar *= 2;
            turnvar *= 2;
        }


        if (turnvar < 0.4f)
            turnvar = 0.4f;
        if (speedvar < 0.1f)
            speedvar = 0.1f;

        sirenred = transform.FindChild("SirenRed").gameObject;
        sirenblue = transform.FindChild("SirenBlue").gameObject;



        transform.position = new Vector3(transform.position.x, transform.position.y, 1.74f);

        transform.localScale = new Vector3(transform.localScale.x + 0.25f * (float)angryvar, transform.localScale.y + -0.4f * (float)angryvar, transform.localScale.z * 0.25f * (float)angryvar);

        transform.rotation = Quaternion.Euler(0, 180f, Random.Range(0f, 360f));

        health = angryvar * 2;
    }

    void OnShotDead()
    {
        PlayerController p = player.GetComponent<PlayerController>();

        switch (angryvar)
        {
            case 1: GameManager.getGameManager().nefarious += 1; p.shields += 0; break;
            case 3: GameManager.getGameManager().nefarious += 2; p.shields += 2; break;
            case 10: GameManager.getGameManager().nefarious += 6; p.shields += 5; break;
            case 20: GameManager.getGameManager().nefarious += 18; p.shields += 10; break;
            default:
                GameManager.getGameManager().nefarious += 1; break;
        }

        GameManager.getGameManager().changeShields(p.shields);
    }
    
	// Update is called once per frame
	void Update () 
    {
        if (isdying)
        {
            dyingtime -= Time.deltaTime;
            if (dyingtime <= 0)
            {
                OnShotDead();
                GameObject.Destroy(gameObject);
            }
            explosion.transform.position = this.transform.position;

            if (angryvar >= 10)
            {
                explosion2.transform.position = this.transform.position + new Vector3(0.5f, 0.2f, 0f);
                explosion3.transform.position = this.transform.position + new Vector3(-0.1f, -0.3f, 0f);
            }
        }

        if((player.transform.position - transform.position).magnitude > 120f)
            GameObject.Destroy(this.gameObject);

        fdelay -= Time.deltaTime * Random.Range(0.8f * (float)angryvar, 1.2f * (float)angryvar);

        if (sirenstate)
        {
            sirenred.renderer.enabled = false;
            sirenblue.renderer.enabled = true;
        }
        else
        {
            sirenred.renderer.enabled = true;
            sirenblue.renderer.enabled = false;
        }

        sirentimer -= Time.deltaTime;
        if (sirentimer < 0f)
        {
            sirentimer = 0.2f;
            sirenstate = !sirenstate;
        }

        Vector3 target = player.transform.position - transform.position;
        Vector3 forward = transform.up;
        
        angle = Vector3.Angle(target, forward);

        if (player.transform.position.x > transform.position.x)
            angle *= -1;

        Debug.DrawLine(transform.position, (player.GetComponent<PlayerController>().velocity.normalized * aimvar + player.transform.position));

        var n = (player.GetComponent<PlayerController>().velocity.normalized * aimvar + player.transform.position) - transform.position;
        var newRotation = Quaternion.LookRotation(n, Vector3.back) * Quaternion.Euler(270, 0, 0);

        newRotation = Quaternion.Euler(new Vector3(0, 180, newRotation.eulerAngles.z));
        

        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * turnvar);
        
        transform.position = new Vector3(transform.position.x, transform.position.y, 1.74f);
        if (target.magnitude > 5)
            velocity += transform.rotation * (Vector3.up * speedvar * Time.deltaTime * 12);

        if (velocity.magnitude > 12 * speedvar)
            velocity = velocity.normalized * 12 * speedvar;

        float angle2 = Mathf.Atan2(target.y, target.x) * 180 / Mathf.PI;
        angle2 -= 90;
        if (angle2 < -180)
            angle2 += 360;

        transform.position -= velocity * Time.deltaTime;

        float curangle = 180 - transform.rotation.eulerAngles.z;

        //Debug.Log(curangle + " " + angle2);

        firetime += Time.deltaTime;

        if (firetime > 5f)
        {
            firecooldown = 5f;
            Debug.Log("Police ship on firecooldown.");
        }

        if (firecooldown > 0)
        {
            firetime = 0f;
            firecooldown -= Time.deltaTime;
            
        }


        if (fdelay <= 0 && (player.transform.position - transform.position).magnitude < 35f && firecooldown <= 0f)
        {
            float deltaangle = Mathf.DeltaAngle(curangle, angle2);

            if (deltaangle < 30 + (float)angryvar * 5 && deltaangle > -50 - (float)angryvar * 5)
            {
                Vector3 bulletspawn = Quaternion.Euler(new Vector3(0, 0, angle)) * transform.up.normalized * 1 + transform.position;

                fdelay = firedelay;

                float randvar = Random.Range(-20f - (float)angryvar * 3, 20f + (float)angryvar * 3);

                GameObject.Instantiate(bullet, bulletspawn, Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z + 180 + randvar));

                if (angryvar > 10)
                {
                    randvar = Random.Range(-20f - (float)angryvar * 3, 20f + (float)angryvar * 3);

                    GameObject.Instantiate(bullet, bulletspawn, Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z + 180 + randvar));

                }

                fire2 = !fire2;
            }
            else
                firetime = 0f;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        
        PlayerLaser oth = other.gameObject.GetComponent<PlayerLaser>();
        if ( oth != null)
        {
            GameObject.Destroy(oth.gameObject);

            if (isdying == true)
                return;

            
            health -= 1;
            if (health <= 0)
            {
                int longsplosion = Random.Range(angryvar, 20);

                if (longsplosion < 15)
                {
                    OnShotDead();                    
                    GameObject.Instantiate(Resources.Load("explosions/deathsmall"), transform.position, transform.rotation);
                    GameObject.Destroy(this.gameObject);
                    return;
                }

                explosion = (GameObject)GameObject.Instantiate(deathsplosion, transform.position, transform.rotation);

                if (angryvar >= 10)
                {
                    explosion2 = (GameObject)GameObject.Instantiate(deathsplosion, transform.position + new Vector3(0.5f, 0.2f, 0f), transform.rotation);
                    explosion3 = (GameObject)GameObject.Instantiate(deathsplosion, transform.position + new Vector3(-0.1f, -0.3f, 0f), transform.rotation);
                }
                isdying = true;              
            }
        }

        
    }
}


