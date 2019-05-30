using UnityEngine;
using System.Collections;

public class PlayerAttribute : MonoBehaviour {
	
	CharacterController colliderPlayer;
	AnimationControl animationControl;
	private bool isMoveing;
	private Vector3 nor;
	private Vector3 endVector;
	
	// Use this for initialization
	void Awake()
	{
		colliderPlayer = GetComponent<CharacterController>();
		animationControl = GetComponent<AnimationControl>();
       
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdatePlayerAttribute();
	}
	
	void UpdatePlayerAttribute()
	{
		if(!colliderPlayer.isGrounded)
		{
			print("===UpdatePlayerAttribute===");
			colliderPlayer.Move(Vector3.up*Time.deltaTime*-10);
		}
		
		if(isMoveing)
		{
			MoveOrientation(nor);
			MoveStop(endVector);
		}
	}
	public void SendMovePath(Vector3 endPath)
	{
		Vector3 startPath = this.transform.position;
		endVector = endPath;
		nor = (endPath-startPath).normalized;
		isMoveing = true;
		animationControl.PlayAnimRun();
	}
	
	void MoveOrientation(Vector3 vector)
	{
		Quaternion tranTemp ;
		tranTemp = Quaternion.LookRotation(vector);
		float yTemp = tranTemp.eulerAngles.y;
		Quaternion currentRotation = Quaternion.Euler (0, yTemp, 0);
		this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation,currentRotation,Time.deltaTime*300);
		
		colliderPlayer.Move(vector*Time.deltaTime*3);
		
	}
	
	void MoveStop(Vector3 vector)
	{
		Vector3 myPing = this.transform.position;
		if(Vector3.Distance(myPing,vector) <= 0.3f)
		{
			isMoveing = false;
			animationControl.PlayAnimPeaceWait();
		}
	}
	
}
