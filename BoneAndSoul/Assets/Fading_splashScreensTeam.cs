using UnityEngine;
using System.Collections;

//Autor:    Christian Schildhauer
//Use:      Fading between Scenes
//Date:     24.01.2017

public class Fading_splashScreensTeam: MonoBehaviour {

    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.8f;

    private int drawDepth = -1000;
    private float alpha = 1;
    private int fadeDir = -1;
    public float wait;
    public float waittwo;


    void Start()
    {
        StartCoroutine(FadingMemes("NEW"));
    } 

    void OnGUI()
    {

            alpha += fadeDir * fadeSpeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);

            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
            GUI.depth = drawDepth;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    public float BeginFade (int dir)
    {
        fadeDir = dir;

        return (fadeSpeed);
    }

    //void OnLevelWasLoaded()
    //{

    //    alpha = 1;
    //    StartCoroutine(FadingMemes("NEW"));

    //}

    public IEnumerator FadingMemes(string to)
    {

        to = "Warten auf";
        BeginFade(-1);
        yield return new WaitForSeconds(wait);
        
        StartCoroutine(FadingMomes("NEW"));
        //UnityEngine.SceneManagement.SceneManager.LoadScene("StartScreen");
        to = "Godot";


    }

    public IEnumerator FadingMomes(string to)
    {

        to = "Warten auf";
        BeginFade(1);
        yield return new WaitForSeconds(waittwo);

        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScreen");
        to = "Godot";


    }
    //if scene changes to next
    //beginFade(-1)
}
