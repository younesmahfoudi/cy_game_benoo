using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager: MonoBehaviour
{
	public Text killCounterText;
	public Animator killCounterAnim;
	public Text gemCounterText;
	public Animator gemCounterAnim;
	public Text timerText;
	private int _killCounter = 0;
	private int _gemCounter = 0;
	private int _seconds = 0;
	private int _minutes = 0;

	public void Init(int gems, int timerMinutes, int timerSeconds)
    {
		_gemCounter = gems;
		_minutes = timerMinutes;
		_seconds = timerSeconds;

		gemCounterAnim.ResetTrigger("Shake");
		killCounterAnim.ResetTrigger("Shake");
		killCounterText.text = "0";
		gemCounterText.text = _gemCounter.ToString();
		UpdateTimer();
	}

	public void UpdateTimer()
    {
		string secondsStr = _seconds.ToString();
		string minutesStr = _minutes.ToString();

		if (_seconds / 10 == 0)
			secondsStr = "0" + _seconds.ToString();
		if (_minutes / 10 == 0)
			minutesStr = "0" + _minutes.ToString();
		timerText.text = minutesStr + ":" + secondsStr;
	}

	//Return true if timer is over
	public bool DecreaseTimer()
    {
		if (_minutes == 0 && _seconds == 0)
			return true;
		_seconds -= 1;
		if (_minutes == 0 && _seconds == 0)
		{
			UpdateTimer();
			return true;
		}

		if (_seconds < 0)
        {
			_minutes -= 1;
			_seconds = 59;
        }

		UpdateTimer();
		return false;
    }

	public void IncreaseKillCounter()
    {
		killCounterAnim.SetTrigger("Shake");
		_killCounter++;
		killCounterText.text = _killCounter.ToString();
	}

	public void IncreaseGemCounter()
	{
		gemCounterAnim.SetTrigger("Shake");
		_gemCounter++;
		gemCounterText.text = _gemCounter.ToString();
	}

	public int GetGems()
    {
		return _gemCounter;
    }

	public int GetScore()
    {
		return _killCounter;
    }
}
