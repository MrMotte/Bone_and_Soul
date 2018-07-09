using UnityEngine;
using System.Collections;

public class Steak_AnimControll : MonoBehaviour {

    private SpriteRenderer steak;

	// Use this for initialization
	void Start () {
        steak = this.GetComponent<SpriteRenderer>();
        steak.enabled = false;
	}
	
void OnTriggerEnter(Collider other)
    {
        steak.enabled = true;
    }

    void OnTriggerExit(Collider other)
    {
        steak.enabled = false;
    }
}
