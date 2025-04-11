using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int lives;
    private float speed;
    private int weaponType;
    private bool isShieldActive;

    private GameManager gameManager;

    private float horizontalInput;
    private float verticalInput;

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject thrusterPrefab;
    public GameObject shieldPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        lives = 3;
        speed = 5.0f;
        weaponType = 1;
        isShieldActive = false;
        gameManager.ChangeLivesText(lives);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shooting();
    }
    IEnumerator WeaponPowerDown()
    {
        yield return new WaitForSeconds(3f);
        weaponType = 1;
        if (isShieldActive)
        {
            gameManager.ManagePowerUpText(4);
        }
        else
        {
            gameManager.ManagePowerUpText(0);
        }
        gameManager.PlaySound(3);
    }

    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(3f);
        speed = 5f;
        thrusterPrefab.SetActive(false);
        if (isShieldActive)
        {
            gameManager.ManagePowerUpText(4);
        }
        else
        {
            gameManager.ManagePowerUpText(0);
        }
        gameManager.PlaySound(3);
    }

    private void OnTriggerEnter2D(Collider2D whatDidIHit)
    {
        if (whatDidIHit.tag == "Coin")
        {
            Destroy(whatDidIHit.gameObject);
            gameManager.AddScore(1);
            gameManager.ChangeScoreText(gameManager.score);
            gameManager.PlaySound(1);
        } else if(whatDidIHit.tag == "PowerUp")
        {
            Destroy(whatDidIHit.gameObject);
            gameManager.PlaySound(2);
            int whichPowerUp = Random.Range(1, 5);
            switch (whichPowerUp)
            {
                case 1:
                    //speed
                    speed = 8f;
                    thrusterPrefab.SetActive(true);
                    gameManager.ManagePowerUpText(1);
                    StartCoroutine(SpeedPowerDown());
                    break;
                case 2:
                    //double shot
                    weaponType = 2;
                    StartCoroutine(WeaponPowerDown());
                    gameManager.ManagePowerUpText(2);
                    break;
                case 3:
                    weaponType = 3;
                    StartCoroutine(WeaponPowerDown());
                    gameManager.ManagePowerUpText(3);
                    //triple shot
                    break;
                case 4:
                    //sheild
                    if (isShieldActive)
                    {
                        gameManager.ManagePowerUpText(5);
                        gameManager.AddScore(1);
                        gameManager.ChangeScoreText(gameManager.score);
                        break;
                    }
                    else
                    {
                        shieldPrefab.SetActive(true);
                        isShieldActive = true;
                        gameManager.ManagePowerUpText(4);
                        gameManager.PlaySound(2);
                        break;
                    }
            }
        }
    }
    public void LoseALife()
    {
        if (isShieldActive)
        {
            lives = lives + 0;
            isShieldActive = false;
            shieldPrefab.SetActive(false);
            gameManager.ManagePowerUpText(0);
            gameManager.PlaySound(3);
        }
        else { 
            lives--;
         }
        gameManager.ChangeLivesText(lives);
        if(lives == 0)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            gameManager.GameOver();
            Destroy(this.gameObject);
        }
    }

    void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (weaponType)
            {
                case 1:
                    Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                    break;
                case 2:
                    Instantiate(bulletPrefab, transform.position + new Vector3(-0.5f, 0.5f, 0), Quaternion.identity);
                    Instantiate(bulletPrefab, transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                    break;
                case 3:
                    Instantiate(bulletPrefab, transform.position + new Vector3(-0.5f, 0.5f, 0), Quaternion.Euler(0, 0, 45));
                    Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                    Instantiate(bulletPrefab, transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.Euler(0, 0, -45));
                    break;
            }
        }

    }

    void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * speed);

        float horizontalScreenSize = gameManager.horizontalScreenSize;
        float verticalScreenSize = gameManager.verticalScreenSize;

        if(transform.position.x <= -horizontalScreenSize || transform.position.x > horizontalScreenSize)
        {
            transform.position = new Vector3(transform.position.x * -1, transform.position.y, 0);
        }

        if (transform.position.y <= -verticalScreenSize || transform.position.y > verticalScreenSize)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y * -1, 0);
        }
    }
}
