using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondCountDown : MonoBehaviour
{
	private int counter = 5;
	private Text number;
	// Use this for initialization
	void Start ()
	{
		number = transform.GetChild(0).GetComponent<Text>();
		
	}

	public void startCount()
	{
		counter = 5;
		
	}

	public void Down()
	{
		counter--;
		number.text = counter + ""; 
		if (counter == 0)
		{
			Finished();
		}
		
	}

	private void Finished()
	{
		GameController gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		gm.MenuPressed();
	}
}
