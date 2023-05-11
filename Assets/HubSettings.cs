using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubSettings : MonoBehaviour
{
    [SerializeField] GameObject everything;
    [SerializeField] GameObject[] layouts;

    public void displaySettings()
    {
        everything.SetActive(true);
    }
    public void hideSettings()
    {
        everything.SetActive(false);
    }
    public void switchLayout(int layoutGroup)
    {
        for (int i = 0; i < layouts.Length; i++)
        {
            if (layoutGroup != i)
                layouts[i].SetActive(false);
        }
        layouts[layoutGroup].SetActive(true);
    }
}
