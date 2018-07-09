using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

    [HideInInspector]
    public Renderer rend;
    public GameObject player;
    public GameObject spawn;
    public GameObject depot;
    public GameObject target;
    public EnemyStateMachine enemy;
    private bool rotator;
    public float wait;
    private Rigidbody rb;
    public float speed;
    public bool hasPickUp = false;
    public bool pick;
    int i = 1;
    public Player_Controller powerupsHold;
    private bool interact =false;


    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        this.transform.position = spawn.transform.position;
        rend = this.GetComponent<Renderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        rotator = true;
        pick = false;
	}
	
    void Knochen()
    {

        if (i == 1)
        {
            this.transform.position = player.transform.position;
            i++;
        }
    }

	void Update ()
    {
        if(Input.GetKey(KeyCode.E) && powerupsHold.pickups >= 1 && interact)
        {
            Knochen();
            rend.enabled = true;
            while (this.gameObject.transform.position != target.transform.position)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
            }
            interact = false;
            powerupsHold.pickups--;
            StartCoroutine(MoveOn("NEW"));
            
            
        }


        if(rotator==true)
        {
                transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        }

        if(powerupsHold.killed == true)
        {
            pick = true;
            StartCoroutine(MoveOn("Hallo"));
           
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (powerupsHold.pickups == 0)
        {
            if (other.tag == "Player" && !hasPickUp)
            {
                rend.enabled = false;
                rotator = false;
                this.transform.position = depot.transform.position;
                powerupsHold.pickups++;
                hasPickUp = true;
                interact = true;
            }
        }

    if (other.tag == "Enemy")
        {
            pick = true;
            StartCoroutine(MoveOn("Hallo"));
            //Christian, Bugfixing, 27.01.17
            //begin
            enemy = other.GetComponent<EnemyStateMachine>();
            if (enemy.waypoint.Contains(transform))
            {
                enemy.waypoint.Remove(transform);
            }
            //end

        }

    }

    public IEnumerator MoveOn(string hal)
    {
        hal = "Warten auf";
        yield return new WaitForSeconds(wait);
        hal = "Godot";
        i = 1;
        if(powerupsHold.killed == true)
        {
            rend.enabled = true;
            powerupsHold.killed = false;
        }
        if (hasPickUp)
        {
            hasPickUp = !hasPickUp;
        }
        rend.enabled = false;
        this.transform.position = spawn.transform.position;
        rend.enabled = true;
        rotator = true;
        pick = false;
    }
    public IEnumerator MoveTo(string to)
    {

        to = "Warten auf";
        yield return new WaitForSeconds(wait);
        to = "Godot";


    }
}
