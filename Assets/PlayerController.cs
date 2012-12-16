using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    GameManager gm;
         
    public static GameObject player;

    public static GameObject getPlayer()
    {
        if (player == null)
            player = GameObject.Find("Player");
        return player;
    }

    public float angle;
    public Vector3 velocity;

    public Material bgmat;
    public Material dust;
    public GameObject bgobj;

    public GameObject bullet;

    public GameObject explosion;

    ParticleSystem thrust;

    public float firedelay = 1;
    float fdelay = 0;
    bool fire2 = false;

    public bool isdead = false;
    bool isdying = false;
    float dyingtimer = 5f;

    public int health = 30;
    public int shields = 30;

    float shakeamnt = 0f;

    float shieldregentimer = 0f;

	// Use this for initialization
	void Start () {
        velocity = Vector3.zero;

        player = this.gameObject;
        gm = GameManager.getGameManager();
        thrust = gameObject.GetComponentInChildren<ParticleSystem>();
        //thrust.Stop();
        gm.changeHealth(health);
        gm.changeShields(shields);
	}

    void LateUpdate()
    {
        if (isdying)
        {
            shields = 0;
            health = 0;
            gm.changeHealth(0);
            gm.changeShields(0);
        }
    }
   	
	// Update is called once per frame
	void Update () {

        if (isdead)
            return;

        if (isdying)
        {
            explosion.transform.position = transform.position;
            dyingtimer -= Time.deltaTime;
            if (dyingtimer < 0)
            {
                renderer.enabled = false;
                velocity = Vector3.zero;
                isdead = true;
                for (int i = 0; i < transform.childCount; i++)
                {
                    GameObject.Destroy(transform.GetChild(i).gameObject);
                }
                return;
            }
        }



        shieldregentimer -= Time.deltaTime ;
        if (shieldregentimer <= 0)
        {
            if (shields < 30)
            {
                shields++;
                gm.changeShields(shields);
            }
            shieldregentimer = 0.3f;
        }


        fdelay -= Time.deltaTime;

        float leftright = Input.GetAxis("Horizontal");

        angle += leftright * Time.deltaTime * 130 * -1;

        float forwardback = Input.GetAxis("Vertical");

        if (forwardback < 0)
            forwardback *= 0.7f;

        if (forwardback > 0.1f && !thrust.isPlaying)
            thrust.Play();
        if(forwardback < 0.1f)
            thrust.Stop();

        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        transform.rotation = rotation;

        velocity += rotation * (Vector3.up * forwardback * Time.deltaTime* 20);

        if (velocity.magnitude > 10)
            velocity = velocity.normalized * 10;

        transform.position += velocity * Time.deltaTime;

        //Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);

        shakeamnt -= Time.deltaTime;
        if (shakeamnt > 0.5f)
            shakeamnt -= Time.deltaTime * 3f;
        if (shakeamnt < 0)
            shakeamnt = 0;
        if (shakeamnt > 6)
            shakeamnt = 6;
        
        Vector3 shake = new Vector3(transform.position.x + Random.Range(-shakeamnt, shakeamnt), transform.position.y + Random.Range(-shakeamnt, shakeamnt), -10);

        Camera.main.transform.position = shake;

        bgobj.transform.position = new Vector3(transform.position.x, transform.position.y, 100);
        bgmat.mainTextureOffset = new Vector2(transform.position.x, transform.position.y) * -0.001f;
        dust.mainTextureOffset = new Vector2(transform.position.x, transform.position.y) * -0.01f;

        if (Input.GetButton("Fire1") && fdelay < 0)
        {
            Vector3 off;

            if (fire2 == false)
                off = new Vector3(0.5f, 1f, 0);
            else
                off = new Vector3(-0.5f, 1f, 0);
            Vector3 bulletspawn = Quaternion.Euler(new Vector3(0, 0, angle)) * off * 1 + transform.position;

            fdelay = firedelay;
            GameObject.Instantiate(bullet, bulletspawn, transform.rotation);

            fire2 = !fire2;
        }
	}

    void OnTriggerEnter(Collider other)
    {
    
        EnemyLaser oth = other.gameObject.GetComponent<EnemyLaser>();
        if (oth != null)
        {
            GameObject.Destroy(other.gameObject);
            shakeamnt += 0.5f;
            shieldregentimer = 5f;
            if (shields > 0)
            {
                shields--;
                gm.changeShields(shields);
            }
            else if (health > 0)
            {
                health--;
                gm.changeHealth(health);
            }
            
            if (!isdying && !isdead)
            {
                if (health <= 0)
                {
                    explosion = (GameObject) GameObject.Instantiate(Resources.Load("explosions/playerdeathsplosion"), player.transform.position, player.transform.rotation);
                    isdying = true;
                }
            }
        }
    }

}
