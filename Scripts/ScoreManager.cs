using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using UnityEditor.Rendering;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject backGround;
    public int score { get; private set; }
    public CinemachineVirtualCamera virtualCamera;

    private static bool isShaking = false;
    private Vector3 targetRotatePositive1 = new Vector3(0, 0, 10);
    private Vector3 targetRotatePositive2 = new Vector3(0, 0, 20);
    private Vector3 targetRotatePositive3 = new Vector3(0, 0, 30);
    private Vector3 targetRotateNegative1 = new Vector3(0, 0, -10);
    private Vector3 targetRotateNegative2 = new Vector3(0, 0, -20);
    private Vector3 targetRotateNegative3 = new Vector3(0, 0, -30);
    private float killTime;


    private void Start()
    {
        score = 0;
        BallController.instance.OnPlayerHitGround += BallController_OnPlayerHitGround;
    }

    private void Update()
    {
        scoreText.text = score.ToString();
        ShakeCamera();
    }

    private void BallController_OnPlayerHitGround(object sender, System.EventArgs e)
    {
        killTime = 0;
        score++;
    }

    private void ShakeCamera()
    {
        if (score >= 50 && score < 100 && !isShaking)
        {
            StartShake(targetRotatePositive1, targetRotateNegative1);
        }
        else if (score >= 100 && score < 150 && !isShaking)
        {
            StartShake(targetRotatePositive2, targetRotateNegative2);
        }
        else if (score >= 150 && !isShaking)
        {
            StartShake(targetRotatePositive3, targetRotateNegative3);
        }
        killTime += Time.deltaTime;
        if (killTime >= 1)
        {
            virtualCamera.transform.rotation = Quaternion.identity;
        }
    }

    private void StartShake(Vector3 positive, Vector3 negative)
    {
        isShaking = true;
        killTime = 0;
        Sequence shakeSequence = DOTween.Sequence();
        shakeSequence.Append(virtualCamera.transform.DORotate(positive, .3f))
            .Append(virtualCamera.transform.DORotate(negative, .3f))
            .OnComplete(() =>
            {
                isShaking = false;
            });
    }

}
