using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public float timeSinceStart;

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
        //Random.seed = (int)(Time.time/100f);
	}
	
	// Update is called once per frame
	void Update () {
        timeSinceStart += Time.deltaTime;


	}
}
