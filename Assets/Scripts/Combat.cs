using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
                        var anim = this.gameObject.GetComponent<Animation>();
                        anim.Play("Coup");
                    }
                    else
                    {
                        attackCooldown -= Time.deltaTime;
                    }
                }
                else
                {
                    
                    //move to target
                    var position = targetTransform.position;

                    /*var strength = 1;

                    var targetRotation = Quaternion.LookRotation(targetTransform.position - this.transform.position);
                    var str = Mathf.Min(strength * Time.deltaTime, 1);
                    this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, str);*/
                    this.gameObject.GetComponent<NavMeshAgent>().SetDestination(new Vector3(position.x-1, position.y, position.z));
                }
            }
        }
        else
        {
            if (isAttacked)
            {
                //run away from target
                var position = targetTransform.position;

                this.gameObject.GetComponent<NavMeshAgent>().SetDestination(new Vector3(position.x - 5, position.y, position.z - 5));
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        target = other.gameObject;
        if (target.GetComponent<Combat>() != null)
        {
            targetCombat = target.GetComponent<Combat>();
            switch (targetCombat.faction)
            {
                case Faction.black:
                    isAttacking = false;
                    targetCombat.isAttacked = false;
                    break;
                case Faction.red:
                    targetTransform.position = target.transform.position;
                    Debug.Log(target.transform.position);
                    isAttacking = true;
                    targetCombat.isAttacked = true;
                    break;
                case Faction.blue:
                    isAttacking = false;
                    targetCombat.isAttacked = false;
                    break;
                case Faction.green:
                    isAttacking = false;
                    targetCombat.isAttacked = false;
                    break;
                default:
                    break;
            }
            isAttacking = true; 
        }
    }
}
