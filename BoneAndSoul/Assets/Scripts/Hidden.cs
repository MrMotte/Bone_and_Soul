using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
    Name:       Christian Schildhauer
    Date:       06.12.2016
    Function:   Set Player and leaves_states active/inactive
*/

public class Hidden : MonoBehaviour {

    public GameObject Overlay;
    public GameObject hidden;
    public GameObject leaves;
    [HideInInspector]
    public GameObject playerGO;
    public GameObject [] playerGO_rend;
    public float waitInSeconds = 3;
    [HideInInspector]
    public bool hideActive;
    [HideInInspector]
    public bool hide;
    [HideInInspector]
    public Player_Controller cont;

    public AudioSource leavesSound;
    ParticleSystem leaveExplosion;
    private SpriteRenderer augen;
    public GameObject augenAnim;

    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.8f;

    private int drawDepth = -1000;
    private float alpha = 0;
    [Range (0f,0.5f)]
    private int fadeDir = -1;
    public float wait;

    private int i = 0;

    void Start ()
    {
        leaveExplosion = GetComponentInChildren<ParticleSystem>();
        augen = augenAnim.GetComponent<SpriteRenderer>();
        cont = GetComponent<Player_Controller>();
        hidden.SetActive(hide);
        leaves.SetActive(!hide);
        hide = false;
        augen.enabled = false;
    }

    void Update()
    {
        hidden.SetActive(hide);
        leaves.SetActive(!hide);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hide && other.CompareTag("Player"))
        {
            BeginFade(1);
            i = 0;
            leavesSound.Play();
            leaveExplosion.Play();
            augen.enabled = true;
            while (i != playerGO_rend.Length) 
            { 
                playerGO_rend[i].SetActive(false);
                hide = true;
                i++;
            }
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (hide && other.CompareTag("Player"))
        {
            BeginFade(-1);
            i = 0;
            augen.enabled = false;
            while (i != playerGO_rend.Length)
            {
                playerGO_rend[i].SetActive(true);
                hide = false;
                i++;
            }
        }
    }


    void OnGUI()
    {
        if (fadeDir == 1 && alpha != 1)
        {
            //while (alpha !=0)
            //{
                alpha += fadeDir * fadeSpeed * Time.deltaTime;
            //alpha = Mathf.Clamp01(alpha);

            Overlay.GetComponent<Image>().color = new Color(Overlay.GetComponent<Image>().color.r, Overlay.GetComponent<Image>().color.g, Overlay.GetComponent<Image>().color.b, alpha);
                //GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
                //GUI.depth = drawDepth;
                //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);

            //}
        }
        if (fadeDir == -1 && alpha != 0)
        {
            //while (alpha != 1)
            //{
                alpha += fadeDir * fadeSpeed * Time.deltaTime;
                alpha = Mathf.Clamp01(alpha);

            Overlay.GetComponent<Image>().color = new Color(Overlay.GetComponent<Image>().color.r, Overlay.GetComponent<Image>().color.g, Overlay.GetComponent<Image>().color.b, alpha);
            //GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
            //    GUI.depth = drawDepth;
            //    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
            //}
        }
    }

    public float BeginFade(int dir)
    {
        fadeDir = dir;

        return (fadeSpeed);
    }
}
