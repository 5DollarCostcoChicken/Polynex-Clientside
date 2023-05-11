using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyRune : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        
        if (GlobalVariables.playerArray.Length == 1)
        {
            this.GetComponent<Transform>().position = GlobalVariables.playerArray[0].GetComponent<Transform>().position;
            this.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            this.GetComponent<MeshRenderer>().enabled = false;
        }
            
    }
}
