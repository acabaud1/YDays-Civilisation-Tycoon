using UnityEngine;
using UnityEngine.AI;

public class PNJ
{
    protected GameObject GObject;
    private NavMeshAgent _gAgent;

    /// <summary>
    /// Crée le GameObject correspondant au modèle
    /// </summary>
    private void Create()
    {
        _gAgent = GObject.AddComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Spawn un PNJ à la position donnée
    /// </summary>
    public void Spawn(int x, int z)
    {
        // Création du NavMeshAgent
        Create();

        // Définition de la position
        GObject.transform.position = new Vector3(x, 1, z);
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
    /// TODO: Gestion de la distance du PNJ par rapport à la destination
    /// </summary>
    public void CheckDestination()
    {
        if (!_gAgent.pathPending)
        {
            if (_gAgent.remainingDistance <= _gAgent.stoppingDistance)
            {
                if (!_gAgent.hasPath || _gAgent.velocity.sqrMagnitude == 0f)
                {
                    //Move(Random.Range(1, 64), Random.Range(1, 64));
                }
            }
        }
    }

}
