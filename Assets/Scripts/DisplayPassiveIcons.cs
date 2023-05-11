using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPassiveIcons : MonoBehaviour
{
    [TextArea]
    public string displayText;
    public Image displayImage;
    //public GameObject 
    public void Clicked()
    {
        StatusPopup.AbilityPop();
        StatusPopup.playerText.text = displayText.Replace("\\n", "\n");
    }
}
