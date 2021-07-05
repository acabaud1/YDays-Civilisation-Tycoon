using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;

public class FightBehaviour : MonoBehaviour
{
    public enum FactionEnum
    {
        blue, //alliers
        red,  //ennemis
        black, //neutre aggressif
        green //neutre non aggressif
    }
    public FactionEnum Faction; //Faction du combatant
    public float AttackRange;   //Portée d'attaque
    public float AttackSpeed;   //Vitesse d'attaque
    private float _attackCooldown; //Temps restant avant la prochaine attaque

    private SphereCollider _sphCollider;
    public float DetectionRange;    //Portée de détection

    private GameObject _target;   
    private FightBehaviour _targetCombat; 
    private Transform _targetTransform;
    private bool _isAttacked = false;
    private bool _isAttacking = false;

    private CharacterStats _chStats;

    private List<GameObject> _objectsInRange = new List<GameObject>();
    private bool _targetInRange = false;


    // Start is called before the first frame update
    void Start()
    {
        //Création d'un sphere collider au niveau du pnj et passage en mode trigger
        _sphCollider = gameObject.AddComponent<SphereCollider>();
        _sphCollider.center = Vector3.zero;
        _sphCollider.radius = DetectionRange;
        _sphCollider.isTrigger = true;

        _chStats = GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        //Si le pnj est de la faction verte et se fait attaquer, il fuit
        if (Faction == FactionEnum.green && _isAttacked)
        {
            Fleeing();
        }
        else
        {
            //Sinon s'il peut attaquer, il attaque
            if (_isAttacking)
            {
                //S'il est à portée, il attaque
                if (Vector3.Distance(gameObject.transform.position, _targetTransform.position) <= AttackRange)
                {
                    gameObject.GetComponent<NavMeshAgent>().isStopped = true;

                    if (_attackCooldown <= 0)
                    {
                        _attackCooldown = AttackSpeed;

                        var anim = gameObject.GetComponent<Animation>();
                        anim.Play("Coup");

                        if (_chStats.Attack(_target.GetComponent<CharacterStats>()))
                        {
                            _objectsInRange.Remove(_target.gameObject);
                            UpdateTarget();
                        }
                    }
                    else
                    {
                        _attackCooldown -= Time.deltaTime;
                    }
                }
                //Sinon il se rapproche de la cible
                else
                {
                    gameObject.GetComponent<NavMeshAgent>().isStopped = false;

                    Vector3 position = _targetTransform.position;
                    Vector3 targetDirection = position - transform.position;

                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 360 * Time.deltaTime, 0.0f);

                    GetComponent<NavMeshAgent>().destination = position;
                    transform.rotation = Quaternion.LookRotation(newDirection);
                }
            }
        }
    }

    ///<summary>
    ///Retire un objet à objectsInRange s'il n'est à portée et s'il a un script Combat
    ///</summary>
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<FightBehaviour>() != null)
        {
            _objectsInRange.Remove(other.gameObject);
            UpdateTarget();
        }
    }


    ///<summary>
    ///Ajoute un objet à objectsInRange s'il est à portée et s'il a un script Combat
    ///</summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FightBehaviour>() != null)
        {
            _objectsInRange.Add(other.gameObject);
            UpdateTarget();
        }
    }

    #region Private Methods

    ///<summary>
    ///Mets à jour la cible en fonction du contenu de objectsInRange.
    ///Mets aussi à jour le booléen isAttacking en fonction de la faction du pnj et de celle de la cible.
    /// </summary>
    private void UpdateTarget()
    {
        _target = null;
        _targetCombat = null;
        _targetTransform = null;

        if (_objectsInRange.Count >= 1)
        {
            _target = _objectsInRange[0].gameObject;
            _targetCombat = _objectsInRange[0].GetComponent<FightBehaviour>();
            _targetTransform = _objectsInRange[0].transform;

            _targetInRange = true;
        }
        else
        {
            _targetInRange = false;
        }

        if (_targetInRange)
        {
            switch (Faction)
            {
                case FactionEnum.blue:
                    IsBlue();
                    break;
                case FactionEnum.red:
                    IsRed();
                    break;
                case FactionEnum.black:
                    IsBlack();
                    break;
                default:
                    break;
            }
        }
    }

    private void Fleeing()
    {
        gameObject.GetComponent<NavMeshAgent>().isStopped = false;

        //run away from target
        Vector3 position = _targetTransform.position;
        Vector3 targetDirection = position - transform.position;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 360 * Time.deltaTime, 0.0f);

        GetComponent<NavMeshAgent>().destination = -position;
        transform.rotation = Quaternion.LookRotation(-newDirection);
    }

    private void IsBlue()
    {
        switch (_targetCombat.Faction)
        {
            case FactionEnum.red:
            case FactionEnum.black:
                _isAttacking = true;
                break;
            case FactionEnum.green:
            case FactionEnum.blue:
                _isAttacking = false;
                break;
            default:
                break;
        }
    }

    private void IsRed()
    {
        switch (_targetCombat.Faction)
        {
            case FactionEnum.green:
            case FactionEnum.blue:
            case FactionEnum.black:
                _isAttacking = true;
                break;
            case FactionEnum.red:
                _isAttacking = false;
                break;
            default:
                break;
        }
    }

    private void IsBlack()
    {
        switch (_targetCombat.Faction)
        {
            case FactionEnum.green:
            case FactionEnum.blue:
            case FactionEnum.red:
                _isAttacking = true;
                break;
            case FactionEnum.black:
                _isAttacking = false;
                break;
            default:
                break;
        }
    }

    #endregion
}
