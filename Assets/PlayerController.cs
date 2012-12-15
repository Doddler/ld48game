using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float angle;
    public Vector3 velocity;

    public Material bgmat;
    public GameObject bgobj;

    public GameObject bullet;

    public float firedelay = 1;
    float fdelay = 0;
    bool fire2 = false;

	// Use this for initialization
	void Start () {
        velocity = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        fdelay -= Time.deltaTime;

        float leftright = Input.GetAxis("Horizontal");

        angle += leftright * Time.deltaTime * 100 * -1;

        float forwardback = Input.GetAxis("Vertical");

        if (forwardback < 0)
            forwardback *= 0.7f;

        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        transform.rotation = rotation;

        velocity += rotation * (Vector3.up * forwardback * Time.deltaTime* 5);

        if (velocity.magnitude > 10)
            velocity = velocity.normalized * 10;

        transform.position += velocity * Time.deltaTime;

        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);

        bgobj.transform.position = new Vector3(transform.position.x, transform.position.y, 20);
        bgmat.mainTextureOffset = new Vector2(transform.position.x, transform.position.y) * -0.04f;

        if (Input.GetButton("Fire1") && fdelay < 0)
        {
            Vector3 off;

            if (fire2 == false)
                off = new Vector3(0.5f, 1f, 0);
            else
                off = new Vector3(-0.5f, 1f, 0);
            Vector3 bulletspawn = Quaternion.Euler(new Vector3(0, 0, angle)) * off * 1 + transform.position;

            fdelay = firedelay;
            GameObject.Instantiate(bullet, bulletspawn, transform.rotation);

            fire2 = !fire2;
        }
	}
}
