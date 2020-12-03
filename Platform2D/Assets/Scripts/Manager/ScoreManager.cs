using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;

    private Text scoreText;
    private int score;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);

        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();
    }

    private void Start()
    {
        //TODO: chiamate a funzioni di SaveGame come GetScore()
    }

    public void AddScore(int score)
    {
        this.score += score;
        scoreText.text = "Score: " + this.score;
    }

    public void RemoveScore(int score)
    {
        this.score -= score;
        scoreText.text = "Score: " + this.score;
    }

    public int CurrentScore()
    {
        return score;
    }

    public void SaveScore()
    {
        //TODO: chiamata a funzione di SaveGame, SaveScore()
        //SaveGame.SaveScore(score);
    }
}
