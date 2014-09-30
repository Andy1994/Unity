using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	//Transform组件
	Transform m_transform;
	//动画组件
	Animator m_ani;
	//角色旋转速度
	float m_rotSpeed = 120;
	//计时器
	float m_timer = 2;
	//生命值
	//int m_life = 15;
	//主角
	Player m_player;
	//寻路组件
	NavMeshAgent m_agent;
	//移动速度
	float m_movSpeed = 0.15f;

	void Start(){
		m_transform = this.transform;
		m_ani = this.GetComponent<Animator> ();
		m_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		m_agent = GetComponent<NavMeshAgent> ();
		//设置寻路目标
		m_agent.SetDestination (m_player.transform.position);
	}

	void Update(){
		if(m_player.m_life <= 0)
			return;
		
		//获取当前动画状态
		AnimatorStateInfo stateInfo = m_ani.GetCurrentAnimatorStateInfo (0);
		
		//如果处于待机状态
		if(stateInfo.nameHash == Animator.StringToHash("Base Layer.idle")&&!m_ani.IsInTransition(0))
		{
			m_ani.SetBool("idle",false);
			//待机一定时间
			m_timer -= Time.deltaTime;
			if(m_timer>0)
				return;
			//如果距离主角小于1.5m，进入攻击动画状态
			if(Vector3.Distance(m_transform.position,m_player.m_transform.position)<1.5f)
			{
				m_ani.SetBool("attack",true);
			}
			else
			{
				//重置定时器
				m_timer = 1;
				//设置寻路目标点
				m_agent.SetDestination (m_player.transform.position);
				//进入跑步动画状态
				m_ani.SetBool("run",true);
			}
		}
		//如果处于跑步状态
		if(stateInfo.nameHash == Animator.StringToHash("Base Layer.run")&&!m_ani.IsInTransition(0))
		{
			m_ani.SetBool("run",false);
			//每隔一秒定位主角位置
			m_timer -= Time.deltaTime;
			if(m_timer<0)
			{
				m_agent.SetDestination (m_player.transform.position);
				m_timer = 1;
			}
			//追向主角
			MoveTo();
			//如果距离主角小于1.5m，进入攻击动画
			if(Vector3.Distance(m_transform.position,m_player.m_transform.position)<1.5f)
			{
				//停止寻路
				m_agent.ResetPath();

				m_ani.SetBool("attack",true);
			}
		}
		//如果处于攻击状态
		if(stateInfo.nameHash == Animator.StringToHash("Base Layer.attack")&&!m_ani.IsInTransition(0))
		{
			//面向主角
			RotateTo();
			m_ani.SetBool("attack",false);
			//如果动画播完，重新进入待机状态
			if(stateInfo.normalizedTime>=1.0f)
			{
				m_ani.SetBool("idle",true);
				m_timer = 2;
			}
		}
	}

	void MoveTo(){
		float speed = m_movSpeed * Time.deltaTime;
		m_agent.Move (m_transform.TransformDirection ((new Vector3 (0, 0, speed))));
	}

	void RotateTo(){
		//当前角度
		Vector3 oldangle = m_transform.eulerAngles;
		//获得面向主角的速度
		m_transform.LookAt (m_player.m_transform);
		float target = m_transform.eulerAngles.y;
		//转向主角
		float speed = m_rotSpeed * Time.deltaTime;
		float angle = Mathf.MoveTowardsAngle (oldangle.y, target, speed);
		m_transform.eulerAngles = new Vector3 (0, angle, 0);
	}
}
