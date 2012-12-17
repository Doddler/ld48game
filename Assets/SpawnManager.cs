using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

    GameObject player;
    PlayerController pm;
    GameManager gm;

    float spawntick = 7f;
    float msgtimer = 0f;
    string message = "";

    bool skippolice = false;

    bool firstpolice = false;

	// Use this for initialization
	void Start () {
        gm = GameManager.getGameManager();
        player = PlayerController.getPlayer();
        pm = player.GetComponent<PlayerController>();
	}

    Vector3 GetValidSpawnpoint()
    {
        float angle = Random.Range(0f, 360f);
        Quaternion q = Quaternion.Euler(0f, 0, angle);
        Vector3 v = q * Vector3.up;
        
        return v.normalized * 60f + player.transform.position;
    }

    void OnGUI()
    {
        // copy the "label" style from the current skin
        GUIStyle centeredTextStyle = new GUIStyle("label");
        centeredTextStyle.alignment = TextAnchor.MiddleCenter;
        centeredTextStyle.fontSize = 24;
        
        //if(msgtimer > 0)
        //    GUI.Label(new Rect(0, 100, Screen.width, 100), message, centeredTextStyle);
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (pm.isdying)
            return;

        msgtimer -= Time.deltaTime;

        spawntick -= Time.deltaTime;
        if (spawntick > 0f)
            return;

        spawntick += 10f + Random.Range(0f, 10f);

        message = "";
        msgtimer = 5f;

        if (1==1)
        {
            int r = Random.Range(0, 3);
            if (r < 2)
            {
                GameObject transport = (GameObject)GameObject.Instantiate(Resources.Load("ships/cargo_ship"), GetValidSpawnpoint(), Quaternion.identity);

                string cargotype = "";

                int type = Random.Range(0, 10);
                switch (type)
                {
                    case 0: cargotype = "toys for orphans"; break;
                    case 1: cargotype = "newborn puppies"; break;
                    case 2: cargotype = "christmas presents"; break;
                    case 3: cargotype = "rainbows and sunshine"; break;
                    case 4: cargotype = "elderly people"; break;
                    case 5: cargotype = "the last unicorn"; break;
                    case 6: cargotype = "food for the poor"; break;
                    case 7: cargotype = "the cure for all diseases"; break;
                    case 8: cargotype = "playful kittens"; break;
                    case 9: cargotype = "the ludum dare servers"; break;
                    case 10: cargotype = "world peace"; break;
                    
                    default:
                        cargotype = "OMGAR";
                        break;
                }

                message = "A cargo ship containing (" + cargotype + ") has been detected!";
                
            }
            if (r == 2)
            {
                GameObject taxi = (GameObject)GameObject.Instantiate(Resources.Load("ships/TaxiGroup3"), GetValidSpawnpoint(), Quaternion.identity);
                message = "A group of civilian ships has been detected!";
            }
            if (r == 3)
            {
                GameObject taxi = (GameObject)GameObject.Instantiate(Resources.Load("ships/TaxiGroup4"), GetValidSpawnpoint(), Quaternion.identity);
                message = "A group of civilian ships has been detected!";
            }
            Debug.Log("message");
            gm.enqueMessage(message, Color.green);
        }



        if (gm.nefarious > 0)
        {
            int chance = Random.Range(0, 20);

            int surge = 1;

            if (skippolice)
            {
                surge = (int)(surge / 2);
                skippolice = false;
            }

            if (chance == 5)
            {
                if (message != "")
                    message += "\n";
                message = "A surge of police activity has been detected!";

                gm.enqueMessage(message, new Color(0.5f, 0.5f, 1f));

                surge = 2;
                skippolice = true;
            }

            int num = GameObject.FindGameObjectsWithTag("Swat").Length;


            for (int i = 0; i < ((int)(gm.nefarious / 18 / 4) + 1) * surge; i++)
            {
                if (num < (int)(gm.nefarious / 18/ 2))
                {
                    GameObject policeship = (GameObject)GameObject.Instantiate(Resources.Load("ships/PoliceShip"), GetValidSpawnpoint(), Quaternion.identity);
                    num++;
                }
            }
            
        }

        if (gm.nefarious > 100)
        {
            int chance = Random.Range(0, gm.nefarious/18 + 15);

            int num = GameObject.FindGameObjectsWithTag("Swat").Length;

            //Maximum 1 cruiser per 10 ticks of nefariosity.
            if (chance > 15 && num < (int)(gm.nefarious/18/10))
            {
                if (message != "")
                    message += "\n";
                message = "A swat cruiser has been launched to intercept you!";
                gm.enqueMessage(message, new Color(0.5f, 0.5f, 1f));
                GameObject policeship = (GameObject)GameObject.Instantiate(Resources.Load("ships/SpaceSwat"), GetValidSpawnpoint(), Quaternion.identity);
            }
        }

	}
}
