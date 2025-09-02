using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<int> resourcesSaved;
    public GameObject title;
    public GameObject controls;
    public GameObject menuGameObject;

    public GameObject levelLine;

    public GameObject creditsText;
    public bool inMainMenu;
    public List<GameObject> enemies;

    public bool transitionToggled;
    public bool deathScreenToggled;

    public GameObject transitionPoint;
    public GameObject deathScreen;

    public int currentStageNumber;
    public List<Stages> stages;
    PlayerMovement playerInfo;

    public GameObject dialogue;
    public GameObject dialogueName;
    public GameObject dialogueLine;
    public TextMeshProUGUI dialogueNameText;
    public TextMeshProUGUI dialogueLineText;

    public GameObject switchToEndButton;
    public GameObject fightButton;

    public GameObject tempFather;
    public GameObject musicButton;
    public bool isDialogueActive;
   
    public List<DoorScript> doors;

    AudioSource song;
    private void OnEnable()
    {
        References.levelManager = gameObject;
    }

    void Start()
    {
        fightButton.SetActive(true);
        switchToEndButton.SetActive(false);
        //inMainMenu = true;
        currentStageNumber = 0;
        playerInfo = References.player.GetComponent<PlayerMovement>();
        SaveResources();

        Time.timeScale = 0; // ENABLE BEFORE COMPILATION

        song = GetComponent<AudioSource>();
        // ShowDialogue();
        if (musicButton.GetComponent<MusicController>().musicOn) song.Play();
    }

    public void MusicButton()
    {
        if (musicButton.GetComponent<MusicController>().musicOn) song.Play();
        else song.Stop();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetButtonDown("Menu") && !inMainMenu)
        {
            MenuOnOff();
        }

        //Progression
        if (!transitionToggled && enemies.Count == 0 && stages[currentStageNumber].transitionRight)
        {
            transitionPoint.SetActive(true);
            transitionToggled = true;
            if (controls.activeInHierarchy)
            {
                ControlsOnOff();
            }
        }

        //DEATH
        if (!deathScreenToggled && !References.player.activeInHierarchy)
        {
            deathScreenToggled = true;
            transitionToggled = true;
            deathScreen.SetActive(true);
        }
    }
    public void MenuOnOff()
    {
        if (menuGameObject.activeInHierarchy)
        {
            menuGameObject.SetActive(false);
            Time.timeScale = 1;

            if (isDialogueActive)
            {
                dialogue.SetActive(true);
                Time.timeScale = 0;
            }
        }
        else
        {
            if (isDialogueActive)
            {
                dialogue.SetActive(false);
            }
            menuGameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void SaveResources()
    {
        resourcesSaved.Clear();
        resourcesSaved.Add(playerInfo.myCoinsNumber);
        resourcesSaved.Add(playerInfo.shieldsCount);
        resourcesSaved.Add(playerInfo.knivesCount);
    }
    public void CreditsOnOff()
    {
        if (creditsText.activeInHierarchy)
        {
            creditsText.SetActive(false);
            title.SetActive(true); //to avoid overlap

        }
        else
        {
            creditsText.SetActive(true);
            title.SetActive(false);//to avoid overlap
            controls.SetActive(false);//rarely useful
        }
    }
    public void ControlsOnOff()
    {
        if (controls.activeInHierarchy)
        {
            controls.SetActive(false);
            title.SetActive(true); //to avoid overlap
        }
        else
        {
            controls.SetActive(true);
            title.SetActive(false);//to avoid overlap
            creditsText.SetActive(false); //rarely useful
        }
    }
    public void Resume()
    {
        menuGameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void StartNewGame()
    {
        Time.timeScale = 1;
        //SceneManager.LoadScene("Map");
        // SPAWN LEVEL 1
        switchToEndButton.SetActive(false);
        fightButton.SetActive(true);

        inMainMenu = false; //can call the menu again
        menuGameObject.SetActive(false);
        currentStageNumber = 0;
        tempFather.SetActive(true);
        ShowDialogue();
    }

    public void RestartSection()                       //CURRENT STRUGGLE
    {
        Debug.Log("number enemies: " + enemies.Count);
        for (int i = 0; i < enemies.Count; i++)
        {
            Debug.Log ("destroying: "+ i +" - " + enemies[i]);
            Destroy(enemies[i]);
        }
        enemies.Clear();

        //StartNewSection();
        deathScreen.SetActive(false);
        References.player.SetActive(true);
        playerInfo.ableToMove = false;

        deathScreenToggled = false;


        playerInfo.myCoinsNumber = resourcesSaved[0];
        playerInfo.shieldsCount = resourcesSaved[1];
        playerInfo.knivesCount = resourcesSaved[2];
        StartCoroutine(ResetPlayerPosition(-7, 0.5f));
    }
    public void StartNewSection()
    {

        //coroutine to move the player to START position on the left
        StartCoroutine(ResetPlayerPosition(-7, 0.5f));
        //spawn enemies
        //dialogue count ++
        //current level ++
        //check list <enemies to spawn>, list<x.coordinates for each enemy>
    }
    private IEnumerator ResetPlayerPosition(float x, float duration)
    {
        Vector3 startPos = References.player.transform.position;
        Vector3 endPos = new Vector3(x, startPos.y, startPos.z);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            References.player.transform.position = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        References.player.transform.position = endPos;
       // transitionToggled = false; //for later enable 
        ShowDialogue();
    }
    public void ShowDialogue() //activates by LEAVE SHOP button
    {
        Stages currentStageScript = stages[currentStageNumber];

        if (currentStageScript.hasIntro)
        {
            Time.timeScale = 0; //pause for dialogue
            playerInfo.ableToMove = false;

            if (currentStageNumber >= stages.Count - 1)
            {
                switchToEndButton.SetActive(true);
                fightButton.SetActive(false);

                //  return;
            }
            dialogue.SetActive(true);
            isDialogueActive = true;  //NEEDED for MENU activation
                                      //dialogueLine.SetActive(true);
            dialogueNameText.text = currentStageScript.dialogueName[0].ToString();
            dialogueLineText.text = currentStageScript.dialogueLines[0].ToString();
        }else playerInfo.ableToMove=true;
    }

    public void SpawnFighters() //Activates by START FIGHT button
    {
        if (tempFather.activeInHierarchy)
        {
            tempFather.SetActive(false);
        }
        dialogue.SetActive(false);
        isDialogueActive = false;

        // dialogueLine.SetActive(false);
        //Spawn Fighters
        int n = currentStageNumber;
        Stages stageToSpawn = stages[n];
        for (int i = 0; i < stageToSpawn.enemies.Count; i++)
        {
            float xCoord = stageToSpawn.position[i];
            Vector2 spawnPosition = new Vector2(xCoord, levelLine.transform.position.y);
            Instantiate(stageToSpawn.enemies[i], spawnPosition, Quaternion.identity);
        }

        //let the player move again
        playerInfo.ableToMove = true;
        transitionToggled = false; //for later enable 
        Time.timeScale = 1; //pause for dialogue

    }
    private void OnDestroy()
    {
        References.levelManager = null;
    }
}
