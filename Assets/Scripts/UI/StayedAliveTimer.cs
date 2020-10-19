using UnityEngine;
using TMPro;

public class StayedAliveTimer : MonoBehaviour
{
	static int sixty = 60; //no idea what to name this variable, remnant from babylon, I think, and the reason time formats are in base 60 instead of base 10, if I remember correctly.
	private float hundredths;
	private int seconds;
	private int minutes;
	private int hours;
	string separator = ":";
	private float timer;
	public TextMeshProUGUI timeAlive;
	


	void Update()
    {
        if (GameState.GameOver) return;

		hundredths += Time.deltaTime;
		float convertToTwoDecimals = hundredths * 100;
		int displayDecimals = (int)convertToTwoDecimals;
		if (hundredths > 1)
		{
			seconds += (int)hundredths;
			hundredths = 0;
		}
		if (seconds >= sixty) 
		{
			timer = 0;
			seconds = 0;
			minutes++;
		}
		if (minutes >= sixty)
		{
			hours++;
			minutes = 0;
		}
		timeAlive.text = hours + separator + minutes + separator + seconds + "." + displayDecimals;
	}
	public void ResetCounter()
	{
		hundredths = 0;
		seconds = 0;
		minutes = 0;
		hours = 0;
	}
}
