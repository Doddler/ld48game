using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public float timeSinceStart;
    public int nefarious;
    public int score;
    public int playerHealth;
    public int playerShields;
    private List<string> messageQueue;
    private List<Color> messageColors;
    private float timeSinceMessage = 10f;
    static GameManager gm;

    public bool firstpolice;
    public bool firstswat;

    private int linecount;

    public bool firstcivi;
    private int tutorial = 0;


    float timesincewoop = 0f;

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
        messageQueue = new List<string>();
        messageColors = new List<Color>();

        enqueMessage("Now entering imperial space...", Color.yellow);
	}
	
	// Update is called once per frame
	void Update () {
        timeSinceStart += Time.deltaTime;

        //nefarious = (int)(timeSinceStart * 10);

        if (timeSinceStart > 2f && tutorial == 0)
        {
            enqueMessage("Use the arrowkeys to pilot your ship, use the CTRL key to fire lasers.", Color.yellow);
            tutorial = 1;
        }

        if (timeSinceStart > 12f && tutorial == 1)
        {
            enqueMessage("Destroying civilian vessels increases notoriety and recovers shield levels.", Color.yellow);
            tutorial = 2;
        }

        if (firstpolice)
        {
            timesincewoop += Time.deltaTime;
            if (timesincewoop > 5f && tutorial == 2)
            {
                enqueMessage("Larger Police vessels can be destroyed to boost shield levels as well, though not as much.", Color.yellow);
                tutorial = 3;
            }
        }

        message = "";
        for (int i = 0; i < messageQueue.Count; i++)
        {
            message += messageQueue[i] + "\n";
        }

        //if (Time.time - timeSinceMessage > 10)
        //{
        //    if (messageQueue.Count > 0)
        //    {
        //        message = (string)messageQueue.Dequeue();
        //        timeSinceMessage = Time.time;
        //    }
            
        //}
	}



    //ui controller stuff
    public Texture nef_bar_out;
    public Texture nef_bar_fill;
    public Texture health_bar_out;
    public Texture health_bar_in;
    public Texture shield_bar_out;
    public Texture shield_bar_in;
    public Texture chatbox;
    public Texture scanlines;
    public Font MyFont;
    public Texture offthecharts;
    public Texture mlg;
    private string message = "";
    private int scanlinepos = Screen.height - 100;
    private float rotAngle = 0;
    private float defaultRotAngle = 0;
    private Vector2 pivotPoint;
    void OnGUI()
    {

       


        GUI.DrawTexture(new Rect(10, 10, 312, 64), nef_bar_out, ScaleMode.StretchToFill, true);
        int numinserts = (int)Mathf.Floor(nefarious / 18);
        //Debug.Log(numinserts);
        for (int i = 0; i < numinserts; i++)
        {
            GUI.DrawTexture(new Rect(23 + i*14 + i*2, 44, 14, 26), nef_bar_fill);
        }

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
        //scanlines
        GUI.DrawTexture(new Rect(Screen.width / 2 - 256, scanlinepos, 512, 114), scanlines, ScaleMode.StretchToFill, true);

      
        // GUI.DrawTexture(new Rect(Screen.width / 2 - 256, Screen.height - 100, 512,), scanlines, ScaleMode.StretchToFill, true);
        for (int i = 0; i < messageQueue.Count; i++)
        {
            GUIStyle TextStyle = new GUIStyle();
            TextStyle.font = MyFont;
            TextStyle.wordWrap = true;
            TextStyle.normal.textColor = Color.black;

            GUI.Label(new Rect(Screen.width / 2 - 256 + 20 + 2, Screen.height - 80 + i * 15 + 2, 470 + 2, 70 + 2), messageQueue[i].ToUpper(), TextStyle);

            TextStyle.normal.textColor = messageColors[i];

            GUI.Label(new Rect(Screen.width / 2 - 256 + 20, Screen.height - 80 + i * 15, 470, 70), messageQueue[i].ToUpper(), TextStyle);


        }
        //chatbox
        GUI.DrawTexture(new Rect(Screen.width / 2 - 256, Screen.height - 100, 512, 114), chatbox, ScaleMode.ScaleAndCrop, true);

        pivotPoint = new Vector2(500, 26);
        GUIUtility.RotateAroundPivot(rotAngle, pivotPoint);
        int textwidth = 200;
        int textheight = 32;
        if (Mathf.Sin(Time.time) > 0)
        {
            rotAngle += 0.1f;
        }
        else
        {
            rotAngle -= 0.1f;
            textwidth--;
            textheight--;
        }
            //Nefarious meter updates
        if (numinserts > 18)
            GUI.DrawTexture(new Rect(400, 10, 200, 32), offthecharts, ScaleMode.ScaleToFit, true);

        GUIUtility.RotateAroundPivot(rotAngle + 30, new Vector2(Screen.width - 69 + 32, 10));

        if (numinserts > 25)
            GUI.DrawTexture(new Rect(Screen.width - 69, 10, 65, 32), mlg, ScaleMode.ScaleToFit, true);


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

    public void enqueMessage(string message, Color c)
    {
        message = "> " + message;

        if (message.Length > 45)
        {
            string[] original = message.Split(null);

            string final = "";
            int len = 0;

            foreach (var i in original)
            {
                len += i.Length;
                if (len > 45)
                {
                    Debug.Log("Queing: " + final);
                    messageQueue.Add(final);
                    messageColors.Add(c);
                    final = "  " + i;
                    len = i.Length + 2;
                    continue;
                }
                if (final != "")
                {
                    final += " ";
                    len += 1;
                }

                final += i;
            }

            Debug.Log("Queing: " + final);
            messageQueue.Add(final);
            messageColors.Add(c);

        }
        else
        {
            messageQueue.Add(message);
            messageColors.Add(c);
        }

        while (messageQueue.Count > 5)
        {
            messageQueue.RemoveAt(0);
            messageColors.RemoveAt(0);
        }
    }

}
