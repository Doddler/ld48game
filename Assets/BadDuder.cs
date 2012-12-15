using UnityEngine;
using System.Collections;

public class BadDuder : MonoBehaviour {

    GameObject player;
    public float angle;
    public Vector3 velocity;

    float speedvar;
    float turnvar;
    float aimvar;

    GameObject sirenred;
    GameObject sirenblue;

    float sirentimer = 1f;
    bool sirenstate = false;

	// Use this for initialization
	void Start () {
        player = PlayerController.getPlayer();

        speedvar = Random.Range(0.8f, 1.2f);
        turnvar = Random.Range(1.5f, 3.5f);
        aimvar = Random.Range(0f, 2f);

        sirenred = transform.FindChild("SirenRed").gameObject;
        sirenblue = transform.FindChild("SirenBlue").gameObject;

        transform.position = new Vector3(transform.position.x, transform.position.y, 1.74f);
    }

	// Update is called once per frame
	void Update () {
        if (sirenstate)
        {
            sirenred.renderer.enabled = false;
            sirenblue.renderer.enabled = true;
        }
        else
        {
            sirenred.renderer.enabled = true;
            sirenblue.renderer.enabled = false;
        }

        sirentimer -= Time.deltaTime;
        if (sirentimer < 0f)
        {
            sirentimer = 0.2f;
            sirenstate = !sirenstate;
        }

        Vector3 target = player.transform.position - transform.position;
        Vector3 forward = transform.up;
        
        angle = Vector3.Angle(target, forward);

        if (player.transform.position.x > transform.position.x)
            angle *= -1;
        
        var n = (player.transform.position.normalized * aimvar + player.transform.position) - transform.position;
        var newRotation = Quaternion.LookRotation(n, Vector3.back) * Quaternion.Euler(270, 0, 0);

        newRotation = Quaternion.Euler(new Vector3(0, 180, newRotation.eulerAngles.z));
        
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * turnvar);
        
        transform.position = new Vector3(transform.position.x, transform.position.y, 1.74f);
        if (target.magnitude > 5)
            velocity += transform.rotation * (Vector3.up * speedvar * Time.deltaTime * 12);

        if (velocity.magnitude > 12 * speedvar)
            velocity = velocity.normalized * 12 * speedvar;

        transform.position -= velocity * Time.deltaTime;

        //transform.rotation = rotation;
	}
}
