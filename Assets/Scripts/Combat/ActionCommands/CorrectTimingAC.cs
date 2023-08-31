using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CorrectTimingAC : ActionCommand
{
    /* PUBLIC MEMBERS */
    [Header("UI elements")]
    public Image m_FirstShape;
    public Image m_SecondShape;
    public Image m_ThirdShape;
    public Image m_FourthShape;

    [Header("Actions")]
    public InputAction m_InputAction;

    [Header("Mechanics")]
    public float m_TimeBetweenFlashes = 1f;
    public float m_TimeForLastFlash = 1f;

    /* PRIVATE MEMBERS */
    private bool m_IsSetup = false;
    private bool m_IsSuccessfull = false;
    private ShapeState m_State = ShapeState.Start;

    private enum ShapeState
    {
        Start, First, Second, Third, Fourth, TimesUp
    }

    public override void DoActionCommand()
    {
        setup();
    }

    private void setup()
    {
        m_InputAction.Enable();
        m_InputAction.performed += context => InputAction_Performed(context);
        m_IsSetup = true;
    }

    private float m_Timer = 0f;

    private void Update()
    {
        if(m_IsSetup)
        {
            if (m_State == ShapeState.Fourth)
            {
                if (m_Timer < m_TimeForLastFlash)
                {
                    m_Timer += Time.deltaTime;
                }
                else
                {
                    NextState();
                    m_Timer = 0;
                }
            }
            else
            {
                if (m_Timer < m_TimeBetweenFlashes)
                {
                    m_Timer += Time.deltaTime;
                }
                else
                {
                    NextState();
                    m_Timer = 0;
                }
            }
        }
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

    private void NextState()
    {
        switch(m_State)
        {
            case ShapeState.Start:
                m_State = ShapeState.First;
                m_FirstShape.gameObject.SetActive(true);
                break;
            case ShapeState.First:
                m_State = ShapeState.Second;
                m_SecondShape.gameObject.SetActive(true);
                break;
            case ShapeState.Second:
                m_State = ShapeState.Third;
                m_ThirdShape.gameObject.SetActive(true);
                break;
            case ShapeState.Third:
                m_State = ShapeState.Fourth;
                m_FourthShape.gameObject.SetActive(true);
                break;
            case ShapeState.Fourth:
                m_State = ShapeState.TimesUp;
                m_InputAction.Disable();
                Result();
                break;
            case ShapeState.TimesUp:
                break;
        }
    }
    private void Result()
    {
        if (m_IsSuccessfull)
        {
            //success
            CreateSuccessEffect();
            if (CombatManager.instance != null)
            {
                CombatManager.instance.ActionCommandWasSuccessful = true;
            }
        }
        else
        {
            //fail
            CreateFailEffect();
            if (CombatManager.instance != null)
            {
                CombatManager.instance.ActionCommandWasSuccessful = false;
            }
        }
    }

    private void InputAction_Performed(InputAction.CallbackContext context)
    {
        if(m_State == ShapeState.Fourth)
        {
            m_IsSuccessfull = true;
        }
        else
        {
            m_IsSuccessfull = false;
        }
        m_IsSetup = false;
        Result();
    }


}
