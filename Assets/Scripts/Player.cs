using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GestionResourcesStock gestionResourcesStock;
    public GameObject stockage;
    public int ironQuantityGet;
    public int ironQuantityByStock;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Instantiate(stockage);
        }
        if(Input.GetMouseButtonDown(1))
        {
            Destroy(GameObject.FindGameObjectWithTag("Stock"));
        }
        if(Input.GetMouseButtonDown(2))
        {
            gestionResourcesStock.iron.AddResources(ironQuantityGet, gestionResourcesStock.stock.totalSpace);
        }
        gestionResourcesStock.stock.StockUpdate(ironQuantityByStock);
    }
}
