using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

/// <summary>
/// A button is shown and the player has a limited amount of time to repeatedly press the button
/// </summary>
public class ButtonMashAC : ActionCommand
{
    /**
     * PUBLIC MEMBERS
     */
    [Header("functionality variables")]
    [Tooltip("Time the player has to mash the button in seconds")]
    public float m_TimeLimit = 5f;
    [Tooltip("The required number of presses the player must attempt to succeed")]
    public int m_PressesToSucceed = 20;
    [Tooltip("The buttons that the player must press to pass the Action Command")]
    public InputAction m_InputAction;

    [Header("UI Elements")]
    [Tooltip("Unity Panel inside of the Action Command Canvas Object")]
    public GameObject m_ButtonMashPanel;
    [Tooltip("The slider UI elements used for the timer visual")]
    public Slider m_TimeSlider;
    [Tooltip("The back most image that represents the target fill for the button mashing")]
    public Image m_FillAmount;
    [Tooltip("the current amount of fill achieved while button mashing")]
    public Image m_CurrentFill;
    [Tooltip("Text containing the button that must be pressed")]
    public Text m_ButtonLetter;

    /**
     * PRIVATE MEMBERS
     */
    private InputControl m_InputControl;
    private float m_ScaleFractionIncrease;
    private float m_TimeCounter;
    private bool m_IsReady = false;

    /// <summary>
    /// override method for base class version
    /// </summary>
    public override void DoActionCommand()
    {
        Setup();
    }

    /// <summary>
    /// information needed to start the action command is set here
    /// things like which InputAction control needs to be pressed and UI element initialization
    /// </summary>
    public void Setup()
    {
        // get a random binding the use for the button from the selected bindings
        if (m_InputAction.controls.Count != 0)
        {
            m_InputControl = m_InputAction.controls[Random.Range(0, m_InputAction.controls.Count)];
            if(InputControlPath.MatchesPrefix("<Keyboard>", m_InputControl))
            {
                m_ButtonLetter.text = m_InputControl.path[10].ToString();
            }
        }
        // reset the current fill to nothing
        m_CurrentFill.transform.localScale = new Vector3(0f, 0f, 0f);

        // reset the slider back to zero and set the max value to the time limit
        m_TimeSlider.value = 0;
        m_TimeSlider.maxValue = m_TimeLimit;

        // get how much fill each button press will take
        m_ScaleFractionIncrease = 1 / (float)m_PressesToSucceed;

        m_IsReady = true;

        //subscribe this function to be called when an InputAction is performed
        m_InputAction.performed += context => M_InputAction_performed(context);

    }

    /// <summary>
    /// Main Update function
    /// this is where the actual button mash game is happening
    /// </summary>
    private void Update()
    {
        if(m_IsReady)
        {
            // check to see time hasn't expired
            if(m_TimeCounter > m_TimeLimit)
            {
                //failed
                CreateFailEffect();
                m_InputAction.Disable();
                if(CombatManager.instance != null)
                {
                    CombatManager.instance.ActionCommandWasSuccessful = false;
                }
                m_IsReady = false;
            }
            //check to see if filled all the way
            if (m_CurrentFill.transform.localScale.x >= 1)
            {
                //success
                CreateSuccessEffect();
                m_InputAction.Disable();
                if (CombatManager.instance != null)
                {
                    CombatManager.instance.ActionCommandWasSuccessful = true;
                }
                m_IsReady = false;
            }
            m_TimeCounter += Time.deltaTime;
            m_TimeSlider.value = m_TimeCounter;
        }
        //gets called after IsDone is set to true in the play___Sound functions
        if(m_IsDone)
        {
            if (CombatManager.instance != null)
            {
                CombatManager.instance.FinishedActionCommand = true;
            }
            Destroy(gameObject);
        }
        BaseUpdate();
    }

    /// <summary>
    /// called whenever the m_InputAction gets performed
    /// </summary>
    /// <param name="context"></param> information about what triggered the action
    private void M_InputAction_performed(InputAction.CallbackContext context)
    {
        //if the input action pressed was the correct path than scale of the fill image
        InputControl control = context.control;
        if(InputControlPath.Matches(m_InputControl.path, control))
        {
            m_CurrentFill.rectTransform.localScale += new Vector3(m_ScaleFractionIncrease, m_ScaleFractionIncrease, 0f);
        }
    }

    private void OnEnable()
    {
        m_InputAction.Enable();
    }

    private void OnDisable()
    {
        m_InputAction.Disable();
    }
}
