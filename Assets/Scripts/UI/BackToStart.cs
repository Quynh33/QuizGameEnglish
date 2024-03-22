using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Back : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("GameStart");
    }
}
