using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelManager : MonoBehaviour
{
    private GameObject player;
    public Text health_txt;
    public Text coins_txt;

    public gameOver gameOverScreen;
    int coins;  //# of coins player collected
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        coins = 0;
    }

    // Update is called once per frame
    void Update()
    {
        int player_health = player.GetComponent<playerController>().getHealth();    //Get the health of the player
        coins_txt.GetComponent<UnityEngine.UI.Text>().text = ("X " + coins.ToString()); //Set coin_txt to the number of coins
        health_txt.GetComponent<UnityEngine.UI.Text>().text = ("X " + player_health.ToString());    
        if(player_health == 0)
        {
            gameOverScreen.Setup(coins);
        }
    }

    public void incrCoins()
    {
        coins++;
    }
}
