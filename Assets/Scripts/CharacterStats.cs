using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    #region Fields

    public float MaxHp;
    public float Damage;

    private float _curHp;


    #endregion

    #region Unity methods

    private void Start()
    {
        _curHp = MaxHp;
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Le PNJ s'inflige les dégâts passés en paramètres
    /// </summary>
    /// <param name="dmg">Quantité de dégâts subits</param>
    /// <returns></returns>
    public bool TakeDamage(float dmg)
    {
        _curHp -= dmg;
        if (CheckIfDead())
        {
            Die();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Le PNJ attaque la cible passée en paramètre
    /// </summary>
    /// <param name="target">Cible attaquée</param>
    /// <returns></returns>
    public bool Attack(CharacterStats target)
    {
        return target.TakeDamage(this.Damage);
    }

    #endregion

    #region Private methods

    private bool CheckIfDead()
    {
        if (_curHp <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

    #endregion

}
