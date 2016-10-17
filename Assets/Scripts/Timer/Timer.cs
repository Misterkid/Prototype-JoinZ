/*
Author: Eduard Meivogel
Website: http://eddymeivogel.com
Contact: Eddymeivogel@gmail.com
*/

using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour 
{
	private float _time;
	private bool doTimer = false;
	
	private float _countDownTime;
	
	private float _countTickTime;
	private float _countTickAddedTime;
	private float _tickedAmount = 1;
	
	private string _TimerEndEventName = "DoTimerEndEvent";
	private string _TimerTickEventName = "DoTimeTickEvent";
	void Start()
	{
		_time = Time.deltaTime;
	}
	
	void Update()
	{
		if(doTimer)
		{
			_time =  _time + Time.deltaTime;
			
			if(_time > _countDownTime)
			{
				doTimer = false;
                this.gameObject.SendMessage(_TimerEndEventName);
			}
			if(_time > _countTickAddedTime && _countTickAddedTime != 0)
			{
				_tickedAmount ++;
				_countTickAddedTime = _countTickTime * _tickedAmount;
				this.gameObject.SendMessage(_TimerTickEventName);
			}
		}
	}
	
	public void StartTimer(float timeInSeconds,float tick = 0,string TimerEndFunc = "DoTimerEndEvent", string TimerTickFunc = "DoTimeTickEvent")
	{
		doTimer = true;
		_time = Time.deltaTime;
		_countDownTime = timeInSeconds;
		_countTickTime = tick;
		_countTickAddedTime = _countTickTime;
		_TimerEndEventName = TimerEndFunc;
		_TimerTickEventName = TimerTickFunc;
	}

	public void ResetTimer()
	{
		_time = Time.deltaTime;
	}
	
	public float GetTime()
	{
		return Mathf.Round(_time);	
	}
	
	public void StopTimer()
	{
		doTimer = false;
		
	}
	public bool IsStarted()
	{
		return doTimer;
	}
}
