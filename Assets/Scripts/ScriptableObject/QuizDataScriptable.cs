using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuizData", menuName = "QuizData")]
public class QuizDataScriptable : ScriptableObject
{
    // Mảng câu hỏi cho mỗi cấp độ
    public int level;
    public List<Question> questions;
    }
