using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.Assertions.Must;

public class BallVisual : MonoBehaviour
{
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private Light2D light2D;
    [SerializeField] AudioSource audioSrc;
    [SerializeField] AudioClip audioJump;
    [SerializeField] AudioClip audioGameOver;

    Vector3 targetScaleX = new Vector3(.4f, .1f, 0);
    Vector3 orginalScale;

    private float duration = 2f;


    private void Start()
    {
        light2D.enabled = false;
        gameOverText.enabled = false;
        orginalScale = transform.localScale;
        BallController.instance.OnDirectionChanged += BallController_OnDirectionChanged;
        BallController.instance.OnPlayerHitGround += BallController_OnPlayerHitGround;
        BallController.instance.OnGameOver += OnGameOver;
    }

    private void OnGameOver(object sender, System.EventArgs e)
    {
        light2D.enabled = true;
        gameOverText.enabled = true;
        Color color = gameOverText.color;
        color.a = 0;
        gameOverText.color = color;
        gameOverText.DOFade(1, duration);
        audioSrc.PlayOneShot(audioGameOver);
        Invoke("ReturnMainMenu", 2);
        
    }

    private void ReturnMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
        DOTween.KillAll();

    }


    private void BallController_OnPlayerHitGround(object sender, System.EventArgs e)
    {
        gameObject.transform.DOScale(targetScaleX, .1f).OnComplete(() =>
        {
            gameObject.transform.DOScale(orginalScale, .1f);
        });
        audioSrc.PlayOneShot(audioJump);
    }

    private void BallController_OnDirectionChanged(object sender, int e)
    {
        gameObject.GetComponent<SpriteRenderer>().flipX = e == 1 ? false : true;
    }

}
