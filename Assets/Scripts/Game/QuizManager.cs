using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private QuizUI quizUI;
    private List<Question> questions;
    private Question selectedQuestion;
    private GameStatus gameStatus = GameStatus.Next;
    private GameStatus GameStatus { get { return gameStatus; } }
    private List<Question> remainingQuestions;
    [SerializeField] private float timeLimit = 60f;
    public ScoreManager scoreManager;
    private float currentTime;
    public int level;
    [SerializeField] private QuizDataScriptable quizDataScriptable;
    private bool allQuestionsDisplayed = false;
    public void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        currentTime = timeLimit;
        gameStatus = GameStatus.Playing;

        level = PlayerPrefs.GetInt("Current Level", 0);
        if (level <= 0)
        {
            PlayerPrefs.SetInt("Current Level", level);
        }
        LoadQuestionsForCurrentLevel(level);

        // Gọi phương thức lưu điểm số sau mỗi cấp độ
    }
    private void Update()
    {
        if (gameStatus == GameStatus.Playing)
        {
            currentTime -= Time.deltaTime;
            SetTimer(currentTime);
        }
        if (AreAllQuestionsDisplayed())
        {
            // Gọi hàm SaveScore trước để đảm bảo dữ liệu được lưu
            scoreManager.SaveScore();

            // Gọi hàm UpdateStars để cập nhật số sao
            scoreManager.UpdateStars();
        }
    }
    void SelectQuestion()
    {
        if (remainingQuestions.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, remainingQuestions.Count);
            selectedQuestion = remainingQuestions[index];
            quizUI.SetQuestion(selectedQuestion);
            remainingQuestions.RemoveAt(index);
        }
        else
        {
            Debug.LogWarning("Tất cả các câu hỏi đã được gọi");
            // Kiểm tra xem đã hiển thị tất cả câu hỏi chưa
            if (!allQuestionsDisplayed)
            {
                allQuestionsDisplayed = true;

                // Gọi đối tượng QuizUI để hiển thị LevelComplete
                quizUI.ShowLevelComplete();
            }
        }
    }
    public bool AreAllQuestionsDisplayed()
    {
        return allQuestionsDisplayed;
    }
    public bool Answer(string answered)
    {
        bool correctAns = false;
        if (answered == selectedQuestion.correctAns)
        {
            correctAns = true;
            scoreManager.Score += 20;
            quizUI.ScoreText.text = scoreManager.Score.ToString();
        }
        Invoke("SelectQuestion", 0.4f);
        return correctAns;

    }
    // Hàm để load câu hỏi từ tệp JSON
    private void SetTimer(float value)
    {
        TimeSpan time = TimeSpan.FromSeconds(value);
        quizUI.TimerText.text = time.ToString(@"mm\:ss");
    }

    void LoadQuestionsForCurrentLevel(int level)
    {
        // Rest of your code to load questions for the specified level
        QuizDataScriptable quizDataScriptable = Resources.Load<QuizDataScriptable>("QuizData/Level" + level.ToString());

        if (quizDataScriptable != null)
        {
            questions = quizDataScriptable.questions;
            remainingQuestions = new List<Question>(questions);
            SelectQuestion();
        }
        else
        {
            Debug.LogError("Không thể tìm thấy QuizDataScriptable cho Level " + level);
        }
    }


}
[System.Serializable]
public class Question
{
    public string questionInfor;
    public QuestionType questionType;
    public Sprite questionImage;
    public List<string> options;
    public string correctAns;
    public AudioClip qustionClip;         
    public UnityEngine.Video.VideoClip qustionVideo;
}

[System.Serializable]
public enum QuestionType
{
    TEXT,
    IMAGE,
    VIDEO,
    AUDIO
}
[System.Serializable]
public enum GameStatus
{
    Next,
    Playing
}