using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerLevel2 : MonoBehaviour
{
    public float speed = 5.0f;
    public int bullets = 2;
    public Sprite playerUp, playerDown, playerRight, playerLeft;
    public GameObject crown;
    public GameObject walls;
    int keys = 0;
    public Text keyText, levelText, bulletText, scoreText;
    static bool flag = true;

    public GameObject bulletRight, bulletLeft;
    Vector2 bulletPos;
    float fireRate = 0.5f;
    float nextFire = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: " + PlayerLevel1.score;
        crown.SetActive(false);
        if (flag)
        {
            int waitSeconds = 3;
            levelText.text = "Level 2";
            Destroy(levelText, waitSeconds);
            StartCoroutine(DisplayWallsAfterSeconds(waitSeconds));
            flag = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(-speed * Time.deltaTime, 0, 0);
                GetComponent<SpriteRenderer>().sprite = playerLeft;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(speed * Time.deltaTime, 0, 0);
                GetComponent<SpriteRenderer>().sprite = playerRight;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(0, speed * Time.deltaTime, 0);
                GetComponent<SpriteRenderer>().sprite = playerUp;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(0, -speed * Time.deltaTime, 0);
                GetComponent<SpriteRenderer>().sprite = playerDown;
            }
            if (Input.GetKey(KeyCode.LeftControl) && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                fire(0);
            }
            if (Input.GetKey(KeyCode.LeftShift) && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                fire(1);
            }
        }
        catch (MissingComponentException) { }
    }

    private IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Keys")
        {
            PlayerLevel1.score += 100;
            scoreText.text = "Score: " + PlayerLevel1.score;
            keys++;
            keyText.text = "Keys: " + keys + "/3";
            if (keys == 3)
            {
                crown.SetActive(true);
            }
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            PlayerLevel1.score -= 400;
            scoreText.text = "Score: " + PlayerLevel1.score;
            SceneManager.LoadScene("Level 2");
        }

        if (collision.gameObject == crown)
        {
            PlayerLevel1.score += 1000;
            scoreText.text = "Score: " + PlayerLevel1.score;
            Destroy(GetComponent<SpriteRenderer>());
            yield return new WaitForSeconds(2);
            Leaderboard.addLeaderboardEntry(PlayerLevel1.score, PlayerLevel1.username);
            SceneManager.LoadScene("Victory");
        }

        //player on collision with walls
        if (collision.gameObject.tag == "Walls")
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(-speed * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(0, -speed * Time.deltaTime, 0);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(0, speed * Time.deltaTime, 0);
            }
        }
    }

    public IEnumerator DisplayWallsAfterSeconds(int seconds)
    {
        walls.gameObject.SetActive(false);
        yield return new WaitForSeconds(seconds);
        walls.gameObject.SetActive(true);
    }

    void fire(int direction)
    {
        if (bullets > 0)
        {
            PlayerLevel1.score -= 200;
            scoreText.text = "Score: " + PlayerLevel1.score;
            bulletPos = transform.position;
            bullets--;
            bulletText.text = "Bullets: " + bullets;
            if (direction == 0)
            {
                bulletPos += new Vector2(+0.4f, 0f);
                Instantiate(bulletRight, bulletPos, Quaternion.identity);
            }
            else
            {
                bulletPos += new Vector2(-0.4f, 0f);
                Instantiate(bulletLeft, bulletPos, Quaternion.identity);
            }
        }
    }
}
