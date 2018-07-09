using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinFail : MonoBehaviour
{

    public Text winText;
    public Text failText;
    public int Leben;

    void Start()
    {
        winText.text = "";
        failText.text = "";
    }

    void Update()
    {
        fail();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            winText.text = "WIN!";
        }
    }

    void fail()
    {
        if (Leben == 0)
        {
            failText.text = "Game Over!";
        }
    }
}