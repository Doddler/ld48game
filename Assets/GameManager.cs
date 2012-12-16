using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public float timeSinceStart;
    public int nefarious;
    public int score;
    public int playerHealth;
    public int playerShields;
    private Queue messageQueue;
    private float timeSinceMessage;
    static GameManager gm;

    public static GameManager getGameManager()
    {
        if (gm == null)
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        return gm;
    }

	// Use this for initialization
	void Start () {
        timeSinceStart = 0f;
        nefarious = 0;
        score = 0;



	}
	
	// Update is called once per frame
	void Update () {
        timeSinceStart += Time.deltaTime;

        nefarious = (int)(timeSinceStart * 10);

        //nefarious++;

        //Debug.Log(nefarious);




	}



    //ui controller stuff
    public Texture nef_bar_out;
    public Texture nef_bar_fill;
    public Texture health_bar_out;
    public Texture health_bar_in;
    public Texture shield_bar_out;
    public Texture shield_bar_in;
    public Texture chatbox;
    void OnGUI()
    {


        //Nefarious meter updates

        GUI.DrawTexture(new Rect(10, 10, 312, 64), nef_bar_out, ScaleMode.StretchToFill, true);
        int numinserts = (int)Mathf.Floor(nefarious / 18);
        Debug.Log(numinserts);
        for (int i = 0; i < numinserts; i++)
        {
            GUI.DrawTexture(new Rect(23 + i*14 + i*2, 44, 14, 26), nef_bar_fill);
        }


        //spinning off the chartz here

        //health/shields

        GUI.DrawTexture(new Rect(18, 90, 28, 220), health_bar_out, ScaleMode.StretchToFill, true);
        GUI.DrawTexture(new Rect(53, 90, 28, 220), shield_bar_out, ScaleMode.StretchToFill, true);

        for (int i = 0; i < playerHealth; i++)
        {
            
            GUI.DrawTexture(new Rect(20, 304 - 6*(i), 22, 4), health_bar_in, ScaleMode.StretchToFill, true);
        }

        for (int i = 0; i < playerShields; i++)
        {

            GUI.DrawTexture(new Rect(55, 304 - 6 * (i), 22, 4), shield_bar_in, ScaleMode.StretchToFill, true);
        }
        


        //text area

        GUI.DrawTexture(new Rect(Screen.width / 2 - 300, Screen.height - 100, 600, 100), chatbox, ScaleMode.StretchToFill, true);

        if (Time.time - timeSinceMessage > 10)
        {

        }
        //get new message and display it on screen
        else
        {

        }

    }


    public void increaseNef(int num)
    {
        nefarious += num;
    }

    public void increaseScore(int num)
    {
        score += num;
    }

    public void changeShields(int shields)
    {
        playerShields = shields;
    }

    public void changeHealth(int health)
    {
        playerHealth = health;
    }

    public void enqueMessage(string message)
    {
        messageQueue.Enqueue(message);
    }

}
