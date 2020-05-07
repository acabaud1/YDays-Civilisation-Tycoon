using UnityEngine;
using UnityEngine.AI;

public class PNJ
{
    protected GameObject GObject;
    private NavMeshAgent _gAgent;
    public GameObject attachedBuilding;

    private float agentSpeed = 1f;

    /// <summary>
    /// Crée le GameObject correspondant au modèle
    /// </summary>
    private void Create()
    {
        _gAgent = GObject.AddComponent<NavMeshAgent>();
        _gAgent.speed = agentSpeed;
    }

    /// <summary>
    /// Spawn un PNJ à la position donnée
    /// </summary>
    public void Spawn(Vector3 position)
    {
        // Création du NavMeshAgent
        Create();

        // Définition de la position
        GObject.transform.position = position;
    }

    /// <summary>
    /// Règle la destination du PNJ sur la position donnée
    /// </summary>
    public void Move(Vector3 destination)
    {
        _gAgent.destination = destination;
    }

    /// <summary>
    /// Arrête le déplacement du PNJ
    /// </summary>
    public void Stop()
    {
        _gAgent.isStopped = true;
    }

    /// <summary>
    /// Détruit le PNJ
    /// </summary>
    public void Destroy()
    {
        UnityEngine.Object.Destroy(GObject);
    }

    /// <summary>
    /// Gestion de la distance du PNJ par rapport à la destination
    /// </summary>
    public bool CheckDestination()
    {
        if (!_gAgent.pathPending)
        {
            if (_gAgent.remainingDistance <= _gAgent.stoppingDistance)
            {
                if (!_gAgent.hasPath || _gAgent.velocity.sqrMagnitude == 0f)
                {
                    // Arrivé à destination
                    if(attachedBuilding != null) attachedBuilding.SetActive(true);
                    return true;
                }
            }
        }

        return false;
    }

}
