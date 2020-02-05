using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceType : MonoBehaviour
{
    public RessourceEnum Ressource { get; set; }
}

public enum RessourceEnum{
    None,
    Iron,
    Stone,
    Wood
}
