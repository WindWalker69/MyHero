using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [HideInInspector] public CinemachineVirtualCamera vcam;
    [HideInInspector] public Transform lastCheckpointPos;

    public GameObject playerPrefab;
    public GameObject playerDeathEffect;
    public GameObject spawnEffect;
    public float playerRespawnDelay = 2f;

    private GameObject gameOverUI;
    private GameObject winScreenUI;
    private Text countdownText;
    private Text finalScoreText;
    private float currentTime = 0f;
    private float startingTime = 600f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);

        vcam = FindObjectOfType<CinemachineVirtualCamera>();
        gameOverUI = GameObject.FindGameObjectWithTag("GameOverUI");
        winScreenUI = GameObject.FindGameObjectWithTag("WinScreeUI");
        countdownText = GameObject.FindGameObjectWithTag("Countdown").GetComponent<Text>();
        finalScoreText = GameObject.FindGameObjectWithTag("FinalScoreText").GetComponent<Text>();
    }

    private void Start()
    {
        gameOverUI.SetActive(false);
        winScreenUI.SetActive(false);
        EnemyManager.instance.SpawnEnemies();
        currentTime = startingTime;
    }

    private void Update()
    {
        if (vcam.GetComponent<CinemachineVirtualCamera>().m_Follow == null)
        {
            vcam.GetComponent<CinemachineVirtualCamera>().m_Follow = FindPlayer().transform;
            if (vcam.GetComponent<CinemachineVirtualCamera>().m_Follow != null)
            {
                vcam.GetComponent<CinemachineVirtualCamera>().enabled = true;
            }
        }

        currentTime -= 1f * Time.deltaTime;
        countdownText.text = currentTime.ToString("0");

        if (currentTime <= 0f)
        {
            GameObject player = FindPlayer();
            vcam.GetComponent<CinemachineVirtualCamera>().enabled = false;
            Destroy(player);
            Instantiate(playerDeathEffect, player.transform.position, Quaternion.identity);
            SoundManager.instance.PlayPlayerDeath();
            GameOver();
        }
    }

    public void CheckIfGameOver(GameObject player)
    {
        vcam.GetComponent<CinemachineVirtualCamera>().enabled = false;
        Destroy(player);
        Instantiate(playerDeathEffect, player.transform.position, Quaternion.identity);
        SoundManager.instance.PlayPlayerDeath();
        if (PlayerManager.instance.RemainingLives() <= 0)
        {
            GameOver();
        }
        else
        {
            EnemyManager.instance.SpawnEnemies();
            StartCoroutine(RespawnPlayer());
        }
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
    }

    public void YouWin(GameObject player)
    {
        vcam.GetComponent<CinemachineVirtualCamera>().enabled = false;
        Destroy(player);
        SoundManager.instance.PlayPlayerDeath();
        winScreenUI.SetActive(true);
        finalScoreText.text = "Score: " + ScoreManager.instance.CurrentScore();
    }

    public IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(playerRespawnDelay);
        Instantiate(playerPrefab, lastCheckpointPos.position, Quaternion.identity);
        Instantiate(spawnEffect, lastCheckpointPos.position, Quaternion.identity);
    }

    public GameObject FindPlayer()
    {
        GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
        if (searchResult != null)
        {
            return searchResult;
        }
        else
            return null;
    }
}
