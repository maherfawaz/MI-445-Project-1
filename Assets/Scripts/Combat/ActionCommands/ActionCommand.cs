using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class for all action commands
/// Handles audio (based on success/fail) for actions
/// </summary>
public abstract class ActionCommand : MonoBehaviour
{
    [Header("Polish Effect Settings")]
    [Tooltip("The prefab of the effect to create when this action command is failed")]
    public GameObject m_ActionCommandFailEffect;
    [Tooltip("The prefab of the effect to create when this action command is done successfully")]
    public GameObject m_ActionCommandSuccessEffect;
    [Tooltip("The time in seconds to wait after the action command ends before moving on")]
    public float m_ActionCommandEndDelay = 0.5f;

    private bool m_PlaySuccessSound = false;
    private bool m_PlayFailSound = false;


    protected bool m_IsDone = false;

    public abstract void DoActionCommand();

    protected void CreateFailEffect()
    {
        if (m_ActionCommandFailEffect != null)
        {
            Instantiate(m_ActionCommandFailEffect);
        }
        m_PlayFailSound = true;
    }

    protected void CreateSuccessEffect()
    {
        if (m_ActionCommandSuccessEffect != null && !m_PlaySuccessSound)
        {
            Instantiate(m_ActionCommandSuccessEffect);
        }
        m_PlaySuccessSound = true;
    }
    protected void CreateFailEffect(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (m_ActionCommandFailEffect != null && !m_PlayFailSound)
        {
            Instantiate(m_ActionCommandFailEffect, position, rotation, parent);
        }
        m_PlayFailSound = true;
    }

    protected void CreateSuccessEffect(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (m_ActionCommandSuccessEffect != null)
        {
            Instantiate(m_ActionCommandSuccessEffect, position, rotation, parent);
        }
        m_PlaySuccessSound = true;
    }

    private float m_Counter = 0f;

    protected void BaseUpdate()
    {
        if(m_PlaySuccessSound)
        {
            if(m_Counter >= m_ActionCommandEndDelay)
            {
                m_IsDone = true;
            }
            m_Counter += Time.deltaTime;
        }
        else if(m_PlayFailSound)
        {
            if (m_Counter >= m_ActionCommandEndDelay)
            {
                m_IsDone = true;
            }
            m_Counter += Time.deltaTime;
        }
    }
}
