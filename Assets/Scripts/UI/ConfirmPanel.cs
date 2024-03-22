using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ConfirmPanel : MonoBehaviour
{
    public string levelToLoad;
    private int starsActive;
    public int level;
    private GameData gameData;
    public int highScore;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI starText;
    // Start is called before the first frame update
    void OnEnable()
    {
        gameData = FindObjectOfType<GameData>();
        LoadData();
        SetText();
    }
    void LoadData()
    {
        if (gameData != null && gameData.saveData != null)
        {
            starsActive = gameData.saveData.stars[level-1];
            highScore = gameData.saveData.highScores[level-1];

        }
    }
    void SetText()
    {
        highScoreText.text = "" + highScore;
        starText.text = "" + starsActive + "/3";
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void Cancel()
    {
        this.gameObject.SetActive(false);
    }
    public void Play()
    {
        PlayerPrefs.SetInt("Current Level", level - 1 );
        SceneManager.LoadScene(levelToLoad);
    }
}