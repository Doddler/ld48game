using UnityEngine;
using System.Collections;

public class TaxiGroup : MonoBehaviour {

    Vector3 move;

    public int childcount;

    float angle;

    public bool isSplit = false;

    GameObject player;

	// Use this for initialization
	void Start () {
        angle = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        move = transform.up.normalized * 3f;

        childcount = gameObject.transform.childCount;

        player = PlayerController.getPlayer();
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.transform.childCount == 0)
            GameObject.Destroy(gameObject);

        float distancebetween = (player.transform.position - transform.position).magnitude;

        if (distancebetween > 150)
            Destroy(gameObject);

        transform.position += move * Time.deltaTime;

        if (isSplit)
            return;

        if (gameObject.transform.childCount != childcount)
        {
            SplitGroup();
        }
        
	}

    void SplitGroup()
    {
        childcount = gameObject.transform.childCount;

        while(gameObject.transform.childCount > 0)
        {
            SpaceTaxi t = gameObject.transform.GetChild(0).GetComponent<SpaceTaxi>();
        
            t. velocity = move;
            t.angle = angle;
            t.newangle = Random.Range(0, 360);
            t.isIndependant = true;

            //Debug.Log(t[i].newangle);

            t.gameObject.transform.parent = null;
        }

        //move *= 0f;
        isSplit = true;

    }

    void OnTriggerEnter(Collider other)
    {

        PlayerLaser oth = other.gameObject.GetComponent<PlayerLaser>();
        if (oth != null && isSplit == false)
        {
//            GameObject.Destroy(gameObject.GetComponent<SphereCollider>());
            SplitGroup();
            
        }
    }
}
