using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public float timeSinceStart;
    public int nefarious;
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




	}
	
	// Update is called once per frame
	void Update () {
        timeSinceStart += Time.deltaTime;

        //nefarious++;






	}



    //ui controller stuff
    public Texture nef_bar_out;
    public Texture nef_bar_fill;

    void OnGUI()
    {
        //156x32
        //setting up ui stuff ehre
        GUI.DrawTexture(new Rect(10, 10, 312, 64), nef_bar_out, ScaleMode.StretchToFill, true);
        //7x13
        GUI.DrawTexture(new Rect(23, 44, 14, 26), nef_bar_fill);

        int numinserts = (int)Mathf.Floor(nefarious / 18);
        
        for (int i = 0; i < numinserts; i++)
        {
            GUI.DrawTexture(new Rect(23 + i*14 + i*2, 44, 14, 26), nef_bar_fill);
        }



    }

}
