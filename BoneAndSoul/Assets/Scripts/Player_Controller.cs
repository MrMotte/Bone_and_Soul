using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
    Name:       Samuel Bohren
    Date:       11.01.2017
    Function:   Add Animation
*/

public class Player_Controller : MonoBehaviour {

    public AudioSource a;
    public AudioSource b;
    public AudioSource c;
    public float speed = 1;
    public Transform spawnPoint;
    public int maxLife = 3;
    public int life;
    public float timeToWait = 5f;
    public Vector3 position;
    public Vector3 goalPosition;
    public bool hidden = false;
    public float step = 0.1f;
    public GameObject animGameObject;
    public GameObject GameManagerGO;
    [HideInInspector]
    public GameObject enemy;
    public Hidden hide;
    public Text textLife;
    public Text textGameOver;
    public Text textWin;
    public Text textMaxLife;
    public float pickups;
    public bool killed; //additional bool to compare death for pickups

    public GameObject blinkenTod;
    public GameObject blinkenSense;

    private MeshRenderer rend;
    private CharacterController cont;
    public GameObject[] lifeBones;

    public AudioSource womanDied;
    public AudioSource playerDied;
    public GameObject walkSound;
    public AudioSource breakingBones;
    public AudioSource Tür;

    public float waitCry;

    [HideInInspector]
    public bool loseLife = false;
    [HideInInspector]
    public bool victory = false;
    [HideInInspector]
    public bool gameOver = false;

    GridDemo grid;

    Animator anim;

    public bool IsAttack = false;

    public bool victoryone = false;
    public bool victorytwo = false;
    public bool victorythree = false;

    private bool isWinning = false;


    public void Init()
    {
        gameObject.SetActive(true);
    }

    void Start()
    {
        GameManagerGO = GameObject.Find("GameManager");
        cont = this.GetComponent<CharacterController>();
        grid = GetComponent<GridDemo>();
        rend = GetComponent<MeshRenderer>();
        position = transform.position;
        life = maxLife;
        pickups = 0;

        blinkenSense = GameObject.Find("Cube.001");
        blinkenTod = GameObject.Find("Tod");

        //textMaxLife.text = " / 3";

        anim = GetComponent<Animator>();

        if (enemy != null)
        {
            enemy.GetComponent<EnemyStateMachine>();
        }
    }

    void Update()
    {
        hidden = enemy.GetComponent<EnemyStateMachine>().playerIsHidden;
    }

    void FixedUpdate()
    {

        Movement();
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy") && !hidden)
        {
            //added, Christian, 26.01.17
            //begin
            pickups = 0;
            killed = true;
            //end
            StartCoroutine(Restart(""));
            life--;
            loseLife = true;
            transform.position = spawnPoint.position;
            position = transform.position;
            textLifeUpdate();
            Invoke("SetFalse", 1f);
            

            //PlayAnim();
            if (life == 0)
            {
                //change GMState to GameOver
                if (GameManagerGO != null)
                {
                    // set immediatly the gameOverScreen (  later invoke it)
                    //GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOverState);
                }
                gameOver = true;
                //gameObject.SetActive(false);
                //Invoke("SetFalse", 1f);
            }
        }
        if (other.CompareTag("House"))
        {
            //Win-Condition
            anim.SetBool("DoorOpen", true);
            anim.SetBool("IsWalk", false);
            Invoke("Winning", 5);
            isWinning = true;
            //womanDied.Play();
            StartCoroutine(doorAndCry("New"));
            victory = true;
            Invoke("SetFalse", 1f);
            if (GameManagerGO != null)
            {
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Gameplay_level1")
                {
                    victoryone = true;
                    a.Play();
                }
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Gameplay_level2")
                {
                    victorytwo = true;
                    b.Play();
                }
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Gameplay_level3")
                {
                    victorythree = true;
                    c.Play();
                }
                //set immediatly the victoryScreen (later invoke it)
                //GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.VictoryState);

                //Christian, 04.02.17 // begin
                if(GameManagerGO.GetComponent<GameManager>().lastLevel == 1 && 
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Gameplay_level3")
                {
                    GameManagerGO.GetComponent<GameManager>().lastLevel = 0;
                    GameManagerGO.GetComponent<GameManager>().str = "StartScene";
                }

                if (GameManagerGO.GetComponent<GameManager>().lastLevel == 1)
                {
                    GameManagerGO.GetComponent<GameManager>().str = "Gameplay_level1";
                    GameManagerGO.GetComponent<GameManager>().victoryMenu.enabled = true;
                    GameManagerGO.GetComponent<GameManager>().victory_nextLevel.enabled = true;
                    //GameManagerGO.GetComponent<GameManager>().Clear();
                    //GameManagerGO.GetComponent<GameManager>().HandleButtons();
                    Debug.Log("Nummer 1 lebt");
                    //Time.timeScale = 0.0f;
                }
                if (GameManagerGO.GetComponent<GameManager>().lastLevel == 2)
                {
                    GameManagerGO.GetComponent<GameManager>().str = "Gameplay_level2";
                    GameManagerGO.GetComponent<GameManager>().victoryMenu.enabled = true;
                    GameManagerGO.GetComponent<GameManager>().victory_nextLevel.enabled = true;
                    //GameManagerGO.GetComponent<GameManager>().Clear();
                    //GameManagerGO.GetComponent<GameManager>().HandleButtons();
                    Debug.Log("Nummer 2 lebt");
                    //Time.timeScale = 0.0f;
                }
                if (GameManagerGO.GetComponent<GameManager>().lastLevel == 3)
                {
                    GameManagerGO.GetComponent<GameManager>().lastLevel = 0;
                    GameManagerGO.GetComponent<GameManager>().str = "Gamplay_level3";
                    GameManagerGO.GetComponent<GameManager>().victoryMenu.enabled = true;
                    GameManagerGO.GetComponent<GameManager>().victory_nextLevel.enabled = true;
                    //GameManagerGO.GetComponent<GameManager>().Clear();
                    //GameManagerGO.GetComponent<GameManager>().HandleButtons();
                    Debug.Log("Nummer 3 lebt");
                    //Time.timeScale = 0.0f;
                }
                if(GameManagerGO.GetComponent<GameManager>().lastLevel == 0 &&
                    GameManagerGO.GetComponent<GameManager>().lastLevel == 4)
                {
                    GameManagerGO.GetComponent<GameManager>().str = "StartScreen";
                    GameManagerGO.GetComponent<GameManager>().lastLevel = 0;
                    GameManagerGO.GetComponent<GameManager>().victoryMenu.enabled = true;
                    GameManagerGO.GetComponent<GameManager>().victory_nextLevel.enabled = true;
                    
                }
                if (!GameManagerGO.GetComponent<GameManager>().restart)
                {
                    ++GameManagerGO.GetComponent<GameManager>().lastLevel;
                    Time.timeScale = 0.0f;
                }
                //end
            }

            //Christian, Bugfixing, 05.02.17 // begin
            if (GameManagerGO.GetComponent<GameManager>().victoryMenu != null)
            {
                GameManagerGO.GetComponent<GameManager>().hasVictoryMenu = true;
                //GameManagerGO.GetComponent<GameManager>().victory_nextLevel.GetComponent<Button>().onClick.AddListener(GameManagerGO.GetComponent<GameManager>().NextLevel);
            }
            //end
        }
    }

    void PlayAnim()
    {
        GameObject anim = (GameObject)Instantiate(animGameObject);
        //set position of anim 
        anim.transform.position = transform.position;
    }

    void textLifeUpdate()
    {
        if (life == 2)
        {
            lifeBones[2].SetActive(false);
            lifeBones[5].SetActive(true);
            breakingBones.Play();
        }
        if (life == 1)
        {
            lifeBones[1].SetActive(false);
            lifeBones[4].SetActive(true);
            breakingBones.Play();
        }
        if (life == 0)
        {
            lifeBones[0].SetActive(false);
            lifeBones[3].SetActive(true);
            breakingBones.Play();
            //PLAY DYE SOUND
            playerDied.Play();
            //GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOverState);

            GameManagerGO.GetComponent<GameManager>().gameOverMenu.enabled = true;
            GameManagerGO.GetComponent<GameManager>().gameOver_BTS.enabled = true;
            GameManagerGO.GetComponent<GameManager>().gameOver_restart.enabled = true;


            
            if (GameManagerGO.GetComponent<GameManager>().gameOverMenu != null)
            {
                GameManagerGO.GetComponent<GameManager>().hasGameOverMenu = true;
                Time.timeScale = 0.0f;
            }
            /*else
            {
                GameManagerGO.GetComponent<GameManager>().gameOverMenu = GameObject.Find("GameOverMenu").GetComponent<Canvas>();
                if (GameManagerGO.GetComponent<GameManager>().gameOverMenu != null)
                {
                    GameManagerGO.GetComponent<GameManager>().hasGameOverMenu = true;
                    Time.timeScale = 0.0f;
                }
            }*/
        }
    }

    void Movement()
    {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                position = transform.position - new Vector3(step, 0, 0);
                position.x = Mathf.Round(position.x);
                transform.eulerAngles = new Vector3(0, -90, 0);
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                position = transform.position - new Vector3(-step, 0, 0);
                position.x = Mathf.Round(position.x);
                transform.eulerAngles = new Vector3(0, 90, 0);
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                position = transform.position - new Vector3(0, 0, -step);
                position.z = Mathf.Round(position.z);
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                position = transform.position - new Vector3(0, 0, step);
                position.z = Mathf.Round(position.z);
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            Vector3 dir = position - transform.position;
            Vector3 move = dir.normalized * Time.deltaTime * speed;
            if (move.magnitude > dir.magnitude)
            {
                cont.Move(dir);
            }
            else
            {
                cont.Move(move);
            }

            // walk cycles
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ||
                Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                anim.SetBool("IsWalk", true);
            //PLAY WALK-SOUND
            walkSound.SetActive( true);
                
            }
            else
            {
                anim.SetBool("IsWalk", false);
                walkSound.SetActive(false);
            }
    }

    void SetFalse()
    {
        IsAttack = false;
        victory = false;
        gameOver = false;
        loseLife = false;
    }

    IEnumerator Restart(string str)
    {
        if (life != 1)
        {
            for (int i = 1; i < 5; i++)
            {
                blinkenTod.SetActive(false);
                blinkenSense.SetActive(false);
                yield return new WaitForSeconds(0.1f);
                blinkenTod.SetActive(true);
                blinkenSense.SetActive(true);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public IEnumerator doorAndCry(string to)
    {

        to = "Warten auf";
        Tür.Play();
        yield return new WaitForSeconds(waitCry);
        womanDied.Play();
        to = "Godot";

    }

        void Winning()
    {
        isWinning = false;
        anim.SetBool("DoorOpen", false);
        anim.SetBool("IsWalk", true);
    }
}
