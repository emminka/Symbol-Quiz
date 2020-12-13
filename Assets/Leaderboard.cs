using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    public ScoreRegistry ScoreRegistry;

    public GameObject scorePanel;
    public GameObject scoreEntry;

    public GameObject localText, localText2, localImage;
    public Sprite localSprite, globalSprite;

    public GameObject globalScorePanel;
    public GameObject globalScoreEntry;
    public GameObject globalScrollView, localScrollView;

    public GameObject loadingImage;

    private bool showLocal = true;

    public void RefreshScores()
    {
        //loadingImage.SetActive(true);
        print(PlayerPrefs.GetString("localScore"));
        ScoreList jsonScores = JsonUtility.FromJson<ScoreList>(PlayerPrefs.GetString("localScore"));

        Array.Sort(jsonScores.scores, delegate (Score s1, Score s2)
        {
            return s2.score.CompareTo(s1.score);
        });

        for (int i = 0; i < jsonScores.scores.Length; i++)
        {
            Score s = jsonScores.scores[i];
            GameObject newEntry = Instantiate(scoreEntry, scorePanel.transform);

            newEntry.SetActive(true);

            newEntry.transform.Find("Name").GetComponent<Text>().text = s.name;
            newEntry.transform.Find("Date").GetComponent<Text>().text = s.date;
            newEntry.transform.Find("Score").GetComponent<Text>().text = s.score.ToString();

            newEntry.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -150 * i - 75);
        }

        scorePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, jsonScores.scores.Length * 150);

        StartCoroutine(ScoreRegistry.GetScores("symbol quiz"));
    }

    public void FillGlobalTable(string data)
    {
        ScoreList jsonScores = JsonUtility.FromJson<ScoreList>(data);

        for (int i = 0; i < jsonScores.scores.Length; i++)
        {
            Score s = jsonScores.scores[i];
            GameObject newEntry = Instantiate(globalScoreEntry, globalScorePanel.transform);

            newEntry.SetActive(true);

            newEntry.transform.Find("Name").GetComponent<Text>().text = s.name;
            newEntry.transform.Find("Date").GetComponent<Text>().text = s.date;
            newEntry.transform.Find("Score").GetComponent<Text>().text = s.score.ToString();

            newEntry.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -150 * i - 75);
        }

        globalScorePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, jsonScores.scores.Length * 150);

        //loadingImage.SetActive(false);
    }

    public void ToggleLocal()
    {
        showLocal = !showLocal;

        if (showLocal)
        {
            localText.GetComponent<Text>().text = "Local scores";
            localText2.GetComponent<Text>().text = "Local scores";
            //localImage.GetComponent<Image>().sprite = localSprite;
            scorePanel.SetActive(true);
            globalScorePanel.SetActive(false);

            globalScrollView.SetActive(false);
            localScrollView.SetActive(true);
        }
        else
        {
            localText.GetComponent<Text>().text = "Global scores";
            localText2.GetComponent<Text>().text = "Global scores";
            //localImage.GetComponent<Image>().sprite = globalSprite;
            globalScorePanel.SetActive(true);
            scorePanel.SetActive(false);

            localScrollView.SetActive(false);
            globalScrollView.SetActive(true);
        }
    }
}

[System.Serializable]
public class ScoreList
{
    public Score[] scores;
}

[System.Serializable]
public class Score
{
    public string name;
    public string date;
    public int score;

    public Score(string _name, string _date, int _score)
    {
        name = _name;
        date = _date;
        score = _score;
    }
}
