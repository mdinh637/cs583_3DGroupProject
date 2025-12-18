using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text currentTimeText;
    [SerializeField] private TMP_Text bestTimeText;

    private float currentTime; //track time
    private float bestTime; //keep best time

    private bool isRunning = true;

    private void Start()
    {
        //load best time from previous runs
        bestTime = PlayerPrefs.GetFloat("BestTime", 0f);
        UpdateBestTimeUI();
    }

    private void Update()
    {
        if (!isRunning)
            return;

        currentTime += Time.deltaTime;
        UpdateCurrentTimeUI();
    }

    //call this when the game ends
    public void StopTimer()
    {
        isRunning = false;

        //check if new best time
        if (currentTime > bestTime)
        {
            bestTime = currentTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
            PlayerPrefs.Save();
        }

        UpdateBestTimeUI();
    }

    private void UpdateCurrentTimeUI()
    {
        currentTimeText.text = $"Time: {currentTime:F1}s";
    }

    private void UpdateBestTimeUI()
    {
        bestTimeText.text = $"Best: {bestTime:F1}s";
    }
}
