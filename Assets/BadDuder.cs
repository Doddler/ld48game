using UnityEngine;
using System.Collections;

public class BadDuder : MonoBehaviour {

    GameObject player;
    public float angle;
    public Vector3 velocity;

	// Use this for initialization
	void Start () {
        player = PlayerController.getPlayer();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 target = player.transform.position - transform.position;
        Vector3 forward = transform.up;
        
        angle = Vector3.Angle(target, forward);

        if (player.transform.position.x > transform.position.x)
            angle *= -1;
        

        //transform.LookAt(player.transform);

        var n = player.transform.position - transform.position;
        var newRotation = Quaternion.LookRotation(n) * Quaternion.Euler(270, 0, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 2.0f);
        //transform.rotation = newRotation;
        transform.position = new Vector3(transform.position.x, transform.position.y, 1.74f);
        if (target.magnitude > 5)
            velocity += transform.rotation * (Vector3.up * 1 * Time.deltaTime * 5);

        if (velocity.magnitude > 7)
            velocity = velocity.normalized * 7;

        transform.position -= velocity * Time.deltaTime;

        //transform.rotation = rotation;
	}
}
