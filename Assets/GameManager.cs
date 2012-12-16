using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public float timeSinceStart;
    public int nefarious;
    public int score;
    public int playerHealth;
    public int playerShields;
    private Queue messageQueue;
    private float timeSinceMessage = 10f;
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
        messageQueue = new Queue();


	}
	
	// Update is called once per frame
	void Update () {
        timeSinceStart += Time.deltaTime;

        //nefarious = (int)(timeSinceStart * 10);


        if (Time.time - timeSinceMessage > 10)
        {
            if (messageQueue.Count > 0)
            {
                message = (string)messageQueue.Dequeue();
                timeSinceMessage = Time.time;
            }
            
        }
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
        GUIStyle TextStyle = new GUIStyle();
        TextStyle.font = MyFont;
        TextStyle.normal.textColor = Color.green;

        GUI.Label(new Rect(Screen.width / 2 - 256 + 20, Screen.height - 80, 470, 70), message.ToUpper(), TextStyle);

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

    public void enqueMessage(string message)
    {
        messageQueue.Enqueue(message);
    }

}
