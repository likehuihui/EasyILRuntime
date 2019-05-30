using UnityEngine;
using System.Collections;

public class TouchPoint : MonoBehaviour {
	
	private GameObject thePlayer;
	PlayerAttribute theplayerAttribute;
	// Use this for initialization
	void Start () {
		thePlayer = GameObject.Find("Player");
		theplayerAttribute = thePlayer.GetComponent<PlayerAttribute>();
	}
	
	// Update is called once per frame
	void Update () {
		TheMouseControl();
	}
	void TheMouseControl()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray,out hit))
			{
				theplayerAttribute.SendMovePath(hit.point);
			}
		}
	}
}
