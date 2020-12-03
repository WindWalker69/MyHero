using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioClip soundtrack;
    [SerializeField] private AudioClip playerAttack;
    [SerializeField] private AudioClip playerDeath;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip castMagic;
    [SerializeField] private AudioClip getCoin;
    [SerializeField] private AudioClip powerUp;

    private AudioSource musicSource;
    private AudioSource playerSource;
    private AudioSource enemySource;

    private void Awake()
    {
        instance = this;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = SaveGame.GetMusicVolume();
        musicSource.clip = soundtrack;
        musicSource.playOnAwake = true;
        musicSource.Play();

        playerSource = gameObject.AddComponent<AudioSource>();
        playerSource.volume = SaveGame.GetEffectsVolume();

        enemySource = gameObject.AddComponent<AudioSource>();
        enemySource.volume = SaveGame.GetEffectsVolume();
    }

    public void PlayPlayerAttack()
    {
        playerSource.clip = playerAttack;
        playerSource.Play();
    }

    public void PlayPlayerDeath()
    {
        playerSource.clip = playerDeath;
        playerSource.Play();
    }

    public void PlayPlayerHit()
    {
        playerSource.clip = hitSound;
        playerSource.Play();
    }

    public void PlayEnemyHit()
    {
        enemySource.clip = hitSound;
        enemySource.Play();
    }

    public void PlayCastMagic()
    {
        playerSource.clip = castMagic;
        playerSource.Play();
    }

    public void PlayGetCoin()
    {
        playerSource.clip = getCoin;
        playerSource.Play();
    }

    public void PlayPowerUp()
    {
        playerSource.clip = powerUp;
        playerSource.Play();
    }
}
