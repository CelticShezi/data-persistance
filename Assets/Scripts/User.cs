using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;



#if UNITY_EDITOR
using UnityEditor;
#endif

public class User : MonoBehaviour
{
    public static User Instance { get; private set; }
    public string PlayerName { get; private set; }
    public int HighScore { get; private set; }
    public string HighPlayer {  get; private set; }

    private string savePath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        savePath = Application.persistentDataPath + "/highScore.json";
        LoadHighScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit()
#endif
    }

    public void UpdatePlayerName(string name)
    {
        PlayerName = name;
    }

    public void UpdateHighScore(int score)
    {
        HighScore = score;
        HighPlayer = PlayerName;
        SaveHighScore();
    }

    private void SaveHighScore()
    {
        HighScoreData hsd = new HighScoreData();
        hsd.name = HighPlayer;
        hsd.score = HighScore;

        string json = JsonUtility.ToJson(hsd);
        File.WriteAllText(savePath, json);
    }

    private void LoadHighScore()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);

            HighScoreData hsd = JsonUtility.FromJson<HighScoreData>(json);
            HighPlayer = hsd.name;
            HighScore = hsd.score;
        }
    }

    [Serializable]
    private class HighScoreData
    {
        public int score;
        public string name;
    }
}
