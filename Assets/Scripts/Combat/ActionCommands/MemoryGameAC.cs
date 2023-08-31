using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Flash four letters (randomized from WASD) for a short time, then enter them in correct sequence
/// </summary>
public class MemoryGameAC : ActionCommand
{

    //
    // PUBLIC VARIABLES
    //
    [Header("Buttons")]
    [Tooltip("Binding for first button")]
    public InputAction button1;
    [Tooltip("Binding for second button")]
    public InputAction button2;
    [Tooltip("Binding for third button")]
    public InputAction button3;
    [Tooltip("Binding for fourth button")]
    public InputAction button4;
    List<InputAction> memoryGameList;

    [Header("Text objects to be used")]
    [Tooltip("Text object of first button")]
    public Text m_FirstButtonLetter;
    [Tooltip("Text object of second button")]
    public Text m_SecondButtonLetter;
    [Tooltip("Text object of third button")]
    public Text m_ThirdButtonLetter;
    [Tooltip("Text object of fourth button")]
    public Text m_FourthButtonLetter;

    [Header("Button objects to be used")]
    [Tooltip("Button object of first button")]
    public GameObject firstButton;
    [Tooltip("Button object of second button")]
    public GameObject secondButton;
    [Tooltip("Button object of third button")]
    public GameObject thirdButton;
    [Tooltip("Button object of fourth button")]
    public GameObject fourthButton;

    [Header("Functionality variables")]
    [Tooltip("Time the player has to enter correct input the button in seconds")]
    public float m_TimeLimit = 5f;
    [Tooltip("The slider used to demonstrate the time")]
    public Slider m_TimeSlider;

    //
    // PRIVATE VARIABLES
    //
    //bool isDone = false;
    bool isReady = false;
    private float m_TimeCounter;
    private float m_FlashTime = 1.0f;

    private string firstButtonToPress;
    private string secondButtonToPress;
    private string thirdButtonToPress;
    private string fourthButtonToPress;

    bool firstCorrect = false;
    bool secondCorrect = false;
    bool thirdCorrect = false;
    bool fourthCorrect = false;
    bool success = false;
    bool enterDone = false;

    public enum AttackState { flashStage, enterStage, doneStage };

    AttackState state;

    float bufferTime = 1f;

    private void Start()
    {
        // set bindings of each key
        button1.AddBinding("<Keyboard>/W");
        button2.AddBinding("<Keyboard>/A");
        button3.AddBinding("<Keyboard>/S");
        button4.AddBinding("<Keyboard>/D");
        // put actions into a list
        memoryGameList = new List<InputAction>();
        memoryGameList.Add(button1);
        memoryGameList.Add(button2);
        memoryGameList.Add(button3);
        memoryGameList.Add(button4);

        ShuffleList(memoryGameList);

        // after list was shuffled, set which binding (button) is supposed to be pressed when
        firstButtonToPress = memoryGameList[0].bindings[0].path;
        secondButtonToPress = memoryGameList[1].bindings[0].path;
        thirdButtonToPress = memoryGameList[2].bindings[0].path;
        fourthButtonToPress = memoryGameList[3].bindings[0].path;

        // display the correct letter for each button
        m_FirstButtonLetter.text = firstButtonToPress[11].ToString();
        m_SecondButtonLetter.text = secondButtonToPress[11].ToString();
        m_ThirdButtonLetter.text = thirdButtonToPress[11].ToString();
        m_FourthButtonLetter.text = fourthButtonToPress[11].ToString();

        m_TimeSlider.value = 0;
        m_TimeSlider.maxValue = m_TimeLimit;

        state = AttackState.flashStage;
    }

    private void Update()
    {
        if (isReady)
        {
            if (state == AttackState.flashStage)
            {
                FlashPhase();
            }

            if (state == AttackState.enterStage)
            {
                EnterPhase();
            }

            if (state == AttackState.doneStage)
            {
                DonePhase();
            }

        }

        if (m_IsDone)
        {

            // add buffer time
            /*if(bufferTime <= 0)
            {*/
            if (CombatManager.instance != null)
            {
                CombatManager.instance.FinishedActionCommand = true;
            }
            Destroy(gameObject);
            /*}
            bufferTime -= Time.deltaTime;*/
        }
        BaseUpdate();
    }

    public void ShuffleList(List<InputAction> lst)
    {
        // randomize the order of the letters
        for (int i = 0; i < lst.Count; i++)
        {
            InputAction temp = lst[i];
            int randInd = Random.Range(i, lst.Count);
            lst[i] = lst[randInd];
            lst[randInd] = temp;
        }
        isReady = true;
    }

    public override void DoActionCommand()
    {
        //Debug.Log("Action Command: Memory Game");
    }

    #region Phases
    /// <summary>
    /// Flash the buttons, then set them to blank after the flash time ends
    /// </summary>
    private void FlashPhase()
    {
        m_FlashTime -= Time.deltaTime;
        if (m_FlashTime <= 0.0f)
        {
            m_FirstButtonLetter.text = "";
            m_SecondButtonLetter.text = "";
            m_ThirdButtonLetter.text = "";
            m_FourthButtonLetter.text = "";
            state = AttackState.enterStage;
        }
    }


    /// <summary>
    /// Make sure player is within time limit
    /// Check the inputs of the buttons through CheckButtonInputs()
    /// Set success based on whether the player finished the sequence
    /// Update the time slider
    /// </summary>
    private void EnterPhase()
    {

        // check that time limit hasn't been reached yet 
        if (m_TimeCounter < m_TimeLimit)
        {
            CheckButtonInputs();

            if (fourthCorrect)
            {
                success = true;
                state = AttackState.doneStage;
            }

            m_TimeCounter += Time.deltaTime;
            m_TimeSlider.value = m_TimeCounter;
        }

        if (m_TimeCounter >= m_TimeLimit)
        {
            //Debug.Log("Fail -- Time Ran Out");
            state = AttackState.doneStage;
        }
    }

    /// <summary>
    /// Perform actions based on whether or not the player succeeded in entering the correct sequence
    /// </summary>
    private void DonePhase()
    {
        if (success)
        {
            CreateSuccessEffect();
        }
        else
        {
            CreateFailEffect();
        }
        //isDone = true;
        isReady = false;
    }
    #endregion

    /// <summary>
    /// Check for the inputs for each button (i.e. that each button has been triggered)
    /// For the first button, you only have to check that no other button has been pressed
    ///     This includes the first button being already pressed
    /// For all other buttons, check that the previous button has been pressed and the current hasn't
    /// </summary>
    private void CheckButtonInputs()
    {
        //
        // FIRST BUTTON CHECK 
        //
        if (memoryGameList[0].triggered)
        {
            if (!firstCorrect)
            {
                //Debug.Log("first button pressed correctly");
                firstCorrect = true;
                m_FirstButtonLetter.text = memoryGameList[0].bindings[0].path[11].ToString();
                firstButton.GetComponent<Image>().color = Color.green;
            }
            else
            {
                //Debug.Log("first button pressed more than once");
                firstCorrect = false;
                success = false;
                state = AttackState.doneStage;
                firstButton.GetComponent<Image>().color = Color.red;
            }

        }

        //
        // SECOND BUTTON CHECK
        //
        if (memoryGameList[1].triggered)
        {
            if (!firstCorrect)
            {
                //Debug.Log("second button pressed before first");
                secondCorrect = false;
                success = false;
                state = AttackState.doneStage;
                secondButton.GetComponent<Image>().color = Color.red;
            }
            else if (!secondCorrect)
            {
                //Debug.Log("second button pressed correctly");
                secondCorrect = true;
                m_SecondButtonLetter.text = memoryGameList[1].bindings[0].path[11].ToString();
                secondButton.GetComponent<Image>().color = Color.green;
            }
            else
            {
                //Debug.Log("second button pressed more than once");
                secondCorrect = false;
                success = false;
                state = AttackState.doneStage;
                secondButton.GetComponent<Image>().color = Color.red;
            }


        }

        //
        // THIRD BUTTON CHECK
        //
        if (memoryGameList[2].triggered)
        {
            if (!secondCorrect)
            {
                //Debug.Log("third button pressed before second");
                thirdCorrect = false;
                success = false;
                state = AttackState.doneStage;
                thirdButton.GetComponent<Image>().color = Color.red;
            }
            else if (!thirdCorrect)
            {
                //Debug.Log("third button pressed correctly");
                thirdCorrect = true;
                m_ThirdButtonLetter.text = memoryGameList[2].bindings[0].path[11].ToString();
                thirdButton.GetComponent<Image>().color = Color.green;
            }
            else
            {
                //Debug.Log("third button pressed more than once");
                thirdCorrect = false;
                success = false;
                state = AttackState.doneStage;
                thirdButton.GetComponent<Image>().color = Color.red;
            }

        }

        //
        // FOURTH BUTTON CHECK
        // 
        if (memoryGameList[3].triggered)
        {
            if (!thirdCorrect)
            {
                //Debug.Log("third button pressed before fourth");
                fourthCorrect = false;
                success = false;
                state = AttackState.doneStage;
                fourthButton.GetComponent<Image>().color = Color.red;
            }
            else if (!fourthCorrect)
            {
                //Debug.Log("fourth button pressed correctly");
                fourthCorrect = true;
                m_FourthButtonLetter.text = memoryGameList[3].bindings[0].path[11].ToString();
                fourthButton.GetComponent<Image>().color = Color.green;
            }
            else
            {
                //Debug.Log("third button pressed more than once");
                fourthCorrect = false;
                success = false;
                state = AttackState.doneStage;
                fourthButton.GetComponent<Image>().color = Color.red;
            }


        }
    }



    #region Enable/Disable
    private void OnEnable()
    {
        button1.Enable();
        button2.Enable();
        button3.Enable();
        button4.Enable();
    }

    private void OnDisable()
    {
        button1.Disable();
        button2.Disable();
        button3.Disable();
        button4.Disable();
    }
    #endregion
}
