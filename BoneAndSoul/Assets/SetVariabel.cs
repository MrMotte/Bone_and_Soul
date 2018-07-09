using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetVariabel : MonoBehaviour {

    public GameManager GM;

	void Start ()
    {
		if (SceneManager.GetActiveScene().name == "StartScreen")
        {
            GM = GameObject.Find("GameManager").GetComponent<GameManager>();

            if(GM != null && GM.lastLevel == 3)
            {
                GM.str = "Gameplay_level1";
                GM.lastLevel = 0;
                this.gameObject.SetActive(false);
            }
        }
	}
}
