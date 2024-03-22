using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class QuizUI : MonoBehaviour
{
    [SerializeField] private QuizManager quizManager;
    [SerializeField] private TextMeshProUGUI questionText, scoreText, timerText;
    [SerializeField] private Image questionImage;
    [SerializeField] private AudioSource questionAudio;
    [SerializeField] private List<Button> options;
    [SerializeField] private Color correctCOl, wrongCOl, normalCol;
    [SerializeField] private UnityEngine.Video.VideoPlayer questionVideo;
    public GameObject LevelComplete;
    private Question question;
    private bool answered;
    private float audioLength;
    public AudioSource audioSource; // Kéo và thả AudioSource từ Inspector vào đây
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public TextMeshProUGUI ScoreText { get { return scoreText; } }
    public TextMeshProUGUI TimerText { get { return timerText; } }

    void Awake()
    {
        for (int i = 0; i < options.Count; i++)
        {
            Button localBtn = options[i];
            localBtn.onClick.AddListener(() => OnClick(localBtn));
        }
    }

    public void SetQuestion(Question question)
    {
        this.question = question;
        switch (question.questionType)
        {
            case QuestionType.TEXT:
                ImageHolder();
                questionImage.transform.parent.gameObject.SetActive(false);
                break;
            case QuestionType.IMAGE:
                ImageHolder();
                questionImage.transform.gameObject.SetActive(true);
                questionImage.sprite = question.questionImage;
                break;
            case QuestionType.AUDIO:
                questionVideo.transform.parent.gameObject.SetActive(true);  
                questionVideo.transform.gameObject.SetActive(false);        
                questionImage.transform.gameObject.SetActive(false);          
                questionAudio.transform.gameObject.SetActive(true);         

                audioLength = question.qustionClip.length;                    
                StartCoroutine(PlayAudio());                                
                break;
        }
        questionText.text = question.questionInfor;
        // trộn các câu trả lời
        List<string> answerList = ShuffleList.ShuffleListItems<string>(question.options);
        for (int i = 0; i < options.Count; i++)
        {
            options[i].GetComponentInChildren<TextMeshProUGUI>().text = answerList[i];
            options[i].name = answerList[i];
            options[i].image.color = normalCol;
        }

        answered = false;
    }
    void ImageHolder()
    {
        questionImage.transform.parent.gameObject.SetActive(true);
        questionImage.transform.gameObject.SetActive(false);
        questionAudio.transform .gameObject.SetActive(false);
        questionVideo.transform .gameObject.SetActive(false);
    }
    IEnumerator PlayAudio()
    {
        // Nếu questionType là audio
        if (question.questionType == QuestionType.AUDIO)
        {
            // Chỉ phát audio khi LevelComplete không hoạt động
            if (!LevelComplete.activeSelf)
            {
                // PlayOneShot
                questionAudio.PlayOneShot(question.qustionClip);
                // Đợi một vài giây
                yield return new WaitForSeconds(audioLength + 0.5f);
                // Play lại audio
                StartCoroutine(PlayAudio());
            }
            else
            {
                // Nếu LevelComplete hoạt động, dừng phát audio
                StopCoroutine(PlayAudio());
            }
        }
        else // Nếu questionType không phải là audio
        {
            // Dừng Coroutine
            StopCoroutine(PlayAudio());
            // Trả về null
            yield return null;
        }
    }
    private void OnClick(Button btn)
    {
        bool val = false;

        if (!answered)
        {
            answered = true;
            val = quizManager.Answer(btn.name);
        }

        if (val)
        {
            btn.image.color = correctCOl;
            // Phát âm thanh khi câu hỏi đúng
            audioSource.PlayOneShot(correctSound);
        }
        else
        {
            btn.image.color = wrongCOl;
            // Phát âm thanh khi câu hỏi sai
            audioSource.PlayOneShot(wrongSound);
        }

    }
    public void ShowLevelComplete()
    {
        LevelComplete.SetActive(true);
    }
}