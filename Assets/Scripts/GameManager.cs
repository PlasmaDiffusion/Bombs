using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public int time;
    public int endGameTime; //Invisible timer that ends the game and goes to the title screen

   

    bool gameEnded;

	// Use this for initialization
	void Start () {
        gameEnded = false;

        setTimerText();

        StartCoroutine(Timer());
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    IEnumerator Timer()
    {
        //Countdown clock
        yield return new WaitForSecondsRealtime(1);
        if (time > 0 && !gameEnded)
        {
            time--;
            setTimerText();
        }


        //After game ended it'll wait a few seconds.
        if (gameEnded)
        {
            endGameTime--;
            if (endGameTime <= 0) finishGame();
        }
        else if (time <= 0) //If the game didn't end yet then check if time ran out
        {
                //If it did it's a draw :/
                setWinText("Draw", new Color(0.8f, 0.0f, 0.8f));
        }


        StartCoroutine(Timer());

    }

    public void checkForWinner()
    {

        int aliveCount = 0;
        int winnerIndex = 0;

        //Check if someone won the game with an array of alive flags
        bool[] alivePlayers = new bool[4];

        for (int i = 0; i < 4; i++)
        {
            GameObject playerObject = GameObject.Find("Player" + (i + 1).ToString()); //Find each player by name. All players should be named like Player1, Player2, etc...

            //If player exists check if they're dead X_X
            if (playerObject)
            {
                Player p = playerObject.GetComponent<Player>();

                if (p.getHealth() <= 0.0f) alivePlayers[i] = false;
                else
                {
                    alivePlayers[i] = true;
                    winnerIndex = i; //If the player is alive, they may be a winner
                }
            }
            else alivePlayers[i] = false;

            if (alivePlayers[i]) aliveCount += 1;

        }

        //If ones alive then they have won! The player number of the winner was just recorded too.
        if (aliveCount == 1 && !gameEnded)
        {
            switch (winnerIndex)
            {
                case 0:
                    setWinText("Player 1\nWins!", new Color(0.8f, 0.5f, 0.5f));
                    break;

                case 1:
                    setWinText("Player 2\nWins!", new Color(0.5f, 0.5f, 0.8f));
                    break;

                case 2:
                    setWinText("Player 3\nWins!", new Color(0.8f, 0.8f, 0.5f));
                    break;

                case 3:
                    setWinText("Player 4\nWins!", new Color(0.5f, 0.8f, 0.5f));
                    break;


            }
            gameEnded = true;
        }

    }

    private void setWinText(string winner, Color color)
    {
        Text winText = GameObject.Find("WinText").GetComponent<Text>();

        //Set background text
        winText.text = winner;
        Text frontWinText = winText.transform.GetChild(0).GetComponent<Text>();

        //Set front text
        frontWinText.text = winner;
        frontWinText.color = color;


        gameEnded = true;

        time = -1;
        setTimerText();
    }

    void setTimerText()
    {
        string newTimeText = time.ToString();

        //If time is -1 the game ended. Don't display the timer anymore.
        if (time == -1) newTimeText = "";

        Text timeText = GameObject.Find("TimerText").GetComponent<Text>();

        //Set background text
        timeText.text = newTimeText;
        Text frontTimeText = timeText.transform.GetChild(0).GetComponent<Text>();

        //Set front text
        frontTimeText.text = newTimeText;
    }

    //Exit to menu or whatever
    void finishGame()
    {

        ReadAndWriteStats statsManager = GetComponent<ReadAndWriteStats>();

        statsManager.gamesPlayed++;
        statsManager.writeStats();

        //Reset node count
<<<<<<< HEAD
       //NodeGenerator.numNodes = 0;
=======
        NodeGenerator.numDepthReached = 0;
>>>>>>> 6baff78cdd982e620c5e1b08ab64c25352a2f216

        SceneManager.LoadScene("basicScene");
    }
}
