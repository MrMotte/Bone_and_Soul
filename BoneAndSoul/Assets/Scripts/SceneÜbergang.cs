using UnityEngine;
using System.Collections;

public class SceneÜbergang : MonoBehaviour {

    int time = 2;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine("Bunga");
	}
	
    IEnumerator Bunga()
    {
        yield return new WaitForSeconds(time);
        UnityEngine.SceneManagement.SceneManager.LoadScene("SplashScreen_Team_logo");
    }
}
