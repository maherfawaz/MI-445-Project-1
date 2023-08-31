using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// 
/// </summary>
public class Character : MonoBehaviour
{
    [Header("Attacks and Animations")]
    [Tooltip("List of attacks this character has")]
    public List<Attack> m_AttackList = new List<Attack>();
    [HideInInspector]
    public Attack m_CurrAttack;

    [Tooltip("animations for each attack (index must match that of the relevant attack)")]
    public List<string> m_AttackAnimations = new List<string>();

    [Header("Stats")]
    [Tooltip("Maximum Health Points the character has")]
    public int m_MaxHp = 20;
    [Tooltip("Health the character currently has")]
    public int m_CurrentHp = 20;

    [Tooltip("Equiptment upgrade constant is a multiplier for attack points")]
    public int m_EquipmentLevel = 1;
    [Tooltip("how effectivly the character can reduce attack damage")]
    public int m_Defense = 10;
    [HideInInspector]
    public int m_PrevDefence;

    [Header("UI")]
    public Slider m_HealthBarSlider;
    public TMP_Text m_HealthText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Setup(Character stats)
    {
        // setup for the character stats
        m_AttackList = stats.m_AttackList;
        m_CurrAttack = stats.m_CurrAttack;
        m_AttackAnimations = stats.m_AttackAnimations;
        m_MaxHp = stats.m_MaxHp;
        m_CurrentHp = stats.m_CurrentHp;
        m_EquipmentLevel = stats.m_EquipmentLevel;
        m_Defense = stats.m_Defense;
        m_PrevDefence = stats.m_PrevDefence;

        // setup for the combat manager UI stuff
        m_PrevDefence = m_Defense;
        if (m_HealthBarSlider != null)
        {
            m_HealthBarSlider.maxValue = m_MaxHp;
            m_HealthBarSlider.value = m_CurrentHp;
        }
        if (m_HealthText != null)
        {
            m_HealthText.text = m_CurrentHp.ToString();
        }

    }

    /// <summary>
    /// Calls the damage calculation and action command functions for the attack
    /// </summary>
    /// <returns></returns> The damage produced
    public int DealAttack(Character attacker, Character defender, bool ACSuccessful)
    {
        // add a boolean if the command was successful
        return m_CurrAttack.AttackDamageCalculation(attacker, defender, ACSuccessful);
    }

    public void InitiateActionCommand()
    {
        GameObject ACCanvas = Instantiate(m_CurrAttack.m_ActionCommandCanvasPrefab);
        ACCanvas.GetComponent<ActionCommand>().DoActionCommand();
    }

    public void UpdateCurrentAttack(int index)
    {
        m_CurrAttack = m_AttackList[index];
        CombatManager.instance.SetDescription(m_AttackList[index].m_AttackDescription);
    }

    public void TakeDamage(int damage)
    {
        m_CurrentHp -= damage;
        if(m_CurrentHp <= 0)
        {
            m_CurrentHp = 0;
            CombatManager.instance.m_State = CombatManager.GameState.BattleEnd;
        }
        m_HealthBarSlider.value = m_CurrentHp;
        m_HealthText.text = m_CurrentHp.ToString();
    }
}
