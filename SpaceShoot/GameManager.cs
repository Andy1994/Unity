using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

	public int m_score = 0;//得分
	
	public static int m_history = 0;//最高记录

	protected Player m_player;//主角

	public AudioClip m_musicClip;//背景音乐

	protected AudioSource m_Audio;//声音源

	void Awake(){
		Instance = this;
	}

	void Start () {
		//m_Audio = this.audio;
		GameObject obj = GameObject.FindGameObjectWithTag("Player");
		if (obj != null) {
			m_player = obj.GetComponent<Player>();		
		}
	}

	void Update () {
		//循环播放背景音乐
		//if(!m_Audio.isPlaying){
			//m_Audio.clip = m_musicClip;
			//m_Audio.Play();
		//}

		//暂停游戏
		if(Time.timeScale>0&&Input.GetKeyDown(KeyCode.Escape)){
			Time.timeScale = 0;
		}
	}

	void OnGUI()
	{
		//游戏暂停
		if (Time.timeScale == 0) {
			//继续游戏按钮
			if (GUI.Button (new Rect (Screen.width * 0.5f - 50, Screen.height * 0.4f, 100, 30), "继续游戏")) {
				Time.timeScale = 1;
			}

			if (GUI.Button (new Rect (Screen.width * 0.5f - 50, Screen.height * 0.6f, 100, 30), "退出游戏")) {
				Application.Quit ();
			}
		}

		int life = 0;
		if (m_player != null) {
			//获取主角生命
			life = (int)m_player.m_life;	
		}
		else //game over
		{
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
		
			GUI.skin.label.fontSize = 15;
			//显示主角生命
			GUI.Label(new Rect(5,5,100,30),"装甲："+life);
			//显示最高分
			GUI.skin.label.alignment = TextAnchor.LowerCenter;
			GUI.Label(new Rect(0,5,Screen.width,30),"记录"+m_history);
			//显示当前得分
			GUI.Label(new Rect(0,25,Screen.width,30),"得分"+m_score);
	}

	public void AddScore(int point)
	{
		m_score += point;
		//更新高分记录
		if (m_history < m_score) {
			m_history = m_score;		
		}
	}

}
