using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreRegistry : MonoBehaviour
{
    public Leaderboard Leaderboard;

    public IEnumerator GetScores(string appName)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get($"https://serene-tor-28878.herokuapp.com/data?app={appName}"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Received: " + webRequest.downloadHandler.text);
            }

            Leaderboard.FillGlobalTable(webRequest.downloadHandler.text);
        }
    }

    public void AddNewScore(string gameName, string username, int score)
    {
        // send to server
        StartCoroutine(Upload(gameName, username, score.ToString()));

        // update client data
        ScoreList jsonScores = JsonUtility.FromJson<ScoreList>(PlayerPrefs.GetString("localScore"));

        Array.Resize(ref jsonScores.scores, jsonScores.scores.Length + 1);
        jsonScores.scores[jsonScores.scores.GetUpperBound(0)] = new Score(username, DateTime.UtcNow.Date.ToString("yyyy-MM-dd"), score);
        string json = JsonUtility.ToJson(jsonScores);
        print("new LS: " + json);
        PlayerPrefs.SetString("localScore", json);
    }

    private IEnumerator Upload(string gameName, string username, string score)
    {
        WWWForm form = new WWWForm();

        form.AddField("app", gameName);
        form.AddField("user", username);
        form.AddField("score", score);

        UnityWebRequest www = UnityWebRequest.Post("https://serene-tor-28878.herokuapp.com/addEntry", form);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Successfully uploaded new score.");
        }
    }
}