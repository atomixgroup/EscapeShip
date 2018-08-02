using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public GameObject quitMenuUI;
    public GameObject scoreBoard;
    public GameObject playText;
    public static bool isMuted = false;
    public Image muteButton;
    public Sprite muted;
    public Sprite soundOn;
    private bool isOnQuitMenu = false;

    public void ChangeScene ()
    {
        ScoreCounter.enable = true;
        AdsController.allow = true;
        Asteroid.isPlay = true;
        SceneManager.LoadScene("Main");
    }

	void Start()
	{
        isMuted = ObscuredPrefs.GetBool("isMuted");
        int max = ObscuredPrefs.GetInt("maxScore");
        if (GameController.score > 0)
        {
            scoreBoard.GetComponent<Text>().text = GameController.score.ToString() + "\nHIGH SCORE: " + max.ToString();
        }
        else
        {
            scoreBoard.GetComponent<Text>().text = "HIGH SCORE\n" + max.ToString();
        }

        if (isMuted)
        {
            muteButton.sprite = muted;
        }
        else
        {
            muteButton.sprite = soundOn;
        }
        quitMenuUI.SetActive(false);
	}

    public void MutePressed()
    {
        if (isMuted)
        {
            isMuted = false;
            ObscuredPrefs.SetBool("isMuted", false);
            muteButton.sprite = soundOn;
        }
        else
        {
            isMuted = true;
            ObscuredPrefs.SetBool("isMuted", true);
            muteButton.sprite = muted;
        }
    }

    private void Awake()
    {
        Application.targetFrameRate = 120;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOnQuitMenu)
            {
                KeepPlaying();
                isOnQuitMenu = false;
            }
            else
            {
                quitMenuUI.SetActive(true);
                scoreBoard.SetActive(false);
                playText.SetActive(false);
                muteButton.enabled = false;
                isOnQuitMenu = true;
            }
        }
    }

    public void KeepPlaying()
    {
        quitMenuUI.SetActive(false);
        scoreBoard.SetActive(true);
        playText.SetActive(true);
        muteButton.enabled = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}