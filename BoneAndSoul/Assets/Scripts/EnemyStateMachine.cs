using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyStateMachine : MonoBehaviour {

    
    public GameObject Target;                                    //reference to player
    public List<Transform> waypoint = new List<Transform>();    //waypoints == positions in space which enemy will walk to
    public float speed = 3;                                     //speed of enemy
    public float pauseDuration = 1;                             //pause duration while reach waypoint
    public int currentWaypoint = 0;                             //current Index of waypoint
    public float turnSpeed = 5;                                 //Time to rotate enemy to next waypoint
    public bool loop = true;                                    //loop through waypoints

    private CharacterController character;                      //reference to attached characterController
    private float curTime = 0;
    private FieldOfView fov;                                    //reference to fov script
    private Player_Controller playCont;


    //NEW REFRENCES
   // public SpriteRenderer sRend_mark;
    //public SpriteRenderer sRend_qmark;

    public bool displayMark = false;
    public bool displayuestionMark = false;
    public bool sawPickUp = false;

    public float AttentionTime = 1f;
    public float TimeToLoose = 2f;
    public float SearchTime = 3f;

    public Transform pickUp;

    public List<GameObject> hiddenList = new List<GameObject>();    // list of all hiddenObjects in scene
    public bool playerIsHidden = false;
    public bool getPickUp = true;
    public BoxCollider boxC;

    public Animator dogAnimator;
    private bool angriff;
    public bool canMove = true;

    public Transform cam;
    public AudioSource bellen;

    public bool dogFollow = false;


    public Transform zeroPosition;
    //NEW REFRENCES



    void Awake()
    {
        boxC = GetComponent<BoxCollider>();
        EState = EnemyState.WalkState;
        UpdateEnemyState();
        character = GetComponent<CharacterController>();
        fov = GetComponentInChildren<FieldOfView>();  //updated 24.01.2017 FieldOf View befindet sich jetzt am Child Object
        playCont = Target.GetComponent<Player_Controller>();
        //sRend_mark.enabled = false;
        //sRend_qmark.enabled = false;

    }


    void Update()
    {
        if (waypoint.Contains(Target.transform) && !fov.dogLookAtPlayer)
        {
            dogFollow = true;
        }
        else
        {
            dogFollow = false;
        }
        //Christian, Bugfix, 26.01.17
        //begin
        if (playCont.loseLife)
        {
            if (waypoint.Contains(Target.transform))
            {
                waypoint.Remove(Target.transform);
            }
        }
        //end

        //Christian, Bugfixing, 27.01.17
        //begin
        if (pickUp != null)
        {
            if (waypoint.Contains(pickUp))
            {
                if (transform.position == pickUp.position || pickUp.position == pickUp.GetComponent<PowerUp>().depot.transform.position || 
                    pickUp.position == pickUp.GetComponent<PowerUp>().spawn.transform.position)
                {
                    waypoint.Remove(pickUp);
                }
            }
        }
        //end

        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);

        //this.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        AnimationControll();
        //protect to wayPoint failures
        if (currentWaypoint >= waypoint.Count)
        {
            currentWaypoint = 0;
        }
        patrol();

        if (pickUp == null && fov.dogLookAtPowerup && getPickUp)
        {
            pickUp = fov.pickup;
        }

        if (fov.dogLookAtPowerup)
        {
            sawPickUp = true;
        }

        if (EState == EnemyState.AttentionState && !playCont.hide && !sawPickUp)
        {
            SetState(EnemyState.FollowState);
        }

        if (EState == EnemyState.FollowState && !fov.dogLookAtPlayer)
        {
             StartCoroutine("Waiting", TimeToLoose);
             SetState(EnemyState.SearchTargetState);
        }
        //check if player is hidden
        CheckForHidden();
        playerIsHidden = CheckForHidden();

        //rotate marks to cam
        //sRend_mark.transform.rotation = cam.rotation;
        //sRend_qmark.transform.rotation = cam.rotation;
        
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //waypoint.Remove(Target.transform);
            currentWaypoint = 0;
            Move();
        }
        if (other.CompareTag("Pickup"))
        {
            boxC.enabled = false;
            character.enabled = false;
            StartCoroutine("LosePickUp", TimeToLoose+2);
        }
    }

	public enum EnemyState 
        {
            IdleState,
            WalkState,
            TurnAroundState,
            AttentionState,
            FollowState,
            AttackState,
            SearchTargetState,
            KOState
        }
    EnemyState EState;

    //Update State
    void UpdateEnemyState()
    {
        switch(EState)
        {
            case EnemyState.IdleState:
                //play IdleAnim
                //stopp moving
                //invoke WalkState
                SetState(EnemyState.TurnAroundState);
                break;

            case EnemyState.WalkState:
                //play WalkAnim
                //Move -> turn to TurnAroundState if reached WayPoint
              //SetState(EnemyState.TurnAroundState);
                break;

            case EnemyState.TurnAroundState:
                //play TurnAroundAnim
                //Stop moving
                //invoke WalkState
                SetWalkState();
                break;

            case EnemyState.AttentionState:
                //stop moving
                //play IdleAnim
                //show Attention (Sprite?, Sound?)
                //change to FollowState
                // 1.0 Stop moving;
              //StartCoroutine("Wait", AttentionTime);
                //sRend_mark.enabled = true;
                displayMark = true;
                break;

            case EnemyState.FollowState:
                //moveToTarget (Player)
                //play FollowAnim
                //play AggroSound (Bellen)
                //if reach player/target --> change to AttackState
                //if lose target/player --> change to SearchTargetState
                break;

            case EnemyState.AttackState:
                //stop moveToTarget (Player) --> freeze Player if hit
                //play AttackAnim
                //play AttackSound
                //if player hit --> change to walkState (last WayPoint)
                ReturnToWayPoint();
                SetWalkState();
                sawPickUp = false;
                break;

            case EnemyState.SearchTargetState:
                //stop MoveToTarget
                //play SearchingAnim
                //show Searching (Sprite?, Sound?)
                //invoke change to WalkAnim (last WayPoint)
                //if getPlayer --> change to AttentionState
               // sRend_qmark.enabled = true;
                displayuestionMark = true;
              //StartCoroutine("Wait", SearchTime);
                ReturnToWayPoint();
                SetWalkState();
                break;

            case EnemyState.KOState:
                //play KOAnim
                //show KOState(Sprite?, Sound?)
                //stop move and MoveToTarget
                //invoke IdleState
                sawPickUp = false;
                Invoke("SetWalkState", 1f);
                break;

        }
    }

    //change State by parameter
    public void SetState(EnemyState eState)
    {
        EState = eState;
        UpdateEnemyState();
    }

    //change to IdleState
    public void SetIdleState()
    {
        EState = EnemyState.IdleState;
        UpdateEnemyState();
    }

    //change to walkState
    public void SetWalkState()
    {
        EState = EnemyState.WalkState;
        UpdateEnemyState();
    }

    //change to followState
    public void SetFollowState()
    {
        EState = EnemyState.FollowState;
        UpdateEnemyState();
    }

    void patrol()
    {
        if (!fov.dogLookAtPlayer && !fov.dogLookAtPowerup)
        {
            if (waypoint[currentWaypoint] == Target.transform && playerIsHidden)
            {
                //set searchTime
                StartCoroutine("Search", SearchTime*2);
                
            }
            //wp pattern
            if (currentWaypoint < waypoint.Count)
            {
                Move();
            }
            else
            {
                if (loop)
                {
                    currentWaypoint = 0;
                }
            }
        }
        //Christian, 09.02.17 //begin
        if (fov.dogLookAtPlayer)
        {
            //bellen.Play();
        }
        //end

        if (fov.dogLookAtPlayer && !fov.dogLookAtPowerup && !playerIsHidden)
        {
            //WIRD AUSGELÖST; WENN PLAYER DEN SICHTBEREICH VERLÄSST
            //bellen.Play();
            StartCoroutine("Wait", 1f);
            //remove pickup from wpList
            if (waypoint.Contains(pickUp))                                                      // start Coroutine to check, if transform equals pickUp.transform
            {                                                                                   // if true, remove pickup.transform, else continue 
                if (transform.position == pickUp.position || pickUp.position == pickUp.GetComponent<PowerUp>().depot.transform.position)
                {
                    waypoint.Remove(pickUp);
                }
            }
            //add player to wpList
            if (!waypoint.Contains(Target.transform))
            {
                bellen.Play();
                waypoint.Add(Target.transform);
            }
            //currentwp = player
            currentWaypoint = waypoint.Count - 1;
            Move();
        }

        if (!fov.dogLookAtPlayer && fov.dogLookAtPowerup)
        {
            //remove player from wpList
            if (waypoint.Contains(Target.transform))
            {
                waypoint.Remove(Target.transform);
            }
            //add pickup to wplist
            if (pickUp != null)
            {
                waypoint.Add(pickUp.transform);
             }
            //currentwp = pickup
            currentWaypoint = waypoint.Count - 1;
            //move to pickup
            Move();
        }

        if (fov.dogLookAtPlayer && fov.dogLookAtPowerup)
        {
            //remove player
            if (waypoint.Contains(Target.transform))
            {
                waypoint.Remove(Target.transform);
            }
            //add pickup
            if (pickUp != null)
            {
                waypoint.Add(pickUp);
            }
            //currentwp = pickup
            currentWaypoint = waypoint.Count - 1;
            //move to pickup
            Move();
        }

        
    }


    //move from waypoint to waypoint
    void Move()
    {
        Vector3 target = Vector3.zero;
        if (waypoint[currentWaypoint] != null)
        {
           target = waypoint[currentWaypoint].position;
            
        }
        target.y = transform.position.y;    //Keep waypoint at enemy's height
        Vector3 moveDirection = target - transform.position;
        if (moveDirection.magnitude < 0.5)
        {
            if (curTime == 0)
            {
               
                curTime = Time.time;        //pause over waypoint
                dogAnimator.SetBool("walk", false);
            }
            if ((Time.time - curTime) >= pauseDuration)
            {
                currentWaypoint++;
                curTime = 0;
            }
        }
        else
        {
            //this.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
            //rotate enemy to next waypoint
            Quaternion rotation = Quaternion.LookRotation(target - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
            character.Move(moveDirection.normalized * speed * Time.deltaTime);
            dogAnimator.SetBool("walk", true);
        }

    }

    //move to target
    void MoveToTarget(Transform target)
    {
        if (!waypoint.Contains(target))
        {
            bellen.Play();
            waypoint.Add(target);
        }
        currentWaypoint = waypoint.Count;
        Move();
    }

    public IEnumerator Wait(float time)
    {
        
        //sRend_mark.enabled = true;
        yield return new WaitForSeconds(time);
        //sRend_mark.enabled = false;
        
    }

    public IEnumerator Waiting(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public IEnumerator Search(float time)
    {
        if (time!=0)
        {
            waypoint.Remove(Target.transform);
            
            //sRend_qmark.enabled = true;
            yield return new WaitForSeconds(time);
            //sRend_qmark.enabled = false;
        }

        transform.position = waypoint[waypoint.Count - 1].position;
        Move();
    }

    public IEnumerator LosePickUp(float time)
    {
        getPickUp = false;
        waypoint.Remove(pickUp);

        yield return new WaitForSeconds(time);
        character.enabled = true;
        boxC.enabled = true;
        getPickUp = true;
        currentWaypoint = 0;
        Move();
    }

    void ReturnToWayPoint()
    {
        if (currentWaypoint != 0)
        {
            currentWaypoint = 0;
        }
        if (waypoint.Contains(Target.transform))
        {
            waypoint.Remove(Target.transform);
        }
    }

    public bool CheckForHidden()
    {
        for (int i = 0; i <= (hiddenList.Count-1); i++)
        {
            if (hiddenList[i] != null)
            {
                if (hiddenList[i].GetComponent<Hidden>().hide)
                {
                    return true;
                }
            }           
        }
        return false;       
    }

    void AnimationControll()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Player");

        Player_Controller player_Controller = enemy.GetComponent<Player_Controller>() as Player_Controller;
        if (player_Controller.IsAttack)
        {
            canMove = false;
            dogAnimator.SetBool("walk", false);
            //transform.Find("FieldOfWiew").gameObject.SetActive(false);
        }
        else
        {
            canMove = true;
            //dogAnimator.SetBool("walk", true);
        }

    }
}
