using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Text pressKeyNotification;
    private string pressKeyNotificationDefaultText = "Press _ to @";

    [SerializeField] private Text currentlyPlaying;
    private string currentlyPlayingDefaultText = "Now playing: _";
    private Animator currentlyPlayingAnimator;

    [SerializeField] private Text currentShownVelocity;
    [SerializeField] private float redVelocityTreshold = 10.0f;

    [SerializeField] private Text timeIndicator;
    private float levelTime = 0.0f;

    [SerializeField] private Text failCountText;
    private Animator failCountAnimator;
    private string defaultFailCountText = "Fails: ";
    private int failCount = 0;

    [Header("WinPopup")]
    [SerializeField] private GameObject winPopup;
    [SerializeField] private int maxTimeScoreValue = 600;
    [SerializeField] private Text winPopupTimeText;
    [SerializeField] private int pointsLostPerFail = 25;
    [SerializeField] private Text winPopupFailsText;
    [SerializeField] private Text winPopupScoreText;

    private void Awake()
    {
        //Time.timeScale = 0.2f;
        Radio radio = FindObjectOfType<Radio>().GetComponent<Radio>();
        radio.onPlayerInRangeOfRadio += ShowPressKeyNotification;
        radio.onPlayerOutOfRangeOfRadio += HidePressKeyNotification;
        radio.onMusicPlay += ShowCurrentPlayingMusic;
        radio.onMusicStop += HideCurrentPlayingMusic;

        PlayerStateMachine player = FindObjectOfType<PlayerStateMachine>().GetComponent<PlayerStateMachine>();
        player.onSendVelocityToUI += UpdateShownVelocity;
        player.onHurt += IncreaseFailCount;

        currentlyPlayingAnimator = currentlyPlaying.gameObject.GetComponent<Animator>();
        failCountAnimator = failCountText.gameObject.GetComponent<Animator>();

        failCountText.text = defaultFailCountText + failCount.ToString();
    }

    private void Update()
    {
        levelTime += Time.deltaTime;
        timeIndicator.text = string.Format("{0:00}", Math.Floor(levelTime / 60)) + ":" 
                            + string.Format("{0:00.0}", levelTime - (Math.Floor(levelTime / 60) * 60)).Replace(',', ':');
    }

    private void ShowPressKeyNotification(KeyCode key, string purpose)
    {
        pressKeyNotification.text = pressKeyNotificationDefaultText;
        pressKeyNotification.text = pressKeyNotification.text.Replace("_", key.ToString());
        pressKeyNotification.text = pressKeyNotification.text.Replace("@", purpose);
        pressKeyNotification.enabled = true;
    }

    private void HidePressKeyNotification()
    {
        pressKeyNotification.text = pressKeyNotificationDefaultText;
        pressKeyNotification.enabled = false;
    }

    private void ShowCurrentPlayingMusic(AudioSource audio)
    {
        currentlyPlaying.text = currentlyPlayingDefaultText;
        currentlyPlaying.text = currentlyPlaying.text.Replace("_", audio.clip.name);
        currentlyPlayingAnimator.Play("FadeIn");
    }

    private void HideCurrentPlayingMusic()
    {
        if (currentlyPlaying.color.a == 1)
        {
            currentlyPlayingAnimator.Play("FadeOut");
        }
        else if (currentlyPlayingAnimator.GetCurrentAnimatorStateInfo(0).IsName("FadeIn"))
        {
            currentlyPlayingAnimator.Play("FadeOut", 0, 1f - currentlyPlaying.color.a);
        }
    }

    private void UpdateShownVelocity(float v)
    {
        if (v >= redVelocityTreshold)
        {
            currentShownVelocity.color = Color.red;
        }
        else
        {
            currentShownVelocity.color = Color.black;
        }
        currentShownVelocity.text = "Speed: " + string.Format("{0:0.0}", v);
    }

    private void IncreaseFailCount()
    {
        failCount++;
        failCountText.text = defaultFailCountText + failCount.ToString();
        failCountAnimator.Play("RedFlash");
    }

    public void OnWinButtonClicked()
    {
        Debug.Log("Clicked");
        winPopupTimeText.text = (maxTimeScoreValue - Math.Floor(levelTime)).ToString();
        winPopupFailsText.text = "-" + (failCount * pointsLostPerFail).ToString();
        winPopupScoreText.text = (maxTimeScoreValue - Math.Floor(levelTime) - (failCount * pointsLostPerFail)).ToString();
        winPopup.SetActive(true);
    }

    public void OnOkWinPopupButtonClicked()
    {
        winPopup.SetActive(false);
    }
}
