using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    #region Fields

    public float maxHp;
    public float damage;

    private float curHp;

    #endregion

    #region Unity methods

    private void Start()
    {
        this.curHp = this.maxHp;
    }

    #endregion


    #region Public Methods

    public void TakeDamage(float dmg)
    {
        this.curHp -= dmg;
        if (CheckIfDead())
        {
            Die();
        }
    }

    public void Attack(CharacterStats target)
    {
        target.TakeDamage(this.damage);
    }

    #endregion

    #region Private methods

    private bool CheckIfDead()
    {
        if (this.curHp <= 0)
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
