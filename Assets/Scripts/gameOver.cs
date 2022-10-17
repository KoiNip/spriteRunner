using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameOver : MonoBehaviour
{
    public Text coinText;
    public GameObject player;
    public void Setup(int coins)
    {
        gameObject.SetActive(true);
        coinText.text = "Coins: " + coins.ToString();
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
