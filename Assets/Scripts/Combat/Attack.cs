using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack_", menuName = "ScriptableObjects/Attack")]
public class Attack : ScriptableObject
{
    //
    // another variable to assign an action command to this action
    //
    [HideInInspector]
    public enum AttackTypes
    {
        RegularAttack, PiercingAttack, ProtectiveAttack
    }
    [Header("Behavior")]
    [Tooltip("Type of the Attack")]
    public AttackTypes m_AttackType = AttackTypes.RegularAttack;

    [Tooltip("Action Command associated with this Attack")]
    public GameObject m_ActionCommandCanvasPrefab;

    [Header("Stats")]
    [Tooltip("name of the attack on screen")]
    public string m_AttackName = "";
    [Tooltip("description of the attack on screen")]
    public string m_AttackDescription = "";
    public int m_BaseDamage = 0;
    [Tooltip("Attack damage the player has")]
    public int m_AttackPower = 0;
    [Tooltip("Minimum damage this attack makes on a failed command")]
    public int m_MinDamage = 0;
    [Tooltip("Maximum damage this attack can do")]
    public int m_MaxDamage = 0;

    /// <summary>
    /// calculates the damage that a player character will do with each attack given the attacker and defenders stats
    /// </summary>
    /// <param name="attacker"></param> The attackers stats
    /// <param name="defender"></param> The defenders stats
    /// <returns></returns>
    public int AttackDamageCalculation(Character attacker, Character defender, bool wasSuccessfull)
    {
        //switch defence back after 1 complete turn
        attacker.m_Defense = attacker.m_PrevDefence;

        int damage;
        switch (m_AttackType)
        {
            case AttackTypes.RegularAttack:
                // check if its a successful player command or unsuccessful enemy command
                if ((wasSuccessfull && CombatManager.instance.m_State == CombatManager.GameState.PlayerAttack) || 
                    (!wasSuccessfull && CombatManager.instance.m_State == CombatManager.GameState.EnemyAttack))
                {
                    damage = m_BaseDamage + ((defender.m_Defense * defender.m_EquipmentLevel) - (m_AttackPower * attacker.m_EquipmentLevel));
                    return Mathf.Abs(damage);
                }
                return m_MinDamage;
                
            case AttackTypes.PiercingAttack:
                if ((wasSuccessfull && CombatManager.instance.m_State == CombatManager.GameState.PlayerAttack) ||
                    (!wasSuccessfull && CombatManager.instance.m_State == CombatManager.GameState.EnemyAttack))
                {
                    damage = m_BaseDamage + (defender.m_Defense * defender.m_EquipmentLevel) -
                    (m_AttackPower * attacker.m_EquipmentLevel) + Mathf.CeilToInt(defender.m_Defense / 2);
                    return Mathf.Abs(damage);
                }
                return m_MinDamage;

            case AttackTypes.ProtectiveAttack:
                damage = m_BaseDamage;
                attacker.m_PrevDefence = attacker.m_Defense;
                if ((wasSuccessfull && CombatManager.instance.m_State == CombatManager.GameState.PlayerAttack) ||
                   (!wasSuccessfull && CombatManager.instance.m_State == CombatManager.GameState.EnemyAttack))
                {
                    float tempDamage = ((attacker.m_Defense + ((float)attacker.m_Defense / 2)) / (float)m_AttackPower) * m_BaseDamage;
                    attacker.m_Defense += Mathf.CeilToInt(tempDamage);
                    return Mathf.Abs(damage);
                }
                return m_MinDamage;
        }
        //Debug.LogError("Error: attack Type was never set");
        return 0;
    }
}
