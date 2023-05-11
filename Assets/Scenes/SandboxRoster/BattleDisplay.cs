using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleDisplay : MonoBehaviour
{
    public string characterString;
    public GameObject model;
    public bool filled;
    public BattlePortraits portraits;
    public void instantiate(string character)
    {
        characterString = character;
        GameObject instance = Instantiate(portraits.charModels[int.Parse(characterString.Substring(0, 3))], this.transform);
        model = instance;
        instance.transform.position = this.transform.position + this.transform.up * 0.1f * this.transform.localScale.x;
        instance.transform.rotation = new Quaternion(0, 180, 6.3622f, 0);
        instance.transform.localScale = new Vector3(40, 520 / 0.071987f * this.transform.localScale.y, 40);
    }
    public void OnMouseUp()
    {
        if (filled)
        {
            if (!characterString.Equals("behemoth")) {
                Destroy(model.gameObject);
                if (int.Parse(characterString.Substring(0, 3)) < portraits.modifiableArray.Count)
                    portraits.modifiableArray.Insert(int.Parse(characterString.Substring(0, 3)), characterString);
                else
                {
                    portraits.modifiableArray.Add(characterString);
                }
            }
            filled = false;
            if (portraits.behemothList.Contains(characterString.Substring(0, 3)))
            {
                portraits.OnBehemothClick();
            }
            model = null;
            characterString = null;
            portraits.portraitArrayInstantiation();
        }
    }
}
