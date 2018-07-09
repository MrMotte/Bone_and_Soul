using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Blumen : MonoBehaviour
{

    Animator flowerpower;


    // Use this for initialization
    void Start()
    {

        //  flowerpower.enabled = false;
        flowerpower = GetComponent<Animator>();

    }

    void OnTriggerEnter(Collider other)
    {
        flowerpower.SetBool("Flower", true);
    }
}