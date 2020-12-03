using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance = null;

    private int playerLives = 3;

    private Text lifeCounter;
    private Text messageTextBox;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);

        lifeCounter = GameObject.FindGameObjectWithTag("LifeCounter").GetComponent<Text>();
        messageTextBox = GameObject.FindGameObjectWithTag("MessageTextBox").GetComponent<Text>();
    }

    public void RemoveLife()
    {
        playerLives--;
        lifeCounter.text = "X " + playerLives;
    }

    public int RemainingLives()
    {
        return playerLives;
    }
    
    public IEnumerator FullHealth()
    {
        messageTextBox.color = Color.red;
        messageTextBox.text = "Full Health";
        yield return new WaitForSeconds(1.5f);
        messageTextBox.text = "";
    }

    public IEnumerator FullMana()
    {
        messageTextBox.color = Color.blue;
        messageTextBox.text = "Full Mana";
        yield return new WaitForSeconds(1.5f);
        messageTextBox.text = "";
    }

    public IEnumerator DoubleDamage()
    {
        messageTextBox.color = new Color(0.6362123f, 0f, 1f);
        messageTextBox.text = "Double Damage";
        yield return new WaitForSeconds(1.5f);
        messageTextBox.text = "";
    }
}
