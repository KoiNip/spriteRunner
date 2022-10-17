using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coins : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            print("Thank you");
        }
        Debug.Log("Entered");
    }

    void OnTriggerStay(Collider other) 
    {

    }

    void OnTriggerExit(Collider other)
    {

    }

    private void OnCollisionEnter2D(Collision2D otherObj)
    {
        print("Thank you");
        Debug.Log("Entered");
    }

}
