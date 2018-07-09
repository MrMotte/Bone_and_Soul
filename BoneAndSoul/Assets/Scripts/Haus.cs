using UnityEngine;
using System.Collections;

public class Haus : MonoBehaviour {

    private Animator hausAnimation;

    void Start()
    {
        hausAnimation = GetComponentInChildren<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            hausAnimation.SetBool("OpenPort", true);
        }
    }
}
