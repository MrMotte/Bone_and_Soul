using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject placeholder;
    public Player_Controller playCont;

    public GameObject pauseGO;

    public static GameManager instance = null;

    public Scene StartScene;
    public Scene GameplayScene;
    public Scene GameOverScene;

    bool vicScreenShown = false;
    bool goScreenShown = false;

    bool paused = false;

    public Canvas quitMenu;
    public Canvas optionsMenu;
    public Canvas creditsMenu;
    public Button startText;
    public Button exitText;
    public Button creditsButton;
    public Button optionsButton;
    public Slider volumeSlider;

    //new Christian, 01.02.17 //begin
    public Canvas victoryMenu;
    public Canvas gameOverMenu;
    public Canvas pauseMenu;
    public Button pause_BTG;
    public Button pause_BTS;
    public Button pause_restart;
    public Button victory_nextLevel;
    public Button victory_restart;
    public Button victory_BTS;
    public Button gameOver_BTS;
    public Button gameOver_restart;
    //03.02.17
    public Button quitYes;
    public Button quitNo;
    public Button creditBack;
    public Button optionBack;

    public bool hasPauseMenu = false;
    public bool hasVictoryMenu = false;
    public bool hasGameOverMenu = false;
    public bool restart = false;
    public bool BTS = false;
    public bool nextLevel = false;
    public bool hasStartConfig = false;

    public bool hasQuitM;
    public bool hasOMenu;
    public bool hasCMenu;
    public bool hasStart;
    public bool hasExit;
    public bool hasCButton;
    public bool hasOButton;
    public bool hasVSlider;
    //end

    //Christian, Bugfixing, 07.02.17 // begin
    public bool lastLevelPause = false;
    public bool lastLevelVictory = false;
    public bool lastLevelGameOver = false;
    //end

    public float lastLevel = 0;
    public string str = "";

    public float waitFading;
    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.8f;

    private int drawDepth = -1000;
    private float alpha = 1;
    private int fadeDir = -1;

    public enum GameManagerState
    {
        StartState,
        Gameplay_level1,
        Gameplay_level2,
        Gameplay_level3,
        GameOverState,
        PauseState,
        VictoryState

    }
    GameManagerState GMState;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        GMState = GameManagerState.StartState;
        if (player != null && playCont != null)
        {
            playCont = player.GetComponent<Player_Controller>();
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playCont = player.GetComponent<Player_Controller>();
            }
        }
    }

    void Update()
    {
       /* if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FadingStart("Gameplay_level3"));
        }
        */
        if(SceneManager.GetActiveScene().name == "Gamplay_level3")
        {

            ForLastLevel();
            str = "Gameplay_level3";
            lastLevel = 3;
            if(hasVictoryMenu && victoryMenu != null)
            {
                victoryMenu.enabled = true;
            }
        }

        if(SceneManager.GetActiveScene().name == "Gameplay_level2")
        {
            if(GameObject.Find("Player_2.0").GetComponent<Player_Controller>().victorytwo)
            {
                Debug.Log("2tes Level gewonnen");
            }
        }

        //Christian, 08.02.17
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Gameplay_level3")
        {
            str = "Gameplay_level3";
            lastLevel = 3;
        }

        HandleState();

        HandleButtons();

        Pause();

        GetStartConfig();

        //CheckLevel();

        //Test();

        //LastLevelPause();

        if (playCont != null)
        {
            if ((GMState == GameManagerState.Gameplay_level1 || GMState == GameManagerState.Gameplay_level2) && playCont.victory && !vicScreenShown)
            {
                Invoke("Victory", 2f);
                vicScreenShown = true;
            }
            if ((GMState == GameManagerState.Gameplay_level1 || GMState == GameManagerState.Gameplay_level2) && playCont.gameOver && ! goScreenShown)
            {
                Invoke("GameOver", 3f);
                goScreenShown = true;
            }

        }
        
        
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    paused = togglePause();
        //}

        if (player != null && playCont != null)
        {
            playCont = player.GetComponent<Player_Controller>();
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playCont = player.GetComponent<Player_Controller>();
            }
        }

        //PAUSE
        /*
        if (pauseGO == null)
        {
            pauseGO = GameObject.FindGameObjectWithTag("Pause");
            if (pauseGO != null && paused)
            {
                pauseGO.GetComponent<Image>().enabled = true;
            }
            else if(pauseGO != null && !paused)
            {
                pauseGO.GetComponent<Image>().enabled = false;
            }
        }
        else
        {
            if (paused && pauseGO != null)
            {
                pauseGO.GetComponent<Image>().enabled = true;
            }
            else
            {
                pauseGO.GetComponent<Image>().enabled = false;
            }
        }
        */
        //PAUSE
    }

    //update GMState
    void UpdateGameManagerState()
    {
        switch (GMState)
        {

            case GameManagerState.StartState:
                vicScreenShown = false;
                goScreenShown = false;
                //start scene

                //SceneManager.LoadScene("StartScreen");

                //UnityEngine.SceneManagement.SceneManager.LoadScene("StartScreen");
                StartCoroutine(FadingStart("StartScreen"));
                break;

                //load first level
            case GameManagerState.Gameplay_level1:
                //reset score/timer
                //show player while playing
                // set enemys active
                //load scene
                lastLevel += 1;
                //UVictoryScreen
                StartCoroutine(FadingStart("Gameplay_level1"));
                break;

                //load second level
            case GameManagerState.Gameplay_level2:
                lastLevel += 1;
                //UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay_level2");
                StartCoroutine(FadingStart("Gameplay_level2"));
                break;

                //load third level
            case GameManagerState.Gameplay_level3:
                lastLevel += 1;
                //UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay_level3");
                StartCoroutine(FadingStart("Gameplay_level3"));
                break;

            case GameManagerState.GameOverState:
                //set enemys inactive
                //show GameOver screen
                //UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScreen");
                StartCoroutine(FadingStart("GameOverScreen"));
                break;

            case GameManagerState.PauseState:
                break;

            case GameManagerState.VictoryState:
                //load victory screen
                //UnityEngine.SceneManagement.SceneManager.LoadScene("VictoryScreen");
                StartCoroutine(FadingStart("VictoryScreen"));
                break;
        }
    }
    //Set state 
    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        UpdateGameManagerState();
    }

    public void Victory()
    {
        SetGameManagerState(GameManagerState.VictoryState);
    }

    public void GameOver()
    {
        SetGameManagerState(GameManagerState.GameOverState);
    }

    //Set state to opening
    public void ChangeToStart()
    {
        GMState = GameManagerState.StartState;
        UpdateGameManagerState();
    }

    void HandleState()
    {
        //check if player is in victory screen
        if(GMState == GameManagerState.VictoryState)
        {
            //if (Input.GetKeyDown)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //if yes, load level with index of lastLevel plus one
                if (lastLevel == 1)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay_level1");
                    lastLevel = 2;
                }
                if (lastLevel == 2)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay_level2");
                    lastLevel = 3;
                }
                if (lastLevel == 3)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay_level3");
                    //define level as
                    lastLevel = 0;
                }
                else
                {
                    //
                }
            }
        }
        
        //use controls to start (if input.getkey)

        //check if player is in gameOver screen
        //if yes, load level with index of last level
        //use controls to start (if input.getkey)
    }

    //pause the Game
    bool togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            return (true);
        }
    }

    void Start()
    {
        volumeSlider.value = 1;
        quitMenu = quitMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
        creditsButton = creditsButton.GetComponent<Button>();
        optionsButton = optionsButton.GetComponent<Button>();

        quitMenu.enabled = false;
        optionsMenu.enabled = false;
        creditsMenu.enabled = false;

    }

    public void ExitPress()
    {
        quitMenu.enabled = true;
        startText.enabled = false;
        exitText.enabled = false;
        creditsButton.enabled = false;
        optionsButton.enabled = false;
    }

    public void NoPress()
    {
        quitMenu.enabled = false;
        creditsMenu.enabled = false;
        optionsMenu.enabled = false;
        startText.enabled = true;
        exitText.enabled = true;
        creditsButton.enabled = true;
        optionsButton.enabled = true;
    }

    public void StartLevel()
    {
        //use right GamePlayScene name
        //if level one starts set lastlevel to one
        Clear();
        lastLevel = 1;
        StartCoroutine(FadingStart(str));
        //UnityEngine.SceneManagement.SceneManager.LoadScene(str);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    //Display OptionsMenu
    public void OptionsPress()
    {
        startText.enabled = false;
        exitText.enabled = false;
        creditsButton.enabled = false;
        optionsButton.enabled = false;
        optionsMenu.enabled = true;
        creditsMenu.enabled = false;
        quitMenu.enabled = false;
    }

    //Display CreditsMenu
    public void CreditsPress()
    {
        startText.enabled = false;
        exitText.enabled = false;
        creditsButton.enabled = false;
        optionsButton.enabled = false;
        quitMenu.enabled = false;
        optionsMenu.enabled = false;
        creditsMenu.enabled = true;
    }

    public void SetMasterVolume(float value)
    {
        GetComponent<AudioListener>();
        AudioListener.volume = volumeSlider.value;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        if (isFullScreen)
        {
            Resolution[] allResolutions = Screen.resolutions;
            Resolution maxResolution = allResolutions[allResolutions.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }
        else
        {
            Resolution[] allResolutions = Screen.resolutions;
            Resolution maxResolution = allResolutions[allResolutions.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, false);
        }
    }

    public IEnumerator FadingStart(string to)
    {
        BeginFade(1);
        yield return new WaitForSeconds(waitFading);
        UnityEngine.SceneManagement.SceneManager.LoadScene(to);
        BeginFade(-1);
    }

    void OnGUI()
    {

        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    public float BeginFade(int dir)
    {
        fadeDir = dir;

        return (fadeSpeed);
    }

    public void HandleButtons()
    {
        //PauseMenu
        if (!hasPauseMenu && !restart && !BTS)
        {
            //Pause - Buttons
            if (GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<Canvas>())
            {
                pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<Canvas>();
            }
            if (!pause_BTG)
            {
                pause_BTG = GameObject.Find("BackToGame_button_p").GetComponent<Button>();
                pause_BTG.onClick.AddListener(BackToGame);
            }
            if (!pause_BTS)
            {
                pause_BTS = GameObject.Find("BackToStart_button_p").GetComponent<Button>();
                pause_BTS.onClick.AddListener(BackToStart);
                BTS = false;
            }
            if (!pause_restart)
            {
                pause_restart = GameObject.Find("Restart_button_p").GetComponent<Button>();
                pause_restart.onClick.AddListener(Restart);
                //restart = false;
            }
            
            if (pauseMenu != null)
            {
                if (!paused)
                {
                    pauseMenu.enabled = false;
                    //hasPauseMenu = true;
                }
            }
            
        }

        if(BTS && SceneManager.GetActiveScene().name == "StartScreen")
        {
            //Destroy(this.gameObject);
        }
        //Victory - Menu
        if (!hasVictoryMenu && !BTS)
        {
            //Victory - Buttons
            victoryMenu = GameObject.FindGameObjectWithTag("VictoryMenu").GetComponent<Canvas>();
            if (!victory_BTS)
            {
                victory_BTS = GameObject.Find("BackToStart_button_v").GetComponent<Button>();
                victory_BTS.onClick.AddListener(BackToStart);
                BTS = false;
            }
            //if "Next Level"-Button in VictoryMenu is null
            if (!victory_nextLevel)
            {
                    victory_nextLevel = GameObject.Find("NextLevel_button_v").GetComponent<Button>();
                if (victory_nextLevel != null)
                {
                    victory_nextLevel.onClick.AddListener(NextLevel);
                    nextLevel = true;
                }
            }
            if (!victory_restart)
            {
                victory_restart = GameObject.Find("Restart_button_v").GetComponent<Button>();
                victory_restart.onClick.AddListener(Restart);
                restart = false;
            }
            
            if (victoryMenu != null)
            {
                victoryMenu.enabled = false;
                //hasVictoryMenu = true;
            }
            
        }


        //Christian, Bugfixing, 06.02.17 // begin
        if (hasVictoryMenu)
        {
            if (victory_nextLevel != null)
            {
                victory_nextLevel = null;
                victory_nextLevel = GameObject.Find("NextLevel_button_v").GetComponent<Button>();
                victory_nextLevel.onClick.AddListener(NextLevel);
            }
        }
        //end


        //GameOver - Menu
        if (!hasGameOverMenu && !BTS)
        {
            //GameOver - Buttons
            gameOverMenu = GameObject.FindGameObjectWithTag("GameOverMenu").GetComponent<Canvas>();
            if (!gameOver_BTS)
            {

                gameOver_BTS = GameObject.Find("BackToStart_button_g").GetComponent<Button>();
                gameOver_BTS.onClick.AddListener(BackToStart);
                BTS = false;
            }
            if (!gameOver_restart)
            {
                gameOver_restart = GameObject.Find("Restart_button_g").GetComponent<Button>();
                gameOver_restart.onClick.AddListener(Restart);
                restart = false;
            }
            
            if (gameOverMenu != null)
            {
                gameOverMenu.enabled = false;
                //hasGameOverMenu = true;
            }
            
        }
        /*
                if (gameOverMenu != null)
                {
                    gameOverMenu.enabled = false;
                }
                if(victoryMenu != null)
                {
                    victoryMenu.enabled = false;
                }
                if (pauseMenu != null)
                {
                    pauseMenu.enabled = false;
                }
                */
        //StartCoroutine(BUttonsOnOff(""));
    }

    IEnumerator BUttonsOnOff(string str)
    {
        str = "";
        yield return new WaitForSeconds(0.1f);
        hasQuitM = true;
        hasVictoryMenu = true;
        hasPauseMenu = true;
        str = "";
    }

    public void Pause()
    {
        if (pauseMenu && Time.timeScale == 1.0f && !paused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                paused = togglePause();
                pauseMenu.enabled = true;
            }
        }
        else
        {
            if(paused)
            {
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    paused = togglePause();
                    pauseMenu.enabled = false;
                }
            }
        }
    }

    public void BackToGame()
    {
        paused = togglePause();
        pauseMenu.enabled = paused;
    }
    public void BackToStart()
    {
        Clear();
        BTS = true;
        if(Time.timeScale == 0.0f)
        {
            Time.timeScale = 1.0f;
        }
        lastLevel = 0;
        str = "Gameplay_level1";
        StartCoroutine(FadingStart("StartScreen"));
        paused = false;
        pauseMenu.enabled = false;
    }
    public void Restart()
    { 
        restart = true;
        if (Time.timeScale == 0.0f)
        {
            Time.timeScale = 1.0f;
        }
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Gameplay_level1")
        {
            str = "Gameplay_level1";
            lastLevel = 1;
        }
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Gameplay_level2")
        {
            str = "Gameplay_level2";
            lastLevel = 2;
        }
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Gameplay_level3")
        {
            str = "Gameplay_level3";
            lastLevel = 3;
        }
        StartCoroutine(FadingStart(str));
        Clear();
        hasStartConfig = false;
        HandleButtons();
        pauseMenu.enabled = false;
        victoryMenu.enabled = false;
        gameOverMenu.enabled = false;

        hasPauseMenu = false;
        hasVictoryMenu = false;
        hasGameOverMenu = false;
        paused = false;
        pauseMenu.enabled = false;
        /*
        restart = true;
        StartCoroutine(FadingStart(str));
        if (Time.timeScale == 0f)
        {
            togglePause();
        }
        Clear();
        HandleButtons();
        */
    }
    public void NextLevel()
    {
        nextLevel = true;
        victoryMenu.enabled = true;
        Test();
        if (lastLevel == 0)
        {
            str = "StartScreen";
            StartCoroutine(FadingStart(str));
            lastLevel = 1;
        }
        if (lastLevel == 1)
        {
            str = "Gameplay_level1";
            StartCoroutine(FadingStart(str));
            lastLevel = 2;
        }
        if (lastLevel == 2)
        {
            str = "Gameplay_level2";
            StartCoroutine(FadingStart(str));
            //lastLevel = 3;
        }
        if (lastLevel == 3)
        {
            str = "Gameplay_level3";
            StartCoroutine(FadingStart(str));
            //define level as
            lastLevel = 0;
            ForLastLevel();
        }
        if (lastLevel == 4)
        {
            lastLevel = 0;
            str = "StartScreen";
            StartCoroutine(FadingStart("StartScreen"));
        }
        
        Time.timeScale = 1.0f;
        nextLevel = false;
        victoryMenu.enabled = false;
        Clear();
    }

    public void Clear()
    {
        victoryMenu = null;
        gameOverMenu = null;
        pauseMenu = null;
        pause_BTG = null;
        pause_BTS = null;
        victory_BTS = null;
        victory_nextLevel = null;
        victory_restart = null;
        gameOver_BTS = null;
        gameOver_restart = null;

        BTS = false;
        nextLevel = false;
        restart = false;
        
        hasGameOverMenu = false;
        hasPauseMenu = false;
        hasVictoryMenu = false;

        hasCButton = false;
        hasCMenu = false;
        hasOMenu = false;
        hasExit = false;
        hasQuitM = false;
        hasStart = false;
        hasVSlider = false;
        hasOButton = false;

        hasStartConfig = false;
    }

    void GetStartConfig()
    {
        if(!hasStartConfig)
        {
            if (!hasQuitM)
            {
                quitMenu = GameObject.Find("QuitMenu").GetComponent<Canvas>();
                quitMenu.gameObject.GetComponent<Canvas>().enabled = false;
                hasQuitM = true;
            }
            if (!hasOMenu)
            {
                optionsMenu = GameObject.Find("OptionsMenu").GetComponent<Canvas>();
                optionsMenu.gameObject.GetComponent<Canvas>().enabled = false;
                hasOMenu = true;
            }
            if (!hasCMenu)
            {
                creditsMenu = GameObject.Find("CreditsMenu").GetComponent<Canvas>();
                creditsMenu.gameObject.GetComponent<Canvas>().enabled = false;
                hasCMenu = true;
            }
            if (!hasStart)
            {
                startText = GameObject.Find("Play_button").GetComponent<Button>();
                startText.onClick.AddListener(StartLevel);
                hasStart = true;
            }
            if (!hasExit)
            {
                exitText = GameObject.Find("Exit_button").GetComponent<Button>();
                exitText.onClick.AddListener(ExitPress);
                hasExit = true;
            }
            if (!hasCButton)
            {
                creditsButton = GameObject.Find("Credits_button").GetComponent<Button>();
                creditsButton.onClick.AddListener(CreditsPress);
                hasCButton = true;
            }
            if (!hasOButton)
            {
                optionsButton = GameObject.Find("Options_button").GetComponent<Button>();
                optionsButton.onClick.AddListener(OptionsPress);
                hasOButton = true;
            }
            if (!hasVSlider)
            {
                volumeSlider = GameObject.Find("Slider").GetComponent<Slider>();
                volumeSlider.onValueChanged.AddListener(SetMasterVolume);
                hasVSlider = true;
            }
            

            //set the buttonFunctions to their normal ones
            

            quitYes = GameObject.Find("Yes_quit").GetComponent<Button>();
            quitYes.onClick.AddListener(ExitGame);
            quitNo = GameObject.Find("No_quit").GetComponent<Button>(); 
            quitNo.onClick.AddListener(NoPress);
            optionBack = GameObject.Find("Options_back").GetComponent<Button>();
            optionBack.onClick.AddListener(NoPress);
            creditBack = GameObject.Find("Credits_back").GetComponent<Button>(); 
            creditBack.onClick.AddListener(NoPress);
            hasStartConfig = true;
        }
    }

    //Muss in Update abgefragt werden
    //wenn ins nächste level gesprungen wird, dann nimm die neuen menüs aus dem neuen level
    //schalte alle has-Bools aus, bis Scene fertig ist

    void Test()
    {
        if (nextLevel)
        {
            victoryMenu = null;
            victory_BTS = null;
            victory_nextLevel = null;
            victory_restart = null;

            hasVictoryMenu = false;
            hasGameOverMenu = false;
            hasPauseMenu = false;
        }
    }

    //Christian, 06.02.17 //begin --> obsolet
    public void ForLastLevel()
    {
        victoryMenu = GameObject.Find("VictoryMenu").GetComponent<Canvas>();
        if (victoryMenu != null)
        {
            //hasVictoryMenu = true;
        }
        pauseMenu = GameObject.Find("PauseMenu").GetComponent<Canvas>();
        if(pauseMenu != null)
        {
            //hasPauseMenu = true;
        }
    }
    //end

    public void LastLevelPause()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay_level3")
        {
            if (pauseMenu != null)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    togglePause();
                }
            }
        }
    }
}

