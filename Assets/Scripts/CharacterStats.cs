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

    #region Properties

    public float CurHp { get => curHp; set => curHp = value; }

    #endregion

    #region Unity methods

    private void Start()
    {
        this.CurHp = this.maxHp;
    }

    #endregion


    #region Public Methods

    public void TakeDamage(float dmg)
    {
        this.CurHp -= dmg;
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
        if (this.CurHp <= 0)
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
