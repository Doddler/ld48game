using UnityEngine;
using System.Collections;

public class ArrowManager : MonoBehaviour 
{
    GameObject player;

    public GameObject arrow;

	// Use this for initialization
	void Start () 
    {
        arrow = (GameObject)GameObject.Instantiate(arrow);
        player = PlayerController.getPlayer();
	}

    float AngleToPlayer()
    {
        Vector3 target = player.transform.position - transform.position;

        float angle2 = Mathf.Atan2(target.y, target.x) * 180 / Mathf.PI;
        angle2 -= 90;
        if (angle2 < -180)
            angle2 += 360;

        return -angle2;
    }
	
	// Update is called once per frame
	void Update () 
    {
        float distancebetween = (player.transform.position - transform.position).magnitude;

        if (distancebetween < 25f || distancebetween > 100f)
        {
            arrow.renderer.enabled = false;
            return;
        }
        else
            arrow.renderer.enabled = true;

        float angle = AngleToPlayer() + 180;
        Quaternion quat = Quaternion.Euler(0, 180, angle);

        arrow.transform.rotation = quat;

        arrow.transform.position = (quat * Vector3.up).normalized * 15 + player.transform.position;

        float scale = (100 - (distancebetween-50)) / 100;

        if (scale <= 0)
        {
            arrow.renderer.enabled = false;
            return;
        }

        arrow.transform.localScale = new Vector3(scale*2, scale*2, 1f);
	}

    bool isShuttingDown = false;

    void OnApplicationQuit()
    {

        isShuttingDown = true;

    }

    void OnDestroy()
    {
        if(!isShuttingDown)
            Destroy(arrow);
    }
}
