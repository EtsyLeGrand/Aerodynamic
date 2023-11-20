using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayCanvasManager : MonoBehaviour
{
    [SerializeField] private Text failAmountField;
    [SerializeField] private Text timeField;

    [SerializeField] private GameObject winPopup;
    [SerializeField] private Text initialPoints;
    [SerializeField] private Text pointsTimesFailed;
    [SerializeField] private Text pointsTimeLeft;
    [SerializeField] private Text pointsTotal;

    [SerializeField] private int initialPointsOfLevel;
    [SerializeField] private int pointsLostPerFail;
    [SerializeField] private int pointsLostPer30s;


    private float currentTime = 0.0f;
    private int failAmount;

    private void Start()
    {
        timeField.text = "0";
        failAmountField.text = "0";
        EventManager.StartListening("PlayerFail", IncreaseFailAmount);
        EventManager.StartListening("PlayerWin", OnLevelEnded);
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        timeField.text = FormatTime(currentTime);

        if (Input.GetKeyDown(KeyCode.R)) { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
    }

    private string FormatTime(float s) // Pas optimal de caller dans l'update mais j'ai pas le temps de changer pour l'instant
    {
        TimeSpan span = TimeSpan.FromSeconds(s);
        return string.Format("{0:D2}:{1:D2}",
                        span.Minutes,
                        span.Seconds);
    }

    private void IncreaseFailAmount(Dictionary<string, object> _)
    {
        failAmount++;
        failAmountField.text = failAmount.ToString();
    }

    private void OnLevelEnded(Dictionary<string, object> _)
    {
        winPopup.SetActive(true);
        
        int passed30Secs = (int)(currentTime / 30.0f);

        initialPoints.text = "Initial points : " + initialPointsOfLevel;
        pointsTimesFailed.text = "Times failed : -" + (failAmount * pointsLostPerFail);
        pointsTimeLeft.text = "Time: : -" + (passed30Secs * pointsLostPer30s);
        pointsTotal.text = "Total: " + (initialPointsOfLevel - (failAmount * pointsLostPerFail) - (passed30Secs * pointsLostPer30s)).ToString();
        Time.timeScale = 0.0f;
    }

    public void OnBackToMenuButtonClicked()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Title");
    }

    private void OnDestroy()
    {
        EventManager.StopListening("PlayerFail", IncreaseFailAmount);
        EventManager.StopListening("PlayerWin", OnLevelEnded);
    }
}
