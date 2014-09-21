using UnityEngine;
using System.Collections;
[AddComponentMenu("MyGame/Player")]

public class Player : MonoBehaviour {

	public float m_speed = 1;
	protected Transform m_transform;
	public Transform m_rocket;
	float m_rocketRate = 0;

	// Use this for initialization
	void Start () {
		m_transform = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
		float movev = 0;//纵向移动
		float moveh = 0;//横向移动

		if (Input.GetKey (KeyCode.UpArrow)) {
			movev+=m_speed*Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			movev-=m_speed*Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			moveh-=m_speed*Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			moveh+=m_speed*Time.deltaTime;
		}

		this.m_transform.Translate (new Vector3 (moveh, 0, movev));

		m_rocketRate -= Time.deltaTime;
		if (m_rocketRate <= 0) {
			m_rocketRate=0.1f;
			if (Input.GetKey (KeyCode.Space) || Input.GetMouseButton (0)) {
				Instantiate (m_rocket,m_transform.position,m_transform.rotation);
			}
		}
	}

	public float m_life = 3;
	void OnTriggerEnter(Collider other)
	{
		if (other.tag.CompareTo ("PlayerRocket") != 0) 
		{
			m_life-=1;
			if(m_life<=0)
			{
				Destroy(this.gameObject);
			}
		}
	}
}
