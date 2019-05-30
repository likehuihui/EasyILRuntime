using UnityEngine;
using System.Collections;

public class AnimationControl : MonoBehaviour {
	
	/// <summary>
	/// 和平待机
	/// </summary>
    public const string animPeaceWait = "peacewait";
//	private float speedpeaceWait = 1;
	/// <summary>
	///  行走
	/// </summary>
	public const string animWalk="walk";
//	private float speedWalk = 1;
	/// <summary>
	/// 奔跑
	/// </summary>
	public const string animRun="run";
//	private float speedRun = 1;
	/// <summary>
	/// 受攻击
	/// </summary>
	public const string animAttcked="attcked";
//	private float speedAttcked = 1;
	/// <summary>
	/// 战斗等待
	/// </summary>
	public const string animWarWait="warwait";
//	private float speedWarWait = 1;
	/// <summary>
	/// 攻击1
	/// </summary>
	public const string animAttck1="attck1";
//	private float speedAttck1 = 1;
	/// <summary>
	/// 攻击2
	/// </summary>
	public const string animAttck2="attck2";
//	private float speedAttck2 = 1;
	/// <summary>
	/// 死亡
	/// </summary>
	public const string animDead="dead";
	
	public const string animDid="did";
	
	private GameObject obj = null;
	public string theObjName;
	
	
//	private float newTime = 0;
	
	public bool isShowAnimation = false;
	// Use this for initialization
	void Awake()
	{
		obj = GameObject.Find(this.transform.name);
//		string str = this.transform.name;
//		long roleId ;
//		if(str.StartsWith(GameValue.PLAYER_NAME))
//		{
//			roleId = long.Parse(str.Remove(0,10));
//			if(SceneInfoMap.Map.ContainsKey(roleId))
//			{
//				OtherRoleInfo roleInfo = SceneInfoMap.Map[roleId];
//				if(roleInfo.getCurrHP()>0)
//				{
//					PlayAnimPeaceWait();
//				}
//				else 
//				{
//					PlayerAnimDid();
//				}
//			}
//		}
//		else if(str.StartsWith(GameValue.NPC_NAME))
//		{
//			print("str================"+str);
//			roleId = long.Parse(str.Remove(0,4));
//			if(SceneInfoMap.NpcMap.ContainsKey(roleId))
//			{
//				NPCInfo npcInfo = SceneInfoMap.NpcMap[roleId];
//				if(npcInfo.CurrHP>0)
//				{
//					PlayAnimPeaceWait();
//				}
//				else 
//				{
//					PlayerAnimDid();
//				}
//			}
//		}
//		else 
//		{
//			if(SceneInfoMap.RoleInfo.CurrHP>0)
//			{
//				PlayAnimPeaceWait();
//			}
//			else 
//			{
//				PlayerAnimDid();
//			}
//		}
		
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Q))
		{
			if(isShowAnimation)
			{
				isShowAnimation = false;
			}
			else 
			{
				isShowAnimation = true;
			}
		}
		if(isShowAnimation)
		{
	//		ControlAnimation();
	//		CountDownTime();
		}
	//	PlayAnimWalk();
	}
/*	
	public void CountDownTime()
	{
		if(newTime < Time.time)
		{
			countdown--;
			newTime = Time.time + addTime;
		}
		if(countdown <= 0)
		{
		
			wnether = 0;	
		}
	}
	public void ControlAnimation()
	{
		if(Input.GetKey(KeyCode.Alpha2))
		{
			PlayAnimWalk();
		}
        else if(Input.GetKey(KeyCode.Alpha3))
		{
			PlayAnimRun();
		}
		else if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			PlayAnimAttcked();
		}
		else if(Input.GetKeyDown(KeyCode.Alpha5))
		{
			PlayAnimWarWait();
		}
		else if(Input.GetKeyDown(KeyCode.Alpha6))
		{
			PlayAnimAttck1();
		}
		else if(Input.GetKeyDown(KeyCode.Alpha7))
		{
			PlayAnimAttck2();
		}
		else if(Input.GetKeyDown(KeyCode.Alpha8))
		{
			PlayAnimDead();
			isShowWork = true;
		}
		else if(wnether == 1 && !isShowWork)
		{
			PlayAnimWarWait();
		}
		else if(wnether == 0 && !isShowWork)
		{
			PlayAnimPeaceWait();
		}
		
	}
	*/
	/// <summary>
	///停止所有动画
	/// </summary>
	public void AnimationStop()
	{
		GetComponent<Animation>().Stop(animPeaceWait);
		GetComponent<Animation>().Stop(animWalk);
		GetComponent<Animation>().Stop(animRun);
		GetComponent<Animation>().Stop(animAttcked);
		GetComponent<Animation>().Stop(animWarWait);
		GetComponent<Animation>().Stop(animAttck1);
		GetComponent<Animation>().Stop(animAttck2);
		GetComponent<Animation>().Stop(animDead);
		GetComponent<Animation>().Stop(animDid);
		
	}
	/// <summary>
	/// 和平待机
	/// </summary>
	public void PlayAnimPeaceWait()
	{
		obj.GetComponent<Animation>().Blend(animPeaceWait,110f,2f);
	}
	/// <summary>
	/// 停止和平待机
	/// </summary>
	public void StopAnimPeaceWait()
	{
		obj.GetComponent<Animation>().Stop(animPeaceWait);
	}
	/// <summary>
	///行走
	/// </summary>
	public void PlayAnimWalk()
	{
		obj.GetComponent<Animation>().Blend(animWalk,32f,2f);
	}
	/// <summary>
	///停止行走
	/// </summary>
	public void StopAnimWalk()
	{
		obj.GetComponent<Animation>().Stop(animWalk);
	}
	/// <summary>
	/// 跑
	/// </summary>
	public void PlayAnimRun()
	{
		obj.GetComponent<Animation>().Play(animRun);
	}
	/// <summary>
	/// 停止跑
	/// </summary>
	public void StopAnimRun()
	{
		obj.GetComponent<Animation>().Stop(animRun);
	}
	/// <summary>
	/// 被攻击.
	/// </summary>
	public void PlayAnimAttcked()
	{
		obj.GetComponent<Animation>().Play(animAttcked);
		GetComponent<Animation>().CrossFadeQueued (animWarWait,0.3f,QueueMode.CompleteOthers );
	}
	/// <summary>
	/// 停止被攻击.
	/// </summary>
	public void StopAnimAttcked()
	{
		obj.GetComponent<Animation>().Stop(animAttcked);
	}
	/// <summary>
	/// 战斗呼吸
	/// </summary>
	public void PlayAnimWarWait()
	{
		obj.GetComponent<Animation>().Play(animWarWait);
	}
	/// <summary>
	/// 停止战斗呼吸
	/// </summary>
	public void StopAnimWarWait()
	{
		obj.GetComponent<Animation>().Stop(animWarWait);
	}
	/// <summary>
	/// 攻击1
	/// </summary>
	public void PlayAnimAttck1()
	{
		GetComponent<Animation>().Stop(animPeaceWait);
		GetComponent<Animation>().Stop(animWalk);
		GetComponent<Animation>().Stop(animRun);
		GetComponent<Animation>().Stop(animAttcked);
		GetComponent<Animation>().Stop(animWarWait);
		
		obj.GetComponent<Animation>().Play(animAttck1);
		
		GetComponent<Animation>().CrossFadeQueued (animWarWait,0.3f, QueueMode.CompleteOthers );

	}
	/// <summary>
	/// 停止攻击1
	/// </summary>
	public void StopAnimAttck1()
	{
		
		
		obj.GetComponent<Animation>().Stop(animAttck1);
		
	}
	/// <summary>
	/// 攻击2
	/// </summary>
	public void PlayAnimAttck2()
	{
		GetComponent<Animation>().Stop(animPeaceWait);
		GetComponent<Animation>().Stop(animWalk);
		GetComponent<Animation>().Stop(animRun);
		GetComponent<Animation>().Stop(animAttcked);
		GetComponent<Animation>().Stop(animWarWait);
		
		obj.GetComponent<Animation>().Play(animAttck2);
		GetComponent<Animation>().CrossFadeQueued (animWarWait,0.3f,QueueMode.CompleteOthers );
	}
	/// <summary>
	/// 停止攻击2
	/// </summary>
	public void StopAnimAttck2()
	{
		obj.GetComponent<Animation>().Stop(animAttck2);
	}
	/// <summary>
	/// 死亡
	/// </summary>
	public void PlayAnimDead()
	{
		AnimationStop();
		obj.GetComponent<Animation>().Play(animDead);
	}
	/// <summary>
	/// 停止死亡
	/// </summary>
	public void StopAnimDead()
	{
		obj.GetComponent<Animation>().Stop(animDead);
	}
	
	public void PlayerAnimDid()
	{
		obj.GetComponent<Animation>().Stop(animDid);
	}
}
