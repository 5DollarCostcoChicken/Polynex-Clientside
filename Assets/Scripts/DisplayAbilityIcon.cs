using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAbilityIcon : MonoBehaviour
{
    [TextArea]
    public string displayText;
    public Image displayImage;
    public Text displayNumber;
    //public GameObject 
    public void Clicked()
    {
        StatusPopup.EnemyPop();
        StatusPopup.enemyText.text = displayText.Replace("\\n", "\n");
    }
}
