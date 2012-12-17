using UnityEngine;
using System.Collections;

public class SpaceSwatController : MonoBehaviour 
{
    int health = 40;
    float maxspeed;
    float turnspeed;

    Vector3 velocity;
    float targetangle;

    Vector3 target;

    float turnwait = 0f;

    GameObject player;
    Vector3[] guns;

    float shotdelay = 3f;

    bool isdying = false;
    float dyingtime = 3f;

    GameManager gm;

    GameObject[] explosions;

	// Use this for initialization
	void Start () 
    {
        maxspeed = 3;
        turnspeed = 0.3f;

        player = PlayerController.getPlayer();

        velocity = Vector3.zero;

        guns = new Vector3[4];
        explosions = new GameObject[5];

        transform.position = new Vector3(transform.position.x, transform.position.y, 1.74f);

        gm = GameManager.getGameManager();
	}

    Vector3 GetGunPos(int gunpoint)
    {
        GameObject t = transform.GetChild(gunpoint).gameObject;

        return new Vector3(t.transform.position.x, t.transform.position.y, 1.74f);
    }

    float AngleToPlayer()
    {
        target = player.transform.position - transform.position;

        float angle2 = Mathf.Atan2(target.y, target.x) * 180 / Mathf.PI;
        angle2 -= 90;
        if (angle2 < -180)
            angle2 += 360;

        return -angle2;
    }

    IEnumerator Shotgun()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 point = GetGunPos(i);
            float angle = AngleToPlayer() + Random.Range(-3f, 3f);

            for (int s = 0; s < 20; s++)
            {
                

                float angle2 = Random.Range(-4f, 4f);        

                GameObject g = (GameObject)GameObject.Instantiate(Resources.Load("shots/PoliceShot"), point, Quaternion.Euler(0, 180f, angle + angle2));
                EnemyLaser p = g.GetComponent<EnemyLaser>();
                p.velocity = Random.Range(10f, 20f);
                //Debug.Log(p.velocity);
                
            }

            float wait = Random.Range(0.5f, 2f);
            yield return new WaitForSeconds(wait);
        }

        yield return false;
    }

    IEnumerator ShootGuns()
    {
        float angle = AngleToPlayer();

        for (int s = 0; s < 16; s++)
        {
            float angle2 = s * 10;

            for (int i = 0; i < 4; i++)
            {
                Vector3 point = GetGunPos(i);

                float angle3 = (i % 2 == 1) ? angle2 : -angle2;

                GameObject.Instantiate(Resources.Load("shots/PoliceShot"), point, Quaternion.Euler(0, 180f, angle + angle3));
                //GameObject.Instantiate(Resources.Load("shots/PoliceShot"), point, Quaternion.Euler(0, 180f, angle - angle2));

                
            }

            yield return new WaitForSeconds(0.2f);
        }

        yield return false;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (isdying)
        {
            dyingtime -= Time.deltaTime;
            if (dyingtime <= 0)
            {
                PlayerController p = player.GetComponent<PlayerController>();
                p.shields += 15;
                if (p.shields > 30)
                    p.shields = 30;
                GameManager.getGameManager().changeShields(p.shields);

                GameObject.Destroy(gameObject);
            }

            for (int i = 0; i < 4; i++)
            {
                explosions[i].transform.position = GetGunPos(i);
            }
            explosions[4].transform.position = transform.position;
        }

        float distancebetween = (player.transform.position - transform.position).magnitude;


        if (!gm.firstswat)
        {
            if (distancebetween < 30f)
            {
                gm.firstswat = true;
                gm.enqueMessage("Space Swat: \"Stop right there criminal scum!!\"", new Color(0.5f, 0.5f, 1f));
            }
        }

        turnwait -= Time.deltaTime;

        

        if (turnwait < 0)
        {
            turnwait += (6f - (player.transform.position - transform.position).magnitude / 3f)  + Random.Range(0f, 5f);

            if (turnwait < 3f)
                turnwait = 3f;

            target = player.transform.position - transform.position;

            float angle2 = Mathf.Atan2(target.y, target.x) * 180 / Mathf.PI;
            angle2 -= 90;
            if (angle2 < -180)
                angle2 += 360;

            targetangle = -angle2;
        }
        
        float distadj;

        if(distancebetween > 52f)
            distadj = 0.1f;
        else
            distadj = 2f;

        transform.rotation = Quaternion.Slerp(Quaternion.Euler(new Vector3(0f, 180f, transform.rotation.eulerAngles.z)), Quaternion.Euler(new Vector3(0f, 180f, targetangle)), Time.deltaTime/distadj);

        //Debug.DrawLine(transform.position, target);

        velocity += transform.rotation * (Vector3.up * Time.deltaTime * 10);
        if(velocity.magnitude > (3f + (distancebetween / 3f)))
            velocity = velocity.normalized * (3f + (distancebetween / 3f));

        if (velocity.magnitude > 30)
            velocity = Quaternion.Euler(0, 180, AngleToPlayer()) * (Vector3.up * velocity.magnitude * 1.5f);

        transform.position += velocity * Time.deltaTime;

        shotdelay -= Time.deltaTime;
        if (shotdelay <= 0 && distancebetween < 40f)
        {
            int shottype = Random.Range(1, 10);
            if (shottype < 7)
                StartCoroutine("Shotgun");
            else
                StartCoroutine("ShootGuns");

            shotdelay = 6f;
        }
	}

    void OnTriggerEnter(Collider other)
    {

        PlayerLaser oth = other.gameObject.GetComponent<PlayerLaser>();
        if (oth != null)
        {
            GameObject.Destroy(other.gameObject);
            health -= 1;

            if (health <= 0 && !isdying)
            {
                isdying = true;

                for (int i = 0; i < 4; i++)
                {
                    Vector3 gun = GetGunPos(i);
                    explosions[i] = (GameObject)GameObject.Instantiate(Resources.Load("explosions/deathsplosion"), gun, transform.rotation);
                }

                explosions[4] = (GameObject)GameObject.Instantiate(Resources.Load("explosions/deathsplosion"), transform.position, transform.rotation);
                
                //GameObject.Destroy(gameObject);
            }
        }
    }
}
