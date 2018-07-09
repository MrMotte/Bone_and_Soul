using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Steak_UI : MonoBehaviour {


    public Player_Controller powerupsHold;
    public GameObject[] Steak;

    // Use this for initialization
    void Start () {
        Steak[0].SetActive(true);
        Steak[1].SetActive(false);
    }
	
	// Update is called once per frame
	void Update() { 
        if(powerupsHold.pickups == 0)
        {
            Steak[0].SetActive(true);
            Steak[1].SetActive(false);
        }
        if (powerupsHold.pickups == 1)
        {
            Steak[0].SetActive(false);
            Steak[1].SetActive(true);
        }
    }
}
