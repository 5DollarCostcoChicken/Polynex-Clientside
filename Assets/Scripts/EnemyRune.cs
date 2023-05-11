using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRune : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        if (GlobalVariables.enemiesSelected.Length == 1 && GlobalVariables.enemiesSelected[0] != null)
        {
            this.GetComponent<Transform>().position = GlobalVariables.enemiesSelected[0].GetComponent<Transform>().position;
            this.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            this.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
