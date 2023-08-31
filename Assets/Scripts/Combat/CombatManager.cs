using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    [Tooltip("set true if you are only testing the combat system in this scene without any scene changes")]
    public bool m_CombatSceneOnly = true;
    [Tooltip("quick way to test the action command in the 'Action Command Prefab To Test' to make sure the mechanics work properly")]
    public bool m_QuickTestActionCommand = false;
    [Tooltip("tests this action command on play if the 'Quick Test Action Command' bool is checked")]
    public GameObject m_ActionCommandPrefabToTest;
    [Tooltip("time it takes between player action command and the enemy action command")]
    public float m_TimeBetweenAttacks = 3f;

    [Header("UI elements")]
    [Tooltip("The UI button thats starts the players attack")]
    [SerializeField]
    private GameObject m_AttackButtonPrefab;
    [Tooltip("The list of character 1's attacks to choose from")]
    [SerializeField]
    private GameObject m_AttackListUI;
    [Tooltip("The Text that gets displayed when the battle is over")]
    [SerializeField]
    private TMP_Text EndBattleDisplayText;

    [Tooltip("The Text that gets displayed when an attack is selected")]
    public Text descriptionText;
    public GameObject descriptionBox;

    [Header("Characters")]
    [Tooltip("The player (Squirrel)")]
    public Character m_Character1;
    [Tooltip("The CPU Enemy (Wolverine)")]
    public Character m_Character2; // enemy

    private Character m_CurrentTurn;
    private GameObject m_AttackButton;

    public enum GameState { BattleStart, PlayerDecide, PlayerAttack, PlayerFlee, EnemyDecide, EnemyAttack, ActionCommand, Waiting, BattleEnd };

    [Tooltip("(NEVER CHANGE THIS IN INSPECTOR) The Current state of combat")]
    public GameState m_State;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }

        if(BetweenSceneReferenceContainer.instance != null)
        {
            // call their respective setup functions
            if (BetweenSceneReferenceContainer.instance.playerStats != null)
            {
                Character playerStats = BetweenSceneReferenceContainer.instance.playerStats.GetComponent<Character>();
                m_Character1.Setup(playerStats);
            }
            else
            {
                m_Character1.Setup(m_Character1);
            }
            if(BetweenSceneReferenceContainer.instance.enemyStats != null)
            {
                Character enemyStats = BetweenSceneReferenceContainer.instance.enemyStats.GetComponent<Character>();
                m_Character2.Setup(enemyStats);
            }
            else
            {
                m_Character2.Setup(m_Character2);
            }
        }
        else
        {
            m_Character1.Setup(m_Character1);
            m_Character2.Setup(m_Character2);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        
        m_AttackButton = GameObject.Find("AttackBackground");
        m_AttackButton.SetActive(false);

        descriptionBox.SetActive(false);

        foreach (Transform child in m_AttackListUI.GetComponentInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }
        //gather the attack list from each character. might change later
        for (int i = 0; i < m_Character1.m_AttackList.Count; i++)
        {
            GameObject newButton = Instantiate(m_AttackButtonPrefab, m_AttackListUI.transform, false);
            newButton.GetComponentInChildren<TMP_Text>().text = m_Character1.m_AttackList[i].m_AttackName;
            //button will subscribe the the UpdateCurrentAttack function and pass itself into it on startup
            //------ Does not show in the inspector ------//
            //newButton.GetComponent<Button>().onClick.AddListener(() => ShowDescription(newButton));
            newButton.GetComponent<Button>().onClick.AddListener(() => UpdateCurrentAttack(newButton));
        }

        if (m_QuickTestActionCommand)
        {
            m_AttackListUI.SetActive(false);
            m_AttackButton.SetActive(false);
            descriptionBox.SetActive(false);
            GameObject clone = Instantiate(m_ActionCommandPrefabToTest);
            clone.GetComponent<ActionCommand>().DoActionCommand();
        }


        m_State = GameState.BattleStart;

        m_CurrentTurn = m_Character1;
        m_State = GameState.PlayerDecide;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_State)
        {
            case GameState.BattleStart:
                break;
            case GameState.PlayerDecide:
                PlayerDecide();
                break;
            case GameState.PlayerAttack:
                PlayerAttack();
                break;
            case GameState.PlayerFlee:
                PlayerFlee();
                break;
            case GameState.EnemyDecide:
                EnemyDecide();
                break;
            case GameState.EnemyAttack:
                EnemyAttack();
                break;
            case GameState.ActionCommand:
                DoingActionCommand();
                break;
            case GameState.Waiting:
                Waiting();
                break;
            case GameState.BattleEnd:
                BattleEnd();
                break;
        }
    }

    #region States

    private void PlayerDecide()
    {
        
    }

    /// <summary>
    /// Called after the player selects and attack and clicks the attack button
    /// </summary>
    public void PlayerAttackUIButton()
    {
        // select which attack to do
        // wait however long (buffer time to prepare)

        m_AttackListUI.SetActive(false);
        m_AttackButton.SetActive(false);
        descriptionBox.SetActive(false);

        StartActionCommand();

        m_State = GameState.ActionCommand;

        // different functions for each type of attack (button mash, click-and-hold, etc)
        // deal damage (character.DealAttack() returns damage amount if any)
        // if damage == 0, then missed
    }

    public void PlayerAttack()
    {
        m_Character2.TakeDamage(m_Character1.DealAttack(m_Character1, m_Character2, actionCommandWasSuccessful));
        if (m_Character2.m_CurrentHp > 0)
        {
            m_State = GameState.Waiting;
        }
        else
        {
            m_State = GameState.BattleEnd;
        }
        m_CurrentTurn = m_Character2;
    }

    public void PlayerFlee()
    {
        // do some sort of load scene going back to the overworld
    }

    public void EnemyDecide()
    {
        // randomly choose an attack
        if (m_Character2.m_AttackList.Count != 0)
        {
            m_Character2.m_CurrAttack = m_Character2.m_AttackList[Random.Range(0, m_Character2.m_AttackList.Count)];
        }
        StartActionCommand();
        m_State = GameState.ActionCommand;
    }

    private void EnemyAttack()
    {
        m_Character1.TakeDamage(m_Character2.DealAttack(m_Character2, m_Character1, actionCommandWasSuccessful));
        if(m_Character1.m_CurrentHp > 0)
        {
            m_State = GameState.Waiting;
        }
        else
        {
            m_State = GameState.BattleEnd;
        }
        m_CurrentTurn = m_Character1;
        m_AttackListUI.SetActive(true);
        //m_AttackButton.SetActive(true);
    }

    public bool ActionCommandWasSuccessful
    {
        set { actionCommandWasSuccessful = value; }
    }
    public bool FinishedActionCommand
    {
        set { finishedActionCommand = value; }
    }
    private bool finishedActionCommand = false;
    private bool actionCommandWasSuccessful = false;
    private void DoingActionCommand()
    {
        if (finishedActionCommand)
        {
            if(m_CurrentTurn == m_Character1)
            {
                m_State = GameState.PlayerAttack;
            }
            else
            {
                m_State = GameState.EnemyAttack;
            }
            finishedActionCommand = false;
        }
    }

    private float m_TimeCounter = 0f;
    private void Waiting()
    {
        // set buttons to inactive until next state 
        //m_AttackListUI.SetActive(false);
        //m_AttackButton.SetActive(false);
        if (m_TimeCounter >= m_TimeBetweenAttacks)
        {
            if(m_CurrentTurn == m_Character1)
            {
                m_State = GameState.PlayerDecide;
            }
            else
            {
                m_State = GameState.EnemyDecide;
            }
            m_TimeCounter = 0f;
            return;
        }
        m_TimeCounter += Time.deltaTime;
    }

    private float m_EndBattleDisplayTimer = 3f;
    private float m_FadeTimer = 0f;
    private void BattleEnd()
    {
        //change text and color of the display
        if (m_FadeTimer == 0)
        {
            EndBattleDisplayText.gameObject.SetActive(true);
            EndBattleDisplayText.alpha = 0;
            if (m_Character1.m_CurrentHp == 0)
            {
                //change text to defeat and color to red
                EndBattleDisplayText.text = "<color=red>Defeat</color>";
            }
            else
            {
                // change text to victory and color to green
                EndBattleDisplayText.text = "<color=green>Victory</color>";
            }
        }

        //first third of a second fade in
        if(m_FadeTimer < (m_EndBattleDisplayTimer / 3))
        {
            // change alpha from 0 to 255
            EndBattleDisplayText.alpha += Time.deltaTime;
        }
        //second third nothing
        else if(m_FadeTimer < (m_EndBattleDisplayTimer * 2 / 3))
        {

        }
        //last third fade out
        else if(m_FadeTimer < m_EndBattleDisplayTimer)
        {
            // change alpha from 255 to 0
            EndBattleDisplayText.alpha -= Time.deltaTime;
        }
        //change scene
        else
        {
            EndBattleDisplayText.gameObject.SetActive(false);
            m_FadeTimer = 0f;
            InitiateLoadScene();
        }
        m_FadeTimer += Time.deltaTime;

    }

    #endregion States

    /// <summary>
    /// function is called when a player selects an attack.
    /// uses the index of the button to update the players current attack 
    /// </summary>
    /// <param name="button"></param> the attack the player selected
    public void UpdateCurrentAttack(GameObject button)
    {
        m_AttackButton.SetActive(true);
        descriptionBox.SetActive(true);
        m_Character1.UpdateCurrentAttack(button.transform.GetSiblingIndex());
    }

    public void SetDescription(string description)
    {
        descriptionText.text = description;
    }

    public void StartActionCommand()
    {
        m_CurrentTurn.InitiateActionCommand();
    }

    public string m_SceneToLoad;

    public void InitiateLoadScene()
    {        
        if(BetweenSceneReferenceContainer.instance.enemyStats == null)
        {
            //add the enemy stats
            GameObject tempEnemyObject = new GameObject("TempEnemyStats");
            Character enemyStats = tempEnemyObject.AddComponent<Character>();
            enemyStats.Setup(m_Character2);
            tempEnemyObject.transform.parent = BetweenSceneReferenceContainer.instance.transform;
            BetweenSceneReferenceContainer.instance.enemyStats = tempEnemyObject;
        }
        else
        {
            BetweenSceneReferenceContainer.instance.enemyStats.GetComponent<Character>().Setup(m_Character2);
        }

        BetweenSceneReferenceContainer.instance.playerStats.GetComponent<Character>().Setup(m_Character1);

        FindObjectOfType<SceneLoader>().LoadScene(m_SceneToLoad);
    }

}
