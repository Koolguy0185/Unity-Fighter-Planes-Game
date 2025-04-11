using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyOnePrefab;
    public GameObject cloudPrefab;
    public GameObject coinPrefab;
    public GameObject powerUpPrefab;
    public GameObject gameOverText;
    public GameObject restartText;
    public GameObject audioPlayer;

    public AudioClip powerupSound;
    public AudioClip powerdownSound;
    public AudioClip coinSound;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI powerupText;

    public float horizontalScreenSize;
    public float verticalScreenSize;

    public int score;
    public int cloudMove;

    private bool gameOver;
    // Start is called before the first frame update
    void Start()
    {
        horizontalScreenSize = 10f;
        verticalScreenSize = 6.5f;
        score = 0;
        cloudMove = 1;
        gameOver = false;
        powerupText.text = "No Power Ups Activated!";
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        CreateSky();
        ChangeScoreText(score);
        InvokeRepeating("CreateEnemy", 1, 3);
        StartCoroutine(SpawnItems());
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void PlaySound(int whichSound)
    {
        switch (whichSound)
        {
            case 1:
                audioPlayer.GetComponent<AudioSource>().PlayOneShot(coinSound);
                break;
            case 2:
                audioPlayer.GetComponent<AudioSource>().PlayOneShot(powerupSound);
                break;
            case 3:
                audioPlayer.GetComponent<AudioSource>().PlayOneShot(powerdownSound);
                break;
        }
    }

    public void ManagePowerUpText(int powerupType)
    {
        switch (powerupType)
        {
            case 1:
                powerupText.text = "Speed!";
                break;
            case 2:
                powerupText.text = "Double Shot!";
                break;
            case 3:
                powerupText.text = "Triple Shot!";
                break;
            case 4:
                powerupText.text = "Shield!";
                break;
            case 5:
                powerupText.text = "Shield Again? Here's A Point!";
                break;
            default:
                powerupText.text = "No Power Ups Activated!";
                break;
        }
    }

    void CreateEnemy()
    {
        Instantiate(enemyOnePrefab, new Vector3(Random.Range(-horizontalScreenSize, horizontalScreenSize), verticalScreenSize, 0) * 0.9f, Quaternion.Euler(180, 0, 0));
    }

    void CreateSky()
    {
        for (int i = 0; i < 30; i++)
        {
            Instantiate(cloudPrefab, new Vector3(Random.Range(-horizontalScreenSize, horizontalScreenSize), Random.Range(-verticalScreenSize, verticalScreenSize), 0), Quaternion.identity);
        }
    }

    void CreateCoin()
    {
        Instantiate(coinPrefab, new Vector3(Random.Range(-horizontalScreenSize * 0.9f, horizontalScreenSize * 0.9f), Random.Range(-verticalScreenSize * 0.9f, verticalScreenSize * 0.9f), 0), Quaternion.identity);
    }

    void CreatePowerUp()
    {
        Instantiate(powerUpPrefab, new Vector3(Random.Range(-horizontalScreenSize * 0.9f, horizontalScreenSize * 0.9f), Random.Range(-verticalScreenSize * 0.9f, verticalScreenSize * 0.9f), 0), Quaternion.identity);
    }

    IEnumerator SpawnItems()
    {
        float spawnTime = Random.Range(4.5f, 8f);
        yield return new WaitForSeconds(spawnTime);
        CreatePowerUp();
        CreateCoin();
        StartCoroutine(SpawnItems());
    }

    public void AddScore(int earnedScore)
    {
        score = score + earnedScore;
    }

    public void ChangeLivesText(int currentLives)
    {
        livesText.text = "Lives: " + currentLives;
    }

    public void ChangeScoreText(int currentScore)
    {
        scoreText.text = "Score: " + currentScore;
    }

    public void GameOver()
    {
        gameOverText.SetActive(true);
        restartText.SetActive(true);
        gameOver = true;
        cloudMove = 0;
        CancelInvoke();
        StopAllCoroutines();
    }
}
