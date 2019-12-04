using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour
{
    public string message;
    public int quantity;


    public void AddResources(int addedResources, int stockSpace)
    {
        int result = quantity + addedResources;

        try
        {
            if (result <= stockSpace)
            {
                quantity = result;
                
                message = "Success";
            } 
            else
            {
                message = "Error, stock full";
            }
        }
        catch
        {
            throw new System.Exception();
        }
    }
}
