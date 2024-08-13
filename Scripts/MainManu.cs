using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class MainManu : MonoBehaviour
{
    [SerializeField] private TMP_Text playText;
    [SerializeField] private AudioSource audioSrc;
    [SerializeField] private AudioClip buttonAudio;
    [SerializeField] private AudioClip mainManuAudio;

    private float duration = 0.5f;

    private void Start()
    {
        ButtonEffect();
        audioSrc.clip = mainManuAudio;
        audioSrc.Play();
    }

    public void StartGame()
    {
        StartCoroutine(SceneTime());    
    }
  
    public void QuitGame()
    {
        Application.Quit();     
    }

    private void ButtonEffect()
    {
        if ( playText != null )
        {
            playText.DOFade(0, duration).SetLoops(-1, LoopType.Yoyo);
        }
       
    }

    public void ButtonClickSound()
    {
        audioSrc.PlayOneShot(buttonAudio);
    }

    IEnumerator SceneTime()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadSceneAsync(1);
    }
}
