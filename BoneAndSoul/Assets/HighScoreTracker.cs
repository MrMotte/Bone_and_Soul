using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreTracker : MonoBehaviour {

    public int Score = 0;
    public int HighScore = 0;

  

    void Update()
    {
        if(Score > HighScore)
        {
            HighScore = Score;
        }
    }
    
}
