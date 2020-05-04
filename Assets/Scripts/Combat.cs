using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public enum Faction
    {
        blue, //alliers
        red,  //ennemis
        black, //neutre aggressif
        green //neutre non aggressif
    }
    private Faction faction;
    public float attackRange;
    public float attackSpeed;
    private float attackCooldown;

    private SphereCollider sphCollider;
    public float detectionRange;

    private GameObject target;
    private Combat targetCombat;
    private Transform targetTransform;
    private bool isAttacked = false;
    private bool isAttacking = false;

    private CharacterStats chStats;
    private PNJ pnjScript;


    // Start is called before the first frame update
    void Start()
    {
        sphCollider = this.gameObject.AddComponent<SphereCollider>();
        sphCollider.center = Vector3.zero;
        sphCollider.radius = detectionRange;
        sphCollider.isTrigger = true;

        chStats = this.GetComponent<CharacterStats>();
        attackCooldown = attackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.faction != Faction.green)
        {
            if (isAttacking)
            {
                if (Vector3.Distance(this.transform.position, targetTransform.position) < attackRange)
                {
                    if (attackCooldown <= 0)
                    {
                        //attack
                        chStats.Attack(target.GetComponent<CharacterStats>());
                        attackCooldown = attackSpeed;
                    }
                    else
                    {
                        attackCooldown -= Time.deltaTime;
                    }
                }
                else
                {
                    //move to target
                    pnjScript.Move(targetTransform.position);
                }
            }
        }
        else
        {
            if (isAttacked)
            {
                //run away from target
                pnjScript.Move(-targetTransform.position);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        target = other.gameObject;
        if (target.GetComponent<Combat>() != null)
        {
            targetCombat = target.GetComponent<Combat>();
            switch (targetCombat.faction)
            {
                case Faction.black:
                case Faction.red:
                    targetTransform = target.transform;
                    isAttacking = true;
                    targetCombat.isAttacked = true;
                    break;
                case Faction.blue:
                case Faction.green:
                default:
                    break;
            }
            isAttacking = true; 
        }
    }
}
