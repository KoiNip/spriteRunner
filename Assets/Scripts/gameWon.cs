using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameWon : MonoBehaviour
{
    public Text coinText;
    public GameObject player;
    private int numOfCoinsInLevel = 20;

    public void Setup(int coins)
    {
        gameObject.SetActive(true);
        if(numOfCoinsInLevel == coins)
        {
            coinText.text = "You collected all " + coins.ToString() + " coins!";
        }
        else
        {
            coinText.text = "Coins: " + coins.ToString();
        }
        player = GameObject.Find("Player");
        player.GetComponent<playerController>().setIsGameOver(true);
    }

    public void restartButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
