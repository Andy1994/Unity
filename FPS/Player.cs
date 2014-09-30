using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	//组件
	public Transform m_transform;
	CharacterController m_ch;
	Transform m_camTransform;

	//摄像机旋转角度
	Vector3 m_camRot;
	float m_camHeight = 1.4f;

	//角色移动速度
	float m_movSpeed = 3.0f;

	//重力
	float m_gravity = 2.0f;

	//生命值
	public int m_life = 5;

	void Start(){
		m_transform = this.transform;
		m_ch = this.GetComponent<CharacterController> ();

		//获取摄像机
		m_camTransform = Camera.main.transform;
		Vector3 pos = m_transform.position;
		pos.y += m_camHeight;
		m_camTransform.position = pos;
		m_camTransform.rotation = m_transform.rotation;
		m_camRot = m_camTransform.eulerAngles;

		Screen.lockCursor = true;
	}

	void Update(){
		if (m_life <= 0)//生命为0，什么都不做
			return;
		Control();
	}

	void Control(){
		//获取鼠标移动距离
		float rh = Input.GetAxis("Mouse X");
		float rv = Input.GetAxis("Mouse Y");
		//旋转摄像机
		m_camRot.x -= rv;
		m_camRot.y += rh;
		m_camTransform.eulerAngles = m_camRot;
		//使主角的面向方向与摄像机一致
		Vector3 camrot = m_camTransform.eulerAngles;
		camrot.x = 0;
		camrot.z = 0;
		m_transform.eulerAngles = camrot;

		float xm = 0, ym = 0, zm = 0;

		//重力运动
		ym -= m_gravity * Time.deltaTime;

		//上下左右运动
		if (Input.GetKey (KeyCode.W)) {
			zm += m_movSpeed * Time.deltaTime;	
		}
		else if(Input.GetKey(KeyCode.S)){
			zm -= m_movSpeed * Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.A)) {
			xm -= m_movSpeed * Time.deltaTime;	
		}
		else if(Input.GetKey(KeyCode.D)){
			xm += m_movSpeed * Time.deltaTime;
		}

		m_ch.Move (m_transform.TransformDirection (new Vector3 (xm, ym, zm)));

		//使摄像机的位置与主角一致
		Vector3 pos = m_transform.position;
		pos.y += m_camHeight;
		m_camTransform.position = pos;
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawIcon (this.transform.position,"Spawn.tif");
	}
}
