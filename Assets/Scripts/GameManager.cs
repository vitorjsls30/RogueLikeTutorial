using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public float levelStartDelay = 2f;                      //Time to wait before starting level, in seconds
    public float turnDelay = .1f;                           //Delay between each Player Turn
    public int playerFoodPoints = 100;                      //The player's total amount of food points
    public static GameManager instance = null;              //Static instance of GameManager wich allows it to be accessed by any other script
    [HideInInspector]
    public bool playersTurn = true;                         //playersTurn is public but wont show up in the inspector
    
    private Text levelText;                                 //Text to display current level Number
    private GameObject levelImage;                          //Image to block out levelas levels are being set up, background for levelText
    public BoardManager boardScript;                        //Store a reference to our board manager wich will set up the level
    private int level = 1;                                      //Current level number, expressed in game as "Day 1"
    public List<Enemy> enemies;
    public bool enemiesMoving;
    private bool doingSetup;                                //Boolean to check if we're setting up board, prevent Player from moving during setup.

    // Awake is always called before any Start functions
    void Awake () {
	    //Check if instance already exists
        if(instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloding scene
        DontDestroyOnLoad(gameObject);

        //Assign enemies to a new List of Enemy Objects
        enemies = new List<Enemy>();

        //Get a component reference to the attached BoardManager script
        boardScript = GetComponent<BoardManager>();

        //Call the init game function to initialize the first level
        InitGame();
	}

    //This is called each time a scene is loaded
    private void OnLevelWasLoaded(int index)
    {
        //Add 1 to our level number
        level++;
        //Call InitGame to Initialize our lvel
        InitGame();
    }
    
    public void GameOver()
    {
        levelText.text = "After " + level + " days, you starved.";
        levelImage.SetActive(true);
        enabled = false;
    }

    //Initilizes the Game for each level
    void InitGame ()
    {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        //Clear any Enemy object in our List to prepare for the next level
        enemies.Clear();

        //Call the SetupScene function of hte BoardManager script, pass it current level number
        boardScript.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

	// Update is called once per frame
	void Update () {
        //Check that playersTurn or enemiesMoving or doingSetup are not currently true
        if(playersTurn || enemiesMoving || doingSetup)
        {
            //If any of these are true, return and do not start MoveEnemies.
            return;
        }

        //Start Moving Enemies
        StartCoroutine(MoveEnemies());
    }

    //Call this to Add the passed in Enemy to the List of Enemy Objects
    public void AddEnemyToList(Enemy script)
    {
        //Add Enemy to List enemies
        enemies.Add(script);
    }
    
    //Coroutine to move enemies in sequence
    IEnumerator MoveEnemies()
    {
        //While enemiesMoving is true player is unable to move
        enemiesMoving = true;

        //Wait for turnDelay seconds, defaults to .1 (100ms)
        yield return new WaitForSeconds(turnDelay);

        //If there are no enemies spawned (IE in the first level)
        if(enemies.Count == 0)
        {
            //Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
            yield return new WaitForSeconds(turnDelay);
        }

        //Loop through List of Enemy Objects
        for (int i = 0; i < enemies.Count; i++)
        {
            //Call the MoveEnemy function of Enemy at index i in the enemies List
            enemies[i].MoveEnemy();

            //Wait for enemy's moveTime before moving next Enemy
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        //Once enemies are done moving, set playersTurn to be true so player can move
        playersTurn  = true;

        //enemies are done moving, set enemiesMoving to false
        enemiesMoving = false;
    }
}
