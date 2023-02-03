using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerLevel1 : MonoBehaviour
{
    public float speed = 5.0f;
    public Sprite playerUp, playerDown, playerRight, playerLeft;
    public GameObject door;
    public GameObject finishLine;
    public GameObject walls;
    public Text keyText, levelText, scoreText;

    static bool flag = true;
    int keys = 0;
    public static int score = 0;
    public static string username = "";

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: " + score;
        if (flag)
        {
            int waitSeconds = 3;
            levelText.text = "Level 1";
            Destroy(levelText, waitSeconds);
            StartCoroutine(DisplayAfterSeconds(waitSeconds));
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
        }
        catch (MissingComponentException) { }
    }

    private IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Keys")
        {
            score += 100;
            scoreText.text = "Score: " + score;
            keys++;
            keyText.text = "Keys: " + keys + "/3";
            if (keys == 3)
            {
                Destroy(door);
            }
            Destroy(collision.gameObject);
        }

        if (collision.gameObject == finishLine)
        {
            score += 500;
            scoreText.text = "Score: " + score;
            Destroy(GetComponent<SpriteRenderer>());
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene("Level 2");
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

    public IEnumerator DisplayAfterSeconds(int seconds)
    {
        walls.gameObject.SetActive(false);
        yield return new WaitForSeconds(seconds);
        walls.gameObject.SetActive(true);
    }
}
