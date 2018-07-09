using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class enemScript : MonoBehaviour {

        //set waypoints
    public List<Transform> waypoint = new List<Transform>();
        //walk speed
    public float patrolSpeed = 3;
        //turn-speed
    public float dampingLook = 6;
        //pause at each waypoint
    public float pauseDuration = 0;
        //player == target
    public GameObject Target;
    public bool loop = true;
        //index of current waypoint
    public int currentWaypoint = 0;

    //refrences to other own classes
    public FieldOfView fov;
    public Player_Controller playCont;


    private float curTime;
    private CharacterController charCont;
    
}
