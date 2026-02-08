using System;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    public static HighScore Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private int highscore = 20;
    public void NewHighScore(int score)
    {
        highscore = score;
    }

    public int GetScore()
    {
        return highscore;
    }
}
