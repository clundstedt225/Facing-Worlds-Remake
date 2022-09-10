using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //Usable spawn points on the map
    public List<Transform> redSpawnPoints;
    public List<Transform> blueSpawnPoints;

    bool gameOver = false;
    int winningScore = 10;

    public int redScore = 0;
    public int blueScore = 0;

    public GameObject mainUI, deathUI;
    public TextMeshProUGUI ammoCount;
    public TextMeshProUGUI healthCount;
    public TextMeshProUGUI rScore, bScore;
    public TextMeshProUGUI gameOverText;
    public GameObject gameOverCam, player;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver) return;

        //Is matches win condition met?
        if (redScore >= winningScore)
        {
            gameOver = true;

            mainUI.SetActive(false);
            deathUI.SetActive(false);
            player.SetActive(false);
            gameOverCam.SetActive(true);
            gameOverText.text = "Red Team Wins the Match!";
            gameOverText.color = Color.red;
        } else if (blueScore >= winningScore)
        {
            gameOver = true;

            mainUI.SetActive(false);
            deathUI.SetActive(false);
            player.SetActive(false);
            gameOverCam.SetActive(true);
            gameOverText.text = "Blue Team Wins the Match!";
            gameOverText.color = Color.blue;
        }
    }

    public void AddBlueKill()
    {
        //Add to score and update UI
        blueScore += 1;
        bScore.text = blueScore.ToString();
    }

    public void AddRedKill()
    {
        //Add to score and update UI
        redScore += 1;
        rScore.text = redScore.ToString();
    }

    public void setAmmoUI(int amount)
    {
        ammoCount.text = amount.ToString();
    }

    public void setHealthUI(int amount)
    {
        healthCount.text = amount.ToString();
    }
}
