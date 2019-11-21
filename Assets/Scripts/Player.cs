using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int iron;
    public int wood;
    public int stone;
    public int ironStock;
    public int woodStock;
    public int stoneStock;
    public string message;
    public int nbStock;
    public GameObject stockage;
    public int placeStock;

    
    void AddStone (int quantity)
    {
        int result = stone + quantity;

        try
        {
            if (result < stoneStock)
            {
                stone = result;
            } 
            else
            {
                message = "Stock trop plein";
            }
        }
        catch
        {
            throw new System.Exception();
        }
    }

    void AddWood (int quantity)
    {
        int result = wood + quantity;

        try
        {
            if (result < woodStock)
            {
                wood = result;
            } 
            else
            {
                message = "Stock trop plein";
            }
        }
        catch
        {
            throw new System.Exception();
        }
    }

    void AddIron (int quantity)
    {
        int result = iron + quantity;

        try
        {
            if (result < ironStock)
            {
                iron = result;
            } 
            else
            {
                message = "Stock trop plein";
            }
        }
        catch
        {
            throw new System.Exception();
        }
    }

    void stockUpdate()
    {
        GameObject[] listStock = GameObject.FindGameObjectsWithTag("Stock");

        nbStock = listStock.Length;

        ironStock = nbStock * placeStock;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        stockUpdate();

        if(Input.GetMouseButtonDown(0))
        {
            Instantiate(stockage);
        }
        if(Input.GetMouseButtonDown(1))
        {
            GameObject oneStock = GameObject.FindGameObjectWithTag("Stock");

            Destroy(oneStock);
        }
    }
}
