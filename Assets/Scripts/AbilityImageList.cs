using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityImageList : MonoBehaviour
{
    public List<Sprite> AbilityIcons = new List<Sprite>();
    public List<string> AbilityTexts = new List<string>();
    public List<Sprite> PassiveIcons = new List<Sprite>();
    public List<string> PassiveTexts = new List<string>();
    public List<int> AbilityCooldowns = new List<int>();
    public List<int> CurrentAbilityCooldowns = new List<int>();
    public string characterName;
    public string displayName;
    public Sprite characterPortrait;
    public string mainElement;
}
