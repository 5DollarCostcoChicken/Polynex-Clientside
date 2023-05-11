using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claimed : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(copeAboutIt());
    }
    IEnumerator copeAboutIt()
    {
        yield return new WaitForSeconds(1);
        if (this.name.Contains("(Clone)"))
            Destroy(gameObject);
    }
}
