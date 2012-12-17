using UnityEngine;
using System.Collections;

public class mainscreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	

    }
    private float rotAngle = -5;
    public int what;
    private Vector2 pivotPoint;
    public Texture mainscreentex;
    public Texture mainscreenbut;   
    void OnGUI()
    {

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), mainscreentex, ScaleMode.ScaleToFit, true);
        Rect r = new Rect(Screen.width - 330, Screen.height - 200, 144 ,40);

        
        pivotPoint = new Vector2(Screen.width - 330 + 77, Screen.height - 200 + 20);
        GUIUtility.RotateAroundPivot(rotAngle, pivotPoint);
        if (Mathf.Sin(Time.time) > 0)
            rotAngle += Time.deltaTime * 2;
        else
            rotAngle -= Time.deltaTime * 2;
        
        GUI.DrawTexture(r, mainscreenbut, ScaleMode.ScaleToFit, true);
        if(Input.GetMouseButton(0))
        {
            if (r.Contains(Event.current.mousePosition)) Application.LoadLevel(1);
        }
    }


	// Update is called once per frame
	void Update () {
	
	
    
    
    }
}
