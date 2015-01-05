using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Text TimerText;
    public float TargetTime = 60.0f;
	private float _time = 0;

    private FMOD.Studio.ParameterInstance _timeParam;
    private FMOD.Studio.ParameterInstance _locationParam;
    private FMOD.Studio.ParameterInstance _fightModeParam;

	// Use this for initialization
	void Start () {
        FMOD_StudioEventEmitter[] emitters = GetComponents<FMOD_StudioEventEmitter>();
        for (int i = 0; i < emitters.Length; i++) {
            emitters[i].Play();
            FMOD.Studio.ParameterInstance param = emitters[i].getParameter("location");
            if (param != null) {
                _locationParam = param;
                _locationParam.setValue(1.0f);
            }
            param = emitters[i].getParameter("timeleft");
            if (param != null) {
                _timeParam = param;
                _timeParam.setValue(1.0f);
            }
            param = emitters[i].getParameter("combat");
            if (param != null) {
                _fightModeParam = param;
                _fightModeParam.setValue(0.0f);
            }
            emitters[i].Play();
        }
	}
	
	// Update is called once per frame
	void Update () {
		_time += Time.deltaTime;
		TimerText.text = "Temps : " + Mathf.Floor(_time / 60f) + "m " + Mathf.Floor(_time - Mathf.Floor(_time / 60f)*60f) + "s";
        if (_timeParam != null) {
            _timeParam.setValue(Mathf.Max(0.0f, (TargetTime - _time) / TargetTime));

            if (((TargetTime - _time) / TargetTime) < 0.1) {
                TimerText.color = new Color(1.0f, 0.0f, 0.0f);
            } else if (((TargetTime - _time) / TargetTime) < 0.3) {
                TimerText.color = new Color(0.88f, 0.66f, 0.14f);
            } else if (((TargetTime - _time) / TargetTime) < 0.5) {
                TimerText.color = new Color(1.0f, 1.0f, 0.0f);
            } else {
                TimerText.color = new Color(0.0f, 1.0f, 0.0f);
            }
        }
	}

    public void setLocation(int location) {
        if (_locationParam != null) {
            _locationParam.setValue((float)location);
        }
    }

    public void setFightMode(bool fightMode) {
        if (_fightModeParam != null) {
            if (fightMode) {
                _fightModeParam.setValue(1.0f);
            } else {
                _fightModeParam.setValue(0.0f);
            }
        }
    }
}
