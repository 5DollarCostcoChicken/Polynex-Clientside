using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyButton
{
    public Sprite texture;
    public int cooldown;
    public int currentCooldown;
    public int abilityNumber;
    public string charName;
    public bool spetcial = false;

    public BabyButton(Sprite t, int c, int cc, int a, string s)
    {
        texture = t;
        cooldown = c;
        currentCooldown = cc;
        abilityNumber = a;
        charName = s;
    }

    public BabyButton(Sprite t, int c, int cc, int a, string s, bool b)
    {
        texture = t;
        cooldown = c;
        currentCooldown = cc;
        abilityNumber = a;
        charName = s;
        spetcial = b;
    }
}
