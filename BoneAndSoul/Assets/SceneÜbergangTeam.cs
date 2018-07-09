using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneÜbergangTeam : MonoBehaviour {

    int time = 2;

    // Use this for initialization
    void Start()
    {
        StartCoroutine("Display");
    }

    IEnumerator Display()
    {
        yield return new WaitForSeconds(time);
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScreen");
    }
}
