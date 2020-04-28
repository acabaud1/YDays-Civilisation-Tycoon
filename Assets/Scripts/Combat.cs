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
    public float attackCooldown;

    private SphereCollider sphCollider;
    public float detectionRange;

    public GameObject target;
    public Combat targetCombat;
    public Transform targetTransform;
    public bool isAttacked = false;
    public bool isAttacking = false;


    // Start is called before the first frame update
    void Start()
    {
        sphCollider = this.gameObject.AddComponent<SphereCollider>();
        sphCollider.center = Vector3.zero;
        sphCollider.radius = detectionRange;
        sphCollider.isTrigger = true;
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
                }
            }
        }
        else
        {
            if (isAttacked)
            {
                //run away from target
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
