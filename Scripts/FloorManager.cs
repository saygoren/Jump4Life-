using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using TMPro;

public class FloorManager : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private Transform platformSpawner;
    [SerializeField] private ScoreManager scoreManager;

    private float randomX = 0f;
    private float minX;
    private float maxX;
    private float maxValueY = 2f;
    private float jumpForce = 10f;
    private int chanceScore = 50;
    private static bool spawnDirection = true;
    private Vector3 spawnPosition = new Vector3();
    private float lastPlatformPositionX;
    private float minDistance = 3f;

    private void Start()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();
        lastPlatformPositionX = transform.position.x;
        minX = Random.Range(-7, -8);
        maxX = Random.Range(7, 8);
    }

    IEnumerator SpawnPlatform()
    {
        while (true)
        {
            yield return new WaitForSeconds(0f);
            spawnPosition.y = transform.position.y + maxValueY;
            spawnDirection = !spawnDirection;
            spawnPosition.x = GetRandomXPosition();
            spawnPosition.z = platformSpawner.position.z;
            GameObject newPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            newPlatform.tag = "ClonePlatform";
            lastPlatformPositionX = spawnPosition.x;
            yield return new WaitForSeconds(5);
            Destroy(newPlatform);
         
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y < 0)
        {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 jumpVelocity = rb.velocity;
                jumpVelocity.y = jumpForce;
                rb.velocity = jumpVelocity;
                StartCoroutine(DestroyPlatform());
                StartCoroutine(SpawnPlatform());
            }
        }
    }

    private float GetRandomXPosition()
    {
        do
        {
            if (scoreManager.score >= chanceScore)
            {
                randomX = UnityEngine.Random.Range(-7, 7);
            }
            else
            {
                randomX = UnityEngine.Random.Range(-4, 4);
            }
        }
        while (Mathf.Abs(randomX - lastPlatformPositionX) < minDistance);

        return randomX;
    }

    IEnumerator DestroyPlatform()
    {
        yield return new WaitForSeconds(.1f);
        Destroy(gameObject);
    }
}


