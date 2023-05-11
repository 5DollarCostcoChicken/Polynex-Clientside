using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class virioncheese : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().CrossFade("Base Layer.yeah", 2f);
        StartCoroutine(Animation());
    }
    IEnumerator Animation()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(50f);
            GetComponent<Animator>().CrossFade(null, 0f);
            GetComponent<Animator>().CrossFade("Base Layer.yeah", 2f);
        }
    }
}
