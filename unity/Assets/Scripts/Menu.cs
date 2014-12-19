using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

	public Text bestTimeText;
	// Use this for initialization
	void Start () {
		for (var i = 1; i <= 5; i++) {
			if (!PlayerPrefs.HasKey("bestTimeLevel" + i)) {
				PlayerPrefs.SetFloat("bestTimeLevel"+i, 0f);
			}
		}

		var str = "";

		var total = 0f;
		for (var i = 1; i <= 5; i++) {
			str += "Niveau " + i + " ";
			var time = PlayerPrefs.GetFloat("bestTimeLevel" + i);
			total += time;
			str += Mathf.Floor(time / 60f) + "m " + Mathf.Floor(time - Mathf.Floor(time / 60f) * 60f) + "s\n";

		}
		str += "Total ";
		str += Mathf.Floor(total / 60f) + "m " + Mathf.Floor(total - Mathf.Floor(total / 60f) * 60f) + "s";

		bestTimeText.text = str;
	}

	public void StartGame() {
		Application.LoadLevel(1);
	}

}
