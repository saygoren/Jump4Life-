using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    [SerializeField]
    Transform gameOverBorder;

    Rigidbody2D rb;

    private float moveHorizontal;
    private float speed = 20f;
    private int dir = 0;

    public event EventHandler<int> OnDirectionChanged;
    public event EventHandler OnPlayerHitGround;
    public event EventHandler OnGameOver;
    public static BallController instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 2;
    }

    private void FixedUpdate()
    {
        movement();
    }

    void movement() // hareket fonksiyonu
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        resolveDir(moveHorizontal);
        rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);
    }

    void resolveDir(float horizontal)
    {
        if (horizontal > 0 && dir != 1)
        {
            dir = 1;
            OnDirectionChanged?.Invoke(this, dir);
        }
        else if (horizontal < 0 && dir != -1)
        {
            dir = -1;
            OnDirectionChanged?.Invoke(this, dir);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y > 0)
        {
            if (collision.gameObject.CompareTag("ClonePlatform"))
            {
                gameOverBorder.transform.position = new Vector3(0f, this.transform.position.y - 20, 0f);
                OnPlayerHitGround?.Invoke(this, EventArgs.Empty);

            }
        }
        if (collision.gameObject.CompareTag("GameOver"))
        {
            OnGameOver?.Invoke(this, EventArgs.Empty);
        }

    }
}
