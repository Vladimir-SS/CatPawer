using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    float score = 0;
    int level = 0;
    float highScore = 0;

    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if (scoreText)
        {
            scoreText.text = "Score: " + score.ToString() + "  Level: " + level.ToString();
        }
        else
        {
            highScore = PlayerPrefs.GetFloat("highScore", 0);
            highScoreText.text = "High Score: " + highScore.ToString();
        }

    }

    public void AddPoints(float points)
    {
        score += points;

        if(score > highScore)
        {
            PlayerPrefs.SetFloat("highScore", score);
        }

        Debug.Log("Points added " + points);
    }

    public void NextLevel()
    {
        level++;
    }
}
