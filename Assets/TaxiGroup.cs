using UnityEngine;
using System.Collections;

public class TaxiGroup : MonoBehaviour {

    Vector3 move;

    public int childcount;

    float angle;

    public bool isSplit = false;

	// Use this for initialization
	void Start () {
        angle = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        move = transform.up.normalized * 3f;

        childcount = gameObject.transform.childCount;
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.transform.childCount == 0)
            GameObject.Destroy(gameObject);

        transform.position += move * Time.deltaTime;

        if (isSplit)
            return;

        if (gameObject.transform.childCount != childcount)
        {
            childcount = gameObject.transform.childCount;

            for (int i = 0; i < childcount; i++)
            {
                SpaceTaxi t = gameObject.transform.GetChild(i).GetComponent<SpaceTaxi>();

                if (t == null)
                    continue;

                t.velocity = move;
                t.angle = angle;
                t.newangle = Random.Range(0, 360);
                t.isIndependant = true;

                Debug.Log(t.newangle);

                t.gameObject.transform.parent = null;
            }

            //move *= 0f;
            isSplit = true;
        }
        
        


	}


}
