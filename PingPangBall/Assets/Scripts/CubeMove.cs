using UnityEngine;
using System.Collections;

public class CubeMove : MonoBehaviour {
	
	public float m_speed = 10;
	protected Transform m_transform;
	public int startFlag = 0;
	float moveh = 0;

	void Start () {
		m_transform = this.transform;
	}

	void Update () {
		moveh = 0;
		if (Input.GetKey (KeyCode.LeftArrow)) {
			if(startFlag == 0)
				startFlag = 1;
			moveh -= m_speed * Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			if(startFlag == 0)
				startFlag = 1;
			moveh += m_speed * Time.deltaTime;
		}
		if(m_transform.position.x<-8 && moveh>0)
			this.m_transform.Translate (new Vector3 (moveh, 0, 0));
		if(m_transform.position.x>8 && moveh<0)
			this.m_transform.Translate (new Vector3 (moveh, 0, 0));
		if(m_transform.position.x>=-8 && m_transform.position.x<=8)
			this.m_transform.Translate (new Vector3 (moveh, 0, 0));
	}

	void OnGUI()
	{
	}
}
