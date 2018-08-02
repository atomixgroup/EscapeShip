using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public bool ended = false;
    public bool con = false;
    private float endTime = 2f;
    public static int score;
    public Text scoreText;
    public static int max;
    public GameObject ship;
    public AudioSource explosion;
    public AudioSource backgroundMusic;
    public static bool adSeen = true;
    public static bool respawnEverything = false;
    public Button pauseButton;
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
	public GameObject waitMenu;
	public Animator countDownAnimator;
	 
	private void Start()
	{
        score = 0;
        max = ObscuredPrefs.GetInt("maxScore");
        pauseMenuUI.SetActive(false);
	}
    
	private void Update()
	{
        scoreText.text = score.ToString();
		if (ended)
		{
			if (endTime <= 0)
			{
				MenuPressed();
			}
			else
			{
				endTime -= Time.deltaTime;
			}
		}

		if (con)
		{
			if (endTime <= 0)
			{
				Asteroid.isPlay = true;
				con = false;
			}
			else
			{
				endTime -= Time.deltaTime;
			}
		}
        if (Menu.isMuted)
        {
            backgroundMusic.mute = true;
            explosion.mute = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
        	if (isPaused)
        	{
        		Resume();
        	}
        	else
        	{
        		Pause();
        	}
        }
	}

	public void EndGame()
	{
		Asteroid.isPlay = false;
		endTime = 2f;
		ScoreCounter.enable = false;
		if (score > max)
		{
			ObscuredPrefs.SetInt("maxScore", score);
		}
		StartCoroutine(checkInternetConnection((isConnected)=>{
			if (isConnected)
			{
				if (AdsController.allow)
				{
					waitMenu.SetActive(true);
					countDownAnimator.SetTrigger("EnterAnim");
				}
				else
				{
					ended = true;
				}
			}
			else
			{
				ended = true;
			}
		}));
		
		
	  
        explosion.Play();
	    
    }

	private void Awake()
	{
        Application.targetFrameRate = 120;
	}

    public void PausePressed()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    void Pause()
	{
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
	}

    public  void MenuPressed()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ContinueGame()
    {
        ship.SetActive(true);
        ended = false;
        respawnEverything = true;
        endTime = 2f;
        adSeen = false;
	    waitMenu.SetActive(false);
	    Time.timeScale = 1;
	    con = true;
    }
	IEnumerator checkInternetConnection(Action<bool> action){
		WWW www = new WWW("http://google.com");
		yield return www;
		if (www.error != null) {
			action (false);
		} else {
			action (true);
		}
	} 
   
}