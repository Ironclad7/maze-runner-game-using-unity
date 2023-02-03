using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void startGame(string name)
    {
        GameObject t = GameObject.Find("usernameInputText");
        PlayerLevel1.username = t.GetComponent<Text>().text;
        SceneManager.LoadScene(name);
    }

    public void loadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
