using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stock : MonoBehaviour
{
    public int totalSpace;
    public int usedSpace;

    public void StockUpdate(int quantityByStock)
    {
        GameObject[] listStock = GameObject.FindGameObjectsWithTag("Stock");
        
        if (usedSpace > totalSpace)
        {
            usedSpace = totalSpace;
        }
        totalSpace = listStock.Length * quantityByStock;
    }
}
