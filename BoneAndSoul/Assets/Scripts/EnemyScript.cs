using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
    Name:       Samuel Bohren
    Date:       11.01.2017
    Function:   Add Animation
*/

public class EnemyScript : MonoBehaviour
{

    public List<Transform> waypoint = new List<Transform>();             // The amount of Waypoint you want
    public float patrolSpeed = 3;                                       // The walking speed between Waypoints
    public bool loop = true;                                           // Do you want to keep repeating the Waypoints
    public float dampingLook = 0;                                     // How slowly to turn
    public float pauseDuration = 0;                                  // How long to pause at a Waypoint
    public GameObject Target;
    public Transform player;
    [HideInInspector]
    public Player_Controller playCont;
    public float speed = 0.1f;

    private float curTime;
    public int currentWaypoint = 0;
    private CharacterController character;
    private FieldOfView fov;
    private Animator hundAnimator;

    public AudioSource bellen;
    public GameObject hideObject_1;
    public GameObject hideObject_2;
    public GameObject hideObject_3;
    public Hidden hiddenScript_1;
    public Hidden hiddenScript_2;
    public Hidden hiddenScript_3;

    private bool angriff;


    public bool canMove = true;

    void Awake()
    {
        playCont = Target.GetComponent<Player_Controller>();
        hiddenScript_1 = hideObject_1.GetComponent<Hidden>();
        hiddenScript_2 = hideObject_2.GetComponent<Hidden>();
        hiddenScript_3 = hideObject_3.GetComponent<Hidden>();
        character = GetComponent<CharacterController>();
        fov = GetComponent<FieldOfView>();
        hundAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        ControlIfHidden();
        AnimationControll();
        //SetAnimState();
    }


    void ControlIfHidden()
    {
        if (!fov.dogLookAtPlayer)
        {
            if (!hiddenScript_1.hide && !hiddenScript_2.hide && !hiddenScript_3.hide && !playCont.victory && !playCont.gameOver && !playCont.loseLife)
            {
                if (currentWaypoint < waypoint.Count)
                {
                    patrol();
                }
                else
                {
                    if (loop)
                    {
                        currentWaypoint = 0;
                    }
                }
            }
            else
            {
                if (waypoint.Contains(Target.transform))
                {
                    waypoint.Remove(Target.transform);
                }
                if (currentWaypoint < waypoint.Count)
                {
                    patrol();
                }
                else
                {
                    if (loop)
                    {
                        currentWaypoint = 0;
                    }
                }
            }

        }
        else if (fov.dogLookAtPlayer)
        {

            if (!hiddenScript_1.hide && !hiddenScript_2.hide && !hiddenScript_3.hide)
            {
                if (!waypoint.Contains(Target.transform))
                {
                    waypoint.Add(Target.transform);
                    bellen.Play();
                    //anim aggro

                }
                else
                {
                    currentWaypoint = waypoint.Count - 1;
                    patrol();

                }
            }
            else
            {
                currentWaypoint = waypoint.Count;
                patrol();
            }
        }
    }

    void patrol()
    {
        //walk
        if (!waypoint.Contains(Target.transform))
        {
            // hundAnimator.Play("walk");
        }

        Vector3 target = waypoint[currentWaypoint].position;
        target.y = transform.position.y; // Keep waypoint at character's height
        Vector3 moveDirection = target - transform.position;
        if (moveDirection.magnitude < 0.5)
        {
            if (curTime == 0)
            {
                curTime = Time.time; // Pause over the Waypoint
                hundAnimator.SetBool("walk", false);

            }
            if ((Time.time - curTime) >= pauseDuration)
            {
                currentWaypoint++;
                curTime = 0;
            }
        }
        else
        {
            if (canMove)
            {
                Quaternion rotation = Quaternion.LookRotation(target - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * dampingLook);
                character.Move(moveDirection.normalized * patrolSpeed * Time.deltaTime);
                hundAnimator.SetBool("walk", true);
            }
        }
    }

    void AnimationControll()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Player");

            Player_Controller player_Controller = enemy.GetComponent<Player_Controller>() as Player_Controller;
            if (player_Controller.IsAttack)
            {
                canMove = false;
               hundAnimator.SetBool("walk", false);
                //transform.Find("FieldOfWiew").gameObject.SetActive(false);
            } else {
                canMove = true;
                //hundAnimator.SetBool("walk", true);
            }
        
    }


}
