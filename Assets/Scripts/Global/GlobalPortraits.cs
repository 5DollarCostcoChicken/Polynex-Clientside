using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPortraits : MonoBehaviour
{
    public static List<AbilityImageList> characterAbilityIcons = new List<AbilityImageList>();
    public List<AbilityImageList> chAI = new List<AbilityImageList>();
    private void Update()
    {
        characterAbilityIcons = chAI;
    }
}
