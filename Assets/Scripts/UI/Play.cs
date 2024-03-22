using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("LevelSelect");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
