using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

    GameObject player;
    PlayerController pm;
    GameManager gm;

    public List<GameObject> civilian;
    public List<GameObject> police;
    public List<GameObject> swat;

    float spawntick = 3f;
    float msgtimer = 0f;
    string message = "";

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

        civilian = new List<GameObject>();
        police = new List<GameObject>();
        swat = new List<GameObject>();

        return v.normalized * 60f + player.transform.position;
    }

    void OnGUI()
    {
        // copy the "label" style from the current skin
        GUIStyle centeredTextStyle = new GUIStyle("label");
        centeredTextStyle.alignment = TextAnchor.MiddleCenter;
        centeredTextStyle.fontSize = 24;
        
        if(msgtimer > 0)
            GUI.Label(new Rect(0, 50, Screen.width, 100), message, centeredTextStyle);
    }
	
	// Update is called once per frame
	void Update () 
    {
        msgtimer -= Time.deltaTime;

        spawntick -= Time.deltaTime;
        if (spawntick > 0f)
            return;

        spawntick += 10f + Random.Range(0f, 10f);

        message = "";
        msgtimer = 5f;

        if (civilian.Count <= 0)
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

                message = "- A cargo ship containing\n(" + cargotype + ") has been detected!";

                civilian.Add(transport);
            }
            if (r == 2)
            {
                GameObject taxi = (GameObject)GameObject.Instantiate(Resources.Load("ships/TaxiGroup3"), GetValidSpawnpoint(), Quaternion.identity);
                message = "- A group of civilian ships has been detected!";
                civilian.Add(taxi);
            }
            if (r == 3)
            {
                GameObject taxi = (GameObject)GameObject.Instantiate(Resources.Load("ships/TaxiGroup3"), GetValidSpawnpoint(), Quaternion.identity);
                message = "- A group of civilian ships has been detected!";
                civilian.Add(taxi);
            }
        }

        if (gm.nefarious > 0)
        {
            for (int i = 0; i < (int)(gm.nefarious / 18 / 5) + 1; i++)
            {
                GameObject policeship = (GameObject)GameObject.Instantiate(Resources.Load("ships/PoliceShip"), GetValidSpawnpoint(), Quaternion.identity);
                police.Add(policeship);
            }
            
        }

        if (gm.nefarious > 100)
        {
            int chance = Random.Range(0, gm.nefarious/18 + 15);

            if (chance > 15)
            {
                if (message != "")
                    message += "\n";
                message += "- A swat cruiser has been launched to intercept you!";
                GameObject policeship = (GameObject)GameObject.Instantiate(Resources.Load("ships/SpaceSwat"), GetValidSpawnpoint(), Quaternion.identity);
            }
        }

        Debug.Log(police.Count);
	}
}
