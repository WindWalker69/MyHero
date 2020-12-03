using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject enemyManager;
    public GameObject scoreManager;
    public GameObject playerManager;
    public GameObject soundManager;

    private void Awake()
    {
        if (GameManager.instance == null)
            Instantiate(gameManager);

        if (EnemyManager.instance == null)
            Instantiate(enemyManager);

        if (ScoreManager.instance == null)
            Instantiate(scoreManager);

        if (PlayerManager.instance == null)
            Instantiate(playerManager);

        if (SoundManager.instance == null)
            Instantiate(soundManager);
    }
}
