using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour {

    public Timer time;
    private float currentTime;
    public int highscoreVarOne;
    public int highscoreVarTwo;
    public int highscoreVarThree;
    public Player_Controller victory;
    public Text textone;
    public Text texttwo;
    public Text textthree;
    private int a = 1;
    private int b = 1;
    private int c = 1;
    public int timetoint;
    //Christian, 09.02.17 //begin
    //public Text gameOverScore;
    public Text victoryScore;
    public Text victoryHighScore;
    //public Text Timer;
    //end

    // Use this for initialization
    void Start () {
        //if (PlayerPrefs.GetFloat("highscore") != 0)
        //{
            highscoreVarOne = PlayerPrefs.GetInt("highscoreOne");
            textone.text = "Best Time: " + highscoreVarOne.ToString();
            highscoreVarTwo = PlayerPrefs.GetInt("highscoreTwo");
            texttwo.text = "Best Time: " + highscoreVarTwo.ToString();
            highscoreVarThree = PlayerPrefs.GetInt("highscoreThree");
            textthree.text = "Best Time: " + highscoreVarThree.ToString();
         //}
    }
	
	// Update is called once per frame
	void Update () {

        timetoint = (int)time.timeLeft;
        //victoryScore = Timer;
        //gameOverScore = Timer;



        //dritte if mit abfrage nach Scene für Best Time Text aktualliesierung
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Gameplay_level1")
        {
            if(a == 1)
            {
                highscoreVarOne = PlayerPrefs.GetInt("highscoreOne");
                textone.text = "Best Time: " + highscoreVarOne.ToString();
                //victoryHighScore.text = highscoreVarOne.ToString(); ;
                a++;
            }


            if (victory.victoryone)
            {
                if (time.timeLeft <= highscoreVarOne || highscoreVarOne == 0)
                {
                    highscoreVarOne = (int)time.timeLeft;
                    victoryScore.text = timetoint.ToString();
                    PlayerPrefs.SetInt("highscoreOne", highscoreVarOne);
                    textone.text = "Best Time: " + highscoreVarOne.ToString();
                    victoryHighScore.text = highscoreVarOne.ToString();
                    Save();
                }
            }
        }
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Gameplay_level2")
        {
            if (b == 1)
            {
                highscoreVarTwo = PlayerPrefs.GetInt("highscoreTwo");
                texttwo.text = "Best Time: " + highscoreVarTwo.ToString();
                //victoryHighScore.text = highscoreVarTwo.ToString(); ;
                b++;
            }
            if (victory.victorytwo)
            {
                if (time.timeLeft <= highscoreVarTwo || highscoreVarTwo == 0)
                {
                    highscoreVarTwo = (int)time.timeLeft;
                    victoryScore.text = timetoint.ToString();
                    PlayerPrefs.SetInt("highscoreTwo", highscoreVarTwo);
                    texttwo.text = "Best Time: " + highscoreVarTwo.ToString();
                    victoryHighScore.text = highscoreVarTwo.ToString();
                    Save();
                }
            }
        }
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Gameplay_level3")
        {
            if (c == 1)
            {
                highscoreVarThree = PlayerPrefs.GetInt("highscoreThree");
                textthree.text = "Best Time: " + highscoreVarThree.ToString();
                //victoryHighScore.text = highscoreVarThree.ToString(); ;
                c++;
            }
            if (victory.victorythree)
            {
                if (time.timeLeft <= highscoreVarThree || highscoreVarThree == 0)
                {
                    highscoreVarThree = (int)time.timeLeft;
                    victoryScore.text = timetoint.ToString();
                    PlayerPrefs.SetInt("highscoreThree", highscoreVarThree);
                    textthree.text = "Best Time: " + highscoreVarThree.ToString();
                    victoryHighScore.text = highscoreVarThree.ToString();
                    Save();
                }
            }
        }
    }
    public static void Save()
    {
        //PlayerPrefs.Save("highscoreOne");
        //PlayerPrefs.Save("highscoreTwo");
        //PlayerPrefs.Save("highscoreThree");
    }
       
}
