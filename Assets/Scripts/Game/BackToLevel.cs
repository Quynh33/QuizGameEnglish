using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToLevel : MonoBehaviour
{
    public string sceneToLoad;
    private GameData gameData;
    private QuizManager quizManager;
    public void NextScene()
    {
            if (gameData != null)
            {
                gameData.saveData.isActive[quizManager.level + 1] = true;
                gameData.Save();
            }
        SceneManager.LoadScene(sceneToLoad);
    }
        void Start()
    {
        gameData = FindObjectOfType<GameData>();
        quizManager = FindObjectOfType<QuizManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}