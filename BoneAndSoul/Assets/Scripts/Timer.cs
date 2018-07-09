using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour {

    public float timeLeft = 0;
    public Text text;
    public GameObject GameManagerGO;
    public GameObject playerGO;

    void Update ()
    {
        timeLeft += Time.deltaTime;
        text.text = "        " + Mathf.Round(timeLeft);
        if (timeLeft < 0)
        {
            GameOver();
        }
	}

    void GameOver()
    {
            //set GMState to GameOver
    //GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);
    //playerGO.SetActive(false);
           /*set GameOver screen
            play GameOver sound/music
            reset loaded scene*/

        // wait for x seconds --> GameManagerScript
    }
}
