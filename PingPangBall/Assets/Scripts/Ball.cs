using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
	
	public float m_speed = 5;
	private int m_score = 0;
	protected Transform m_transform;
	private bool isStart = false;
	private bool isFinish = false;
	float move_x = 0, move_y = 0;

	void Start ()
	{
		m_transform = this.transform;
		move_x += m_speed * Time.deltaTime * Random.Range (1, 2);
		move_y += m_speed * Time.deltaTime * Random.Range (1, 2);
	}

	void Update ()
	{
		GameObject obj = GameObject.FindGameObjectWithTag ("Player");
		CubeMove start = obj.GetComponent<CubeMove> ();
		if (start.startFlag == 1)
			isStart = true;

		if (isStart) {
			this.m_transform.Translate (new Vector3 (move_x, move_y, 0));
		}
	}

	void OnGUI()
	{
		GUI.skin.label.fontSize = 20;
		//显示当前得分
		GUI.Label(new Rect(5,1,Screen.width,30),"得分: "+m_score);

		if (isFinish) {
			//放大字体
			GUI.skin.label.fontSize = 50;
			//显示游戏失败
			GUI.skin.label.alignment = TextAnchor.LowerCenter;
			GUI.Label(new Rect(0,Screen.height * 0.2f,Screen.width,60),"游戏失败");
			
			GUI.skin.label.fontSize = 20;
			//显示按钮
			if(GUI.Button(new Rect(Screen.width * 0.5f-50,Screen.height * 0.5f,100,30),"再试一次"))
			{
				//读取当前关卡
				Application.LoadLevel("1");
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag.CompareTo ("Player") == 0) {
			move_y = -move_y;
			move_x = move_x * Random.Range(0.5f,1.3f);
			move_y = move_y * Random.Range(0.85f,1.2f);
			m_score++;
		}
		if (other.tag.CompareTo ("Bound") == 0) {
			move_x = -move_x;
			move_y = move_y * Random.Range(0.85f,1.2f);
		}
		if (other.tag.CompareTo ("Finish") == 0) {
			isFinish = true;
		}
	}
}
