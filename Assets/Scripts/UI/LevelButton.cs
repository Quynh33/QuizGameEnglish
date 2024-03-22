using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [Header("Active Stuff")]
    public bool isActive;
    public Sprite activeSprite;
    public Sprite lockedSprite;
    private Image buttonImage;
    private Button myButton;
    public Image[] stars;
    public TextMeshProUGUI levelText;
    public int level;
    public GameObject confirmPanel;
    private GameData gameData;
    private int starsActive;
    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        buttonImage = GetComponent<Image>();
        myButton = GetComponent<Button>();
        LoadData();
        ActivateStars();
        ShowLevel();
        DecideSprite();
        CheckEnabledStars();
          
    }
    void LoadData()
    {
        if (gameData.saveData.isActive[level-1])
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }
            starsActive = gameData.saveData.stars[level - 1];

    }
    void CheckEnabledStars()
    {
        for (int i = 0; i < stars.Length; i++)
        {

            bool isStarEnabled = stars[i].enabled;
        }
    }
    void ActivateStars()
    {
        for (int i = 0; i < starsActive; i++)
        {
            stars[i].enabled = true;
        }
    }
    private void DecideSprite()
    {
        if (isActive)
        {
            buttonImage.sprite = activeSprite;
            myButton.enabled = true;
            levelText.enabled = true;
        }
        else
        {
            buttonImage.sprite = lockedSprite;
            myButton.enabled = false;
            levelText.enabled = false;
        }
    }

    void ShowLevel()
    {
        levelText.text = "" + level;
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void ConfirmPanel(int level)
    {
        //Lấy giá trị từ PlayerPrefs
        int selectedLevel = PlayerPrefs.GetInt("Current Level", level-1);

        Debug.Log("Selected Level: " + selectedLevel);
        // Đảm bảo confirmPanel đã được gán giá trị
        if (confirmPanel != null)
        {
            confirmPanel.GetComponent<ConfirmPanel>().level = level;
            confirmPanel.SetActive(true);
        }
    }
}