using System.Collections;
using System.Collections.Generic;
using Ressource;
using UnityEngine;

/// <summary>
/// Class de création de type de ressource selon ce qu'il y a dans <see cref="ResourceEnum"/>
/// </summary>
public class RessourceType : MonoBehaviour
{
    public ResourceEnum Resource { get; set; } // récupération d'une ressource contenu dans ResourceEnum
}