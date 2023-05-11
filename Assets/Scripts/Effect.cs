using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    readonly string effectName;
    int amount;
    public bool numbered;
    public string color;
    public string effectText;
    public Effect(string n, int a, bool b, string c, string e)
    {
        effectName = n;
        amount = a;
        numbered = b;
        color = c;
        effectText = e;
    }
    public void add(int a)
    {
        if (amount < a)
            amount = a;
    }
    public void burnAdd(int a)
    {
        amount += a;
        if (amount < 0)
            amount = 0;
    }
    public string getName()
    {
        return effectName;
    }
    public int getAmount()
    {
        return amount;
    }
    public void setAmount(int a)
    {
        amount = a;
    }
    public string getEffectText()
    {
        return effectText;
    }

}
