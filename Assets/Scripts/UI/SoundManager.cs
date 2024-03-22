
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    [SerializeField] private AudioSource musicSource;
    public AudioClip background;

    private void Awake()
    {
        if (instance == null)
        {
            // Nếu chưa có instance, thì đặt nó là chính nó
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Nếu đã có instance, phá hủy đối tượng này
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Chỉ chơi nhạc background nếu đây là instance duy nhất
        if (instance == this)
        {
            PlayBackgroundMusic();
        }
    }

    private void PlayBackgroundMusic()
    {
        if (background != null)
        {
            musicSource.clip = background;
            musicSource.Play();
        }
    }
}