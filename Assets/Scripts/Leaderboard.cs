using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> leaderboardEntryTransformList;
    private void Awake()
    {
        entryContainer = transform.Find("highScoreEntryContainer");
        entryTemplate = entryContainer.Find("highScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        /*
        LeaderboardEntries leaderboardEntries1 = new LeaderboardEntries();
        leaderboardEntries1.leaderboardEntryList = new List<LeaderboardEntry>();
        for (int i = 0; i < 7; i++)
            leaderboardEntries1.leaderboardEntryList.Add(new LeaderboardEntry() { score = 0, name = "NIL" });

        string json = JsonUtility.ToJson(leaderboardEntries1);
        PlayerPrefs.SetString("leaderboard", json);
        PlayerPrefs.Save();
        */

        string jsonString = PlayerPrefs.GetString("leaderboard");
        LeaderboardEntries leaderboardEntries = JsonUtility.FromJson<LeaderboardEntries>(jsonString);

        leaderboardEntryTransformList = new List<Transform>();
        foreach (LeaderboardEntry leaderboardEntry in leaderboardEntries.leaderboardEntryList)
        {
            createLeaderboardEntryTransform(leaderboardEntry, entryContainer, leaderboardEntryTransformList);
        }
    }

    private void createLeaderboardEntryTransform(LeaderboardEntry leaderboardEntry, Transform container, List<Transform> leaderboardEntryTransformList)
    {
        float templateHeight = 40f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * leaderboardEntryTransformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = leaderboardEntryTransformList.Count + 1;
        string rankString;
        switch (rank)
        {
            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
            default: rankString = rank + "TH"; break;
        }

        entryTransform.Find("rankText").GetComponent<Text>().text = rankString;
        int score = leaderboardEntry.score;
        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();
        entryTransform.Find("nameText").GetComponent<Text>().text = leaderboardEntry.name;

        // background visibility for odd and even entries
        entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);

        // highlight rank 1
        if (rank == 1)
        {
            entryTransform.Find("rankText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<Text>().color = Color.green;
        }

        leaderboardEntryTransformList.Add(entryTransform);
    }

    private static void sortLeaderboard(List<LeaderboardEntry> leaderboardEntryList)
    {
        //sorting entry list by score
        for (int i = 0; i < leaderboardEntryList.Count; i++)
        {
            for (int j = i + 1; j < leaderboardEntryList.Count; j++)
            {
                if (leaderboardEntryList[j].score > leaderboardEntryList[i].score)
                {
                    LeaderboardEntry temp = leaderboardEntryList[i];
                    leaderboardEntryList[i] = leaderboardEntryList[j];
                    leaderboardEntryList[j] = temp;
                }
            }
        }
    }

    public static void addLeaderboardEntry(int score, string name)
    {
        // create LeaderboardEntry
        LeaderboardEntry leaderboardEntry = new LeaderboardEntry { score = score, name = name };

        // load saved leaderboard from player prefs
        string jsonString = PlayerPrefs.GetString("leaderboard");
        LeaderboardEntries leaderboardEntries = JsonUtility.FromJson<LeaderboardEntries>(jsonString);

        // add new entry to the leaderboard list
        leaderboardEntries.leaderboardEntryList.Add(leaderboardEntry);

        sortLeaderboard(leaderboardEntries.leaderboardEntryList);

        // restricting the number of elements in the list to 7
        while (leaderboardEntries.leaderboardEntryList.Count > 7)
            leaderboardEntries.leaderboardEntryList.RemoveAt(leaderboardEntries.leaderboardEntryList.Count - 1);

        // save updated leaderboard
        string json = JsonUtility.ToJson(leaderboardEntries);
        PlayerPrefs.SetString("leaderboard", json);
        PlayerPrefs.Save();
    }

    private class LeaderboardEntries
    {
        public List<LeaderboardEntry> leaderboardEntryList;
    }

    [System.Serializable]
    private class LeaderboardEntry
    {
        public int score;
        public string name;
    }
}
