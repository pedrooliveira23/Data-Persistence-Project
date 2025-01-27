using System;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{
    private TMP_InputField _nameInput;
    private Score[] _scoreArray;
    public string scoreboardString;
    public string playerName;
    public int score;

    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void PopulateScoreArray()
    {
        _scoreArray = new Score[10];

        for (int i = 0; i < _scoreArray.Length; i++)
        {
            Score s = new Score();
            s.playerName = "Player " + (i + 1);
            s.scoreValue = 0;
            _scoreArray[i] = s;
        }
    }

    private void Update()
    {
        SetPlayerName();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ShowScoreboard()
    {
        LoadScoreboard();
        SceneManager.LoadScene(1);
    }

    public void LoadScoreboard()
    {
        string path = Path.Combine(Application.persistentDataPath, "scoreboard.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            ScoreboardModel scoreboard =
                JsonUtility.FromJson<ScoreboardModel>(json);
            _scoreArray = scoreboard.scoreArray;
        }
        else
        {
            PopulateScoreArray();
        }

        FormatScoreboardString();
    }

    public void SaveScoreboard()
    {
        LoadScoreboard();
        CalculatePlayerScoreboardPosition();
        string path = Path.Combine(Application.persistentDataPath, "scoreboard.json");
        ScoreboardModel scoreboard = new ScoreboardModel();
        scoreboard.scoreArray = _scoreArray;
        File.WriteAllText(path, JsonUtility.ToJson(scoreboard));
    }

    private void CalculatePlayerScoreboardPosition()
    {
        Score newScore = new Score();
        newScore.playerName = playerName;
        newScore.scoreValue = score;

        for (int i = 0; i < _scoreArray.Length; i++)
        {
            if (_scoreArray[i].scoreValue < newScore.scoreValue)
            {
                // Desloca os elementos para a direita até encontrar a posição correta
                for (int j = _scoreArray.Length - 1; j > i; j--)
                {
                    _scoreArray[j] = _scoreArray[j - 1];
                }

                // Insere a nova pontuação na posição correta
                _scoreArray[i] = newScore;
                return;
            }
        }
    }

    private void FormatScoreboardString()
    {
        scoreboardString = "";
        for (int i = 0; i < _scoreArray.Length; i++)
        {
            scoreboardString += (i + 1) + " - " + _scoreArray[i].playerName + " - " + _scoreArray[i].scoreValue +
                                "\n";
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void NewGame()
    {
        SceneManager.LoadScene(2);
    }

    private void SetPlayerName()
    {
        try
        {
            _nameInput = GameObject.Find("NameInput").GetComponent<TMP_InputField>();
            if (_nameInput.text.Length == 0)
            {
                _nameInput.text = playerName;
            }
            else
            {
                playerName = _nameInput.text;
            }
        }
        catch (Exception)
        {
            // ignored
        }
    }
}