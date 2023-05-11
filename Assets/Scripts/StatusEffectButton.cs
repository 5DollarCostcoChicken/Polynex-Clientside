using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectButton : MonoBehaviour
{
    public void OnMouseDown()
    {
        StatusPopup.StatusPop();
        StatusPopup.statusImage.sprite = this.GetComponent<SpriteRenderer>().sprite;
        for (int i = 0; i < this.GetComponentInParent<BuffsDebuffs>().BuffList.Count; i++)
        {
            if (string.Compare(this.GetComponentInParent<BuffsDebuffs>().BuffList[i].getName(), this.GetComponent<SpriteRenderer>().sprite.name, true) == 0)
                StatusPopup.statusText.text = this.GetComponentInParent<BuffsDebuffs>().BuffList[i].getEffectText();
        }
        for (int i = 0; i < this.GetComponentInParent<BuffsDebuffs>().DebuffList.Count; i++)
        {
            if (string.Compare(this.GetComponentInParent<BuffsDebuffs>().DebuffList[i].getName(), this.GetComponent<SpriteRenderer>().sprite.name, true) == 0)
                StatusPopup.statusText.text = this.GetComponentInParent<BuffsDebuffs>().DebuffList[i].getEffectText();
        }
        for (int i = 0; i < this.GetComponentInParent<BuffsDebuffs>().BlueBuffList.Count; i++)
        {
            if (string.Compare(this.GetComponentInParent<BuffsDebuffs>().BlueBuffList[i].getName(), this.GetComponent<SpriteRenderer>().sprite.name, true) == 0)
                StatusPopup.statusText.text = this.GetComponentInParent<BuffsDebuffs>().BlueBuffList[i].getEffectText();
        }
        for (int i = 0; i < this.GetComponentInParent<BuffsDebuffs>().BurnList.Count; i++)
        {
            if (string.Compare(this.GetComponentInParent<BuffsDebuffs>().BurnList[i].getName(), this.GetComponent<SpriteRenderer>().sprite.name, true) == 0)
                StatusPopup.statusText.text = this.GetComponentInParent<BuffsDebuffs>().BurnList[i].getEffectText();
        }
    }
}
