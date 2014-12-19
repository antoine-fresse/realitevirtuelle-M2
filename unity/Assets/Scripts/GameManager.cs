using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Text TimerText;
	private float _time = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		_time += Time.deltaTime;
		TimerText.text = "Temps : " + Mathf.Floor(_time / 60f) + "m " + Mathf.Floor(_time - Mathf.Floor(_time / 60f)*60f) + "s";
	}
}
