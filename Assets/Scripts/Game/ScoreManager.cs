using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private QuizManager quizManager;
    private GameData gameData;
    private int numberStars;
    private int TotalScoreMax = 100;
    public int Score;
    public TextMeshProUGUI ScoreText;
    // Start is called before the first frame update
    void Start()
    {
        quizManager = FindObjectOfType<QuizManager>();
        gameData = FindObjectOfType<GameData>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveScore()
    {
        if (gameData != null)
        {
            int highScore = gameData.saveData.highScores[quizManager.level];
            if (Score > highScore)
            {
                gameData.saveData.highScores[quizManager.level] = Score;
                gameData.Save();
                Debug.Log("High Score saved: " + Score);
            }
        }
    }

    public void UpdateStars()
    {
        // Tính tỷ lệ điểm so với tổng điểm tối đa
        float scorePercentage = (float)Score / TotalScoreMax * 100;

        // Xác định số sao dựa trên tỷ lệ điểm
        if (scorePercentage >= 100)
        {
            numberStars = 3;
        }
        else if (scorePercentage >= 60)
        {
            numberStars = 2;
        }
        else if (scorePercentage >= 20)
        {
            numberStars = 1;
        }
        else
        {
            numberStars = 0;
        }

        // Kiểm tra và cập nhật số sao trong gameData
        if (gameData != null)
        {
            int currentStars = gameData.saveData.stars[quizManager.level];
            if (numberStars > currentStars)
            {
                gameData.saveData.stars[quizManager.level] = numberStars;
                gameData.Save();
            }
        }

        Debug.Log("Stars: " + numberStars);
    }
}
