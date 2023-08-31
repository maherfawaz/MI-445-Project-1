using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HoldReleaseAC : ActionCommand
{
    [Header("Debugging")]
    public bool m_PauseSlider = false;

    [Header("Mechanics")]
    [Tooltip("inputs the player are able to use")]
    public InputAction m_InputAction;
    [Tooltip("if Checked: holding mechanic is removed, player just presses the desired button in the hit window without holding")]
    public bool m_PressToStop = false;
    [Tooltip("How many seconds it takes the slider to go there and back once")]
    public float m_SecondsPerCycle = 1f;

    [Header("UI elements")]
    public GameObject m_CanvasPrefab;
    public Slider m_slider;
    public Image m_ReleaseArea;

    private bool m_IsReady = false;
    private float m_MaxReleaseValue;
    private float m_MinReleaseValue;
    private float m_SliderWid;

    public override void DoActionCommand()
    {
        Setup();
    }

    private void Setup()
    {
        m_SliderWid = m_slider.GetComponent<RectTransform>().rect.width;
        m_slider.maxValue = m_SliderWid;

        //randomize where the release area will be
        //get a min and max value for successful release
        float releaseAreWid = m_ReleaseArea.rectTransform.rect.width;

        m_ReleaseArea.rectTransform.localPosition = new Vector3(Random.Range(0f, 
                                                                                (m_SliderWid) - (releaseAreWid)), m_ReleaseArea.rectTransform.localPosition.y, m_ReleaseArea.rectTransform.localPosition.z);
        m_MaxReleaseValue = m_ReleaseArea.rectTransform.localPosition.x + releaseAreWid;
        m_MinReleaseValue = m_ReleaseArea.rectTransform.localPosition.x;

        m_InputAction.performed += context => M_InputAction_Performed(context);
        m_InputAction.canceled += context => M_InputAction_Canceled(context);
    }

    private bool m_IsIncreasing = true;
    private void Update()
    {
        if(m_PressToStop)
        {
            m_IsReady = true;
        }
        else
        {
            if(Keyboard.current.anyKey.wasPressedThisFrame)
            {
                m_IsReady = true;
            }
        }
        if(m_IsReady)
        {
            //math for the slider moving up or down
            if(!m_PauseSlider)
            {
                if (m_IsIncreasing)
                {
                    m_slider.value += (Time.deltaTime * m_SliderWid) / (m_SecondsPerCycle / 2);
                    if (m_slider.value >= m_slider.maxValue)
                    {
                        m_IsIncreasing = false;
                        m_slider.value = m_slider.maxValue;
                    }
                }
                else
                {
                    m_slider.value -= (Time.deltaTime * m_SliderWid) / (m_SecondsPerCycle / 2);
                    if (m_slider.value <= m_slider.minValue)
                    {
                        m_IsIncreasing = true;
                        m_slider.value = m_slider.minValue;
                    }
                }
            }
        }
        if (m_IsDone)
        {
            if (CombatManager.instance != null)
            {
                CombatManager.instance.FinishedActionCommand = true;
            }
            Destroy(gameObject);
        }
        BaseUpdate();
    }

    private void Result()
    {
        if (m_slider.value >= m_MinReleaseValue && m_slider.value <= m_MaxReleaseValue)
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

    private void M_InputAction_Performed(InputAction.CallbackContext context)
    {
        if(!m_PressToStop)
        {
            m_IsReady = true;
        }
        else
        {
            m_IsReady = false;
        }
    }

    private void M_InputAction_Canceled(InputAction.CallbackContext context)
    {
        if(!m_PressToStop)
        {
            m_IsReady = false;
            Result();
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









