using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

public class PNJManager : MonoBehaviour
{
    public GameObject[] animals;
    public GameObject[] robots;
    public GameObject[] humans;

    List<Animal> _animals = new List<Animal>();
    List<Robot> _robots = new List<Robot>();
    List<Human> _humans = new List<Human>();

    /// <summary>
    ///     Initialise une nouvelle instance de la classe <see cref="PNJManager" />
    /// </summary>
    private PNJManager()
    {
    }

    /// <summary>
    /// Instance de la gestion des PNJ.
    /// </summary>
    private static PNJManager instance;

    /// <summary>
    /// Récupère l'instance de building manager.
    /// </summary>
    /// <returns>Building Manager.</returns>
    public static PNJManager GetInstance()
    {
        if (instance == null)
        {
            instance = new PNJManager();
        }

        return instance;
    }

    public Animal CreateAnimal(Vector3 position)
    {
        var animal = new Animal(Instantiate(animals[UnityEngine.Random.Range(0, animals.Length)]));
        animal.Spawn(position);
        _animals.Add(animal);

        return animal;
    }

    public Robot CreateRobot(Vector3 position)
    {
        var robot = new Robot(Instantiate(robots[UnityEngine.Random.Range(0, animals.Length)], position, Quaternion.identity));
        robot.Spawn(position);
        _robots.Add(robot);

        return robot;
    }

    public Human CreateHuman(Vector3 position)
    {
        var human = new Human(Instantiate(humans[UnityEngine.Random.Range(0, humans.Length)]));
        human.Spawn(position);
        _humans.Add(human);

        return human;
    }

    private void Destroy(PNJ obj)
    {
        if (obj.GetType() == typeof(Animal))
          _animals.Remove((Animal) obj);
        else if (obj.GetType() == typeof(Robot))
          _robots.Remove((Robot) obj);

        obj.Destroy();
    }

    /// <summary>
    ///     Fonction de mise à jour appelé chaque frame
    /// </summary>
    public void Update()
    {
        for (var i = 0; i < _animals.Count; i++)
        {
            if(_animals[i].CheckDestination()) Destroy(_animals[i]);
        }

        for (var i = 0; i < _robots.Count; i++)
        {
            if(_robots[i].CheckDestination()) Destroy(_robots[i]);
        }

    }
}
