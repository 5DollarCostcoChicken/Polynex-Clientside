using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffsDebuffs : MonoBehaviour
{
    public List<Effect> BuffList = new List<Effect>();
    public List<Effect> DebuffList = new List<Effect>();
    public List<Effect> BlueBuffList = new List<Effect>();
    public List<Effect> BurnList = new List<Effect>();
    public List<GameObject> Images = new List<GameObject>();
    public GameObject healthAndArmor;
    public GlobalTextures globalTextures;
    GameObject frostbiteCube;
    //hey you should sort this stuff alphabetically i think perhaps maybe
    //buffs
    public Effect BonusProtection = new Effect("bonusProtection", 0, false, "Green", "Bonus Protection: \nthis unit gains bonus protection, scaling with max health");

    public Effect AccuracyUp = new Effect("accuracyUp", 0, false, "Green", "Accuracy Up: \nThe chance to land a hit increased by 25%");
    public Effect Agility = new Effect("agility", 0, false, "Green", "Agility: \nThis unit's next attack, if able, will be evaded"); //b
    public Effect BlessedSoul = new Effect("blessedSoul", 0, false, "Green", "Blessed Soul: \nIncrease all damage by 5%. Increase critical chance by 20%, and critical hits deal an additional 5% true damage");
    public Effect Brace = new Effect("brace", 0, false, "Green", "Brace: \nIncreased Dodge Change, Increased Block Chance, Increased Parry Chance, all by 20%");
    public Effect CorruptedTaunt = new Effect("corruptedTaunt", 0, false, "Green", "Corrupted Taunt: \nEnemies must target this unit; when attacked this buff is dispelled.");
    public Effect CritChanceUp = new Effect("critChanceUp", 0, false, "Green", "Critical Chance Up: \nChance to critically hit increased by 50%");
    public Effect CritDamageUp = new Effect("critDamageUp", 0, false, "Green", "Critical Damage Up: \nCritical damage multiplier increased by 25%");
    public Effect CritImmunity = new Effect("critImmunity", 0, false, "Green", "Critical Hit Immunity: \nThis unit cannot be critically hit"); //b
    public Effect DamageImmunity = new Effect("damageImmunity", 0, false, "Green", "Damage Immunity: \nThis unit cannot take damage");  //b //to save time, you could edit damagenumbers() thing so it just return; if this is active
    public Effect DefenseUp = new Effect("defenseUp", 0, false, "Green", "Defense Up: \nDefense increased by 50%");
    public Effect EvasionUp = new Effect("evasionUp", 0, false, "Green", "Evasion Up: \nDodge chance increased by 30%");
    public Effect Focus = new Effect("focus", 0, false, "Green", "Focus: \nThis unit's next attack is guaranteed to be a critical hit"); //b
    public Effect Fury = new Effect("fury", 0, true, "Green", "Fury: \nOffense increases by +5% per stack");
    public Effect HealthUp = new Effect("healthUp", 0, false, "Green", "Health Up: \nMax health increased by 25%");
    public Effect HealthStealUp = new Effect("healthStealUp", 0, false, "Green", "Health Steal Up: \nHealth steal increased by 50%");
    public Effect OffenseUp = new Effect("offenseUp", 0, false, "Green", "Offense Up: \nOffense increased by 50%");
    public Effect PotencyUp = new Effect("potencyUp", 0, false, "Green", "Potency Up: \nChance to land debuffs is increased by 30%");
    public Effect Rage = new Effect("rage", 0, true, "Green", "Rage: \n+5% Offense, Crit Chance, and Speed per stack");
    public Effect ResilienceUp = new Effect("resilienceUp", 0, false, "Green", "Resilience Up: \nThis unit will resist debuffs, if able");
    public Effect Retribution = new Effect("retribution", 0, false, "Green", "Retribution: \nThis unit has 100% counter chance");
    public Effect SpeedUp = new Effect("speedUp", 0, false, "Green", "Speed Up: \nSpeed increased by 20%");
    public Effect Stealth = new Effect("stealth", 0, false, "Green", "Stealth: \nEnemies cannot target this unit, this unit cannot be countered"); //b
    public Effect Taunt = new Effect("taunt", 0, false, "Green", "Taunt: \nEnemies must target this unit, if possible"); //b
    public Effect TrueSight = new Effect("trueSight", 0, false, "Green", "True Sight: \nThis unit can ignore taunts and stealth; guaranteed to dodge the next enemy attack."); //b //special

    //blue buffs
    public Effect BloodRage = new Effect("bloodRage", 0, false, "Blue", "Blood Rage: \nVarying buffs based on ability");
    public Effect Broken = new Effect("broken", 0, false, "Blue", "Broken: \nWhen applied, unit’s turn meter is set to 0. This unit can’t counter, block, or parry, and this unit's speed is set to 1/3rd its original value. Miss the next turn.");
    public Effect BurningTip = new Effect("burningTip", 0, false, "Blue", "Burning Tip: \nThis unit gains 25% bonus damage, and inflicts 2 stacks of burning");
    public Effect Channeling = new Effect("channeling", 0, false, "Blue", "Channeling: \nThis unit is charging an ability");
    public Effect ChargedShot = new Effect("chargedShot", 0, false, "Blue", "Charged Shot: \nThis unit's attack deals damage to all enemies");
    public Effect Clumsy = new Effect("clumsy", 0, true, "Blue", "Clumsy: \nAccuracy decreased by 10%, offense increased by 15%");
    public Effect Crescendo = new Effect("crescendo", 0, true, "Blue", "Crescendo: \nAccuracy increased by 10%, max 3 stacks");
    public Effect DarkEnergy = new Effect("darkEnergy", 0, true, "Blue", "Dark Energy: \nFor each stack reduce max protection by 5%, and on the next attack deal bonus magical damage equal to 5% of the target’s max health"); //special
    public Effect ElectrocutedTip = new Effect("electrocutedTip", 0, false, "Blue", "Electrocuted Tip: \nThis unit gains25% bonus damage, and inflict 2 stacks of shocked");
    public Effect EnergyAbsorption = new Effect("energyAbsorption", 0, true, "Blue", "Energy Absorption: \nWhen 'Unleashed' is used, consume all stacks and deal damage an additional time for each stack.");
    public Effect FreezingTip = new Effect("ereezingTip", 0, false, "Blue", "Freezing Tip: \nThis unit gains 25% bonus damage, and inflict 2 stacks of freezing");
    public Effect GiftersBlessing = new Effect("giftersBlessing", 0, false, "Blue", "Gifter's Blessing: \nVarying Buffs based on unit.");
    public Effect GodsMark = new Effect("godsMark", 0, false, "Blue", "God's Mark: \nSpeed reduced by 50%. Reduce magical and physical armor by 30%. Can only attack Forlorn. Forlorn gains 10% of this unit's stats. This unit gains taunt. Once this unit dies dies, Forlorn gains 2% of this unit's stats. ");
    public Effect Guard = new Effect("guard", 0, false, "Blue", "Guard: \nBlock chance increased by 30%, can’t parry while guarding");
    public Effect Hardened = new Effect("hardened", 0, false, "Blue", "Hardened: \nCannot be dazed, staggered, or gain turn meter effects, and whenever this unit is damaged by any attack increase all stats by 2%, stacking. Hardened cannot be copied.");
    public Effect HealthPotion = new Effect("healthPotion", 0, true, "Blue", "Health Potion: \nWhen this unit would be defeated, revive at 50% max health");
    public Effect HexBeyondHope = new Effect("hexBeyondHope", 0, false, "Blue", "Hex: Beyond Hope \nSpeed and offense reduced by 20%");
    public Effect HexBloodCurse = new Effect("hexBloodCurse", 0, false, "Blue", "Hex: Blood Curse \nThis unit takes double damage from bleed.");
    public Effect HexDevourLight = new Effect("hexDevourLight", 0, false, "Blue", "Hex: Devour Light \nWhenever this unit would gain a buff, instead inflict bleed, and the enemy team gains a stack of Healing Over Time.");
    public Effect HexIsolation = new Effect("hexIsolation", 0, false, "Blue", "Hex: Isolation \nThis unit cannot be called to assist.");
    public Effect HexRagingInferno = new Effect("hexRagingInferno", 0, false, "Blue", "Hex: Raging Inferno: \nThis unit takes damage from a permanent burning effect. When this unit is defeated, inflict 4 stacks of burning on all its allies.");
    public Effect Insanity = new Effect("insanity", 0, false, "Blue", "Insanity: \nIf an enemy's next attack lands a debuff on this unit, gain +5 defense and max health stacking. Insanity is removed when this unit gains a buff");
    public Effect Kanashibari = new Effect("kanashibari", 0, false, "Blue", "Kanashibari: \nThis unit’s speed is set to 0 and cannot evade or attack out of turn. In addition, a Yōkai copy is summoned to the first available slot on Kurayami’s team. This effect cannot be dispelled, and is removed when the Yōkai clone is defeated.");
    public Effect LifeSteal = new Effect("lifeSteal", 0, true, "Blue", "Life Steal: \nThis unit gains buffs based on the amount of stacks of Life Steal they have (max 50 stacks) \n1: +10 offense and + 10 crit chance \n5: gain Hellfire ability \n10: gain + 25 crit chance and + 25 offense \n15: Gain crit chance up and crit damage up if this character scores a crit \n25: Deal extra damage based on the target max health(5 %). \n40: Gain protection on crit hits(5 %). \n50: decrease all cooldown on crit hit and can bulletstorm ability.");
    public Effect PaleSacrifice = new Effect("paleSacrifice", 0, true, "Blue", "Pale Sacrifice: \nThis unit's max health, defense, physical and magical resistance, evasion, and parry chance all decrease by 20%, while offense increases by 50%, speed increases by 10%, and crit chance increases by 15%.");
    public Effect PoisonTip = new Effect("poisonTip", 0, false, "Blue", "Poison Tip: \n25% bonus damage, and inflict 2 stacks of poison");
    public Effect ReinforcedArmor = new Effect("reinforcedArmor", 0, true, "Blue", "text");
    public Effect SharpenedTip = new Effect("sharpenedTip", 0, false, "Blue", "Sharpened Tip: \n25% bonus damage, and inflict 2 stacks of bleed");
    public Effect SoulOfCinder = new Effect("soulOfCinder", 0, false, "Blue", "text");
    public Effect SpiritOfTheWolf = new Effect("spiritOfTheWolf", 0, true, "Blue", "Spirit Of The Wolf: \nA spirit assists Nomad for the rest of the battle, boosting his abilties");
    public Effect SturdyIngot = new Effect("sturdyIngot", 0, false, "Blue", "text");
    public Effect Trial = new Effect("trial", 0, false, "Blue", "Trial: \nThis unit gains an extra ability, Plead, which gives all enemies offense up, defense up, health steal up, and removes Trial from this enemy"); //special
    public Effect Thunder = new Effect("thunder", 0, true, "Blue", "Thunder: \nAt 5 stacks, unlock the Hailstorm ability");
    public Effect Uncoordinated = new Effect("uncoordinated", 0, false, "Blue", "Uncoordinated: \nThis unit has a 10% chance to accidentally hit allies with attack; on hit, enemies gain 5% turn meter; on hit, allies gain 10% turn meter.");
    public Effect Untargetable = new Effect("untargetable", 0, false, "Blue", "Untargetable: \nCannot be targeted unless the only unit active."); //b

    //debuffs
    public Effect AbilityBlock = new Effect("abilityBlock", 0, false, "Red", "Ability Block: \nThis unit cannot use special abilities");
    public Effect AccuracyDown = new Effect("accuracyDown", 0, false, "Red", "Accuracy Down: \nChance to land next hit decreased by 25%");
    public Effect Blind = new Effect("blind", 0, false, "Red", "Blind: \nThis unit's next attack is guaranteed to miss; can’t counter, block, dodge, or parry until blind expires"); //b
    public Effect Bloodlust = new Effect("bloodlust", 0, false, "Red", "Bloodlust: \nWhenever damage is dealt, 25% of that damage heals nezerothian allies. Increase offense by 25%. Increase counter damage by 25%.");
    public Effect BuffImmunity = new Effect("buffImmunity", 0, false, "Red", "Buff Immunity: \nThis unit cannot gain buffs"); //b
    public Effect Consume = new Effect("consume", 0, false, "Red", "Consume: \nThis unit cannot assist and cannot gain buffs. Deal 2% true damage per ally turn. This damage cannot defeat the enemy");
    public Effect Corruption = new Effect("corruption", 0, true, "Red", "Corruption: \nEach stack reduces Resilience, Resistance, and Defense by 2%. Culists and Risen allies take 5% less damage to corruption allies based on the amount of stacks and gain 2% potency and maximum health. Max 5 stacks. ");
    public Effect CritChanceDown = new Effect("critChanceDown", 0, false, "Red", "Critical Chance Down: \nChance to critically hit decreased by 50%");
    public Effect CritDamageDown = new Effect("critDamageDown", 0, false, "Red", "Critical Damage Down: \nCritical damage multiplier decreased by 25%");
    public Effect Daze = new Effect("daze", 0, false, "Red", "Daze: \nThis unit cannot be called to assist, can’t counter, no bonus turn meter");
    public Effect DefenseDown = new Effect("defenseDown", 0, false, "Red", "Defense Down: \nDefense decreased by 50%");
    public Effect Disrupted = new Effect("disrupted", 0, false, "Red", "Disrupted: \nIncreases this unit cooldowns by 1 and decreases accuracy by 50%");
    public Effect Drain = new Effect("drain", 0, false, "Red", "Drain: \nEvery ally or enemy turn, deal 2% damage to this unit and grant protection up (3%) to all nezeroth enemies. This damage cannot defeat this unit");
    public Effect EvasionDown = new Effect("evasionDown", 0, false, "Red", "Evasion Down: \nDodge chance decreased by 30%");
    public Effect Frostbite = new Effect("frostbite", 0, false, "Red", "Frostbite: \nMiss the next turn; speed set to 50% of original value.");
    public Effect HealingImmunity = new Effect("healingImmunity", 0, false, "Red", "Healing Immunity: \nThis unit cannot heal");
    public Effect Harvest = new Effect("harvest", 0, false, "Red", "Harvest: \nAll enemies lose (5%) health per turn which heal this unit. Enemies cannot gain health up or heal over time. Once a debuffed unit drops below 50% health, gain daze for 2 turns and ability block for 1 turn");
    public Effect HealthDown = new Effect("healthDown", 0, false, "Red", "HealthDown: \nHealth decreased by 25%");
    public Effect HealthStealDown = new Effect("healthStealDown", 0, false, "Red", "Health Steal Down: \nHealth steal decreased by 50%");
    public Effect Marked = new Effect("marked", 0, false, "Red", "Marked: \nAllies must target this unit until they are reduced to 50% health or lower.");
    public Effect OffenseDown = new Effect("offenseDown", 0, false, "Red", "Offense Down: \nOffense decreased by 50%");
    public Effect PotencyDown = new Effect("potencyDown", 0, false, "Red", "Potency Down: \nPotency decreased by 30%");
    public Effect Petrified = new Effect("petrified", 0, false, "Red", "Petrified: \nUnits with Petrified cannot gain turn meter, but cannot be damaged. Petrified is only removed if this unit is defeated or the debuff is cleansed. If Lithos is defeated, Petrified is cleansed");
    public Effect Pierced = new Effect("pierced", 0, false, "Red", "Pierced: \nDamage inflicted on this unit bypasses armor/magic, and pierced is removed.");
    public Effect ResilienceDown = new Effect("resilienceDown", 0, false, "Red", "Resilience Down: \nResistance to debuffs set to 0");
    public Effect SoulSteal = new Effect("soulSteal", 0, false, "Red", "Soul Steal: \nSpeed, armor and magical armor is reduce 5%. Special attacks cannot crit instead they deal damage to the enemy team (5%).");
    public Effect SpeedDown = new Effect("speedDown", 0, false, "Red", "Speed Down: \nOffense decreased by 20%");
    public Effect Stagger = new Effect("stagger", 0, false, "Red", "Stagger: \nThe next attack against this unit sets their turn meter to 0"); //b
    public Effect Stun = new Effect("stun", 0, false, "Red", "Stun: \nThis unit will lose their next turn. Cannot attack out of turn, block, or evade"); //b
    public Effect Vulnerable = new Effect("vulnerable", 0, true, "Red", "Vulnerable: \nThe next attack dealt to this enemy deals bonus damage equal to 20% of their max health."); //b
    

    //burns
    public Effect HealOverTime = new Effect("heal", 0, true, "Green", "Healing over Time: \nAt the start of this unit’s turn, heal for 5% max health.");
    public Effect Bleed = new Effect("bleed", 0, true, "Red", "Bleed: \nAt the start of this unit’s turn, take damage equal to 5% max health.");
    public Effect Poison = new Effect("poison", 0, true, "Red", "Poison: \nAt the start of this unit’s turn, take damage equal to 4% max health; defense and offense decreased by 20%.");
    public Effect Burning = new Effect("burning", 0, true, "Red", "Burning: \nAt the start of any units’s turn, take damage equal to 2% max health");
    public Effect Freezing = new Effect("freezing", 0, true, "Red", "Freezing: \nAt the start of this unit’s turn, take damage equal to 3% max health; speed decreased by 20%");
    public Effect Shocked = new Effect("shocked", 0, true, "Red", "Shocked: \nAt the start of this unit’s turn, take damage equal to 4% max health, critical avoidance decreased by 30%");

    //original player's stats
    bool isLeader;
    bool Armor;
    int turnMeter;
    float health;
    float maxHealth;
    int offense;
    float speed;
    float CriticalChance;
    float CriticalAvoidance;
    float armor;
    int DamageAmount;
    int damage;
    float PhysResist;
    float MagResist;
    float Potency;
    float Resilience;
    float dodge;
    float block;
    float counter;
    float parry;
    float HealthSteal;
    bool canAssist;

    //other
    public Character character;
    readonly string[] str = new string[2];


    // / / / / / / / / / / / / / / / / / / / / / / / / Creating Image Array / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void ImageArray()
    {
        foreach (GameObject instance in Images)
        {
            Destroy(instance);
        }
        for (int i = 0; i < BlueBuffList.Count; i++)
        {
            GameObject instance;
            ArrayList array = new ArrayList();
            if (BlueBuffList[i].numbered)
            {
                GlobalTextures.initialNumberedSprite.GetComponentInChildren<TextMeshPro>().SetText("" + BlueBuffList[i].getAmount() * 1);
                switch (BlueBuffList[i].color)
                {
                    case "Green":
                        GlobalTextures.initialNumberedSprite2.GetComponent<SpriteRenderer>().sprite = GlobalTextures.Green;
                        break;
                    case "Red":
                        GlobalTextures.initialNumberedSprite2.GetComponent<SpriteRenderer>().sprite = GlobalTextures.Red;
                        break;
                    case "Blue":
                        GlobalTextures.initialNumberedSprite2.GetComponent<SpriteRenderer>().sprite = GlobalTextures.Blue;
                        break;
                }
                instance = Instantiate(GlobalTextures.initialNumberedSprite, healthAndArmor.transform);
            }
            else
                instance = Instantiate(GlobalTextures.initialSprite, healthAndArmor.transform);
            instance.transform.position = healthAndArmor.transform.position + (.8f * 100 * healthAndArmor.transform.localScale.x * healthAndArmor.transform.up) + ((BlueBuffList.Count + DebuffList.Count + BuffList.Count + BurnList.Count - 1) * -.375f * 100 * healthAndArmor.transform.localScale.x * healthAndArmor.transform.right);
            instance.transform.position += .75f * 100 * healthAndArmor.transform.localScale.x * i * healthAndArmor.transform.right;
            array.Add(instance);
            array.Add(BlueBuffList[i].getName());
            globalTextures.SendMessage("setTexture", array);
            Images.Add(instance);
        }
        for (int i = 0; i < BuffList.Count; i++)
        {
            GameObject instance;
            ArrayList array = new ArrayList();
            if (BuffList[i].numbered)
            {
                GlobalTextures.initialNumberedSprite.GetComponentInChildren<TextMeshPro>().SetText("" + BuffList[i].getAmount() * 1);
                switch (BuffList[i].color)
                {
                    case "Green":
                        GlobalTextures.initialNumberedSprite2.GetComponent<SpriteRenderer>().sprite = GlobalTextures.Green;
                        break;
                    case "Red":
                        GlobalTextures.initialNumberedSprite2.GetComponent<SpriteRenderer>().sprite = GlobalTextures.Red;
                        break;
                    case "Blue":
                        GlobalTextures.initialNumberedSprite2.GetComponent<SpriteRenderer>().sprite = GlobalTextures.Blue;
                        break;
                }
                instance = Instantiate(GlobalTextures.initialNumberedSprite, healthAndArmor.transform);
            }
            else
                instance = Instantiate(GlobalTextures.initialSprite, healthAndArmor.transform);
            instance.transform.position = healthAndArmor.transform.position + (.8f * 100 * healthAndArmor.transform.localScale.x * healthAndArmor.transform.up) + ((BlueBuffList.Count + DebuffList.Count + BuffList.Count + BurnList.Count - 1) * -.375f * 100 * healthAndArmor.transform.localScale.x * healthAndArmor.transform.right);
            instance.transform.position += (i + BlueBuffList.Count) * .75f * 100 * healthAndArmor.transform.localScale.x * healthAndArmor.transform.right;
            array.Add(instance);
            array.Add(BuffList[i].getName());
            globalTextures.SendMessage("setTexture", array);
            Images.Add(instance);
        }
        for (int i = 0; i < DebuffList.Count; i++)
        {
            GameObject instance;
            ArrayList array = new ArrayList();
            if (DebuffList[i].numbered && DebuffList[i].getAmount() > 0)
            {
                GlobalTextures.initialNumberedSprite.GetComponentInChildren<TextMeshPro>().SetText("" + DebuffList[i].getAmount() * 1);
                switch (DebuffList[i].color)
                {
                    case "Green":
                        GlobalTextures.initialNumberedSprite2.GetComponent<SpriteRenderer>().sprite = GlobalTextures.Green;
                        break;
                    case "Red":
                        GlobalTextures.initialNumberedSprite2.GetComponent<SpriteRenderer>().sprite = GlobalTextures.Red;
                        break;
                    case "Blue":
                        GlobalTextures.initialNumberedSprite2.GetComponent<SpriteRenderer>().sprite = GlobalTextures.Blue;
                        break;
                }
                instance = Instantiate(GlobalTextures.initialNumberedSprite, healthAndArmor.transform);
            }
            else
                instance = Instantiate(GlobalTextures.initialSprite, healthAndArmor.transform);
            instance.transform.position = healthAndArmor.transform.position + (.8f * 100 * healthAndArmor.transform.localScale.x * healthAndArmor.transform.up) + ((BlueBuffList.Count + DebuffList.Count + BuffList.Count + BurnList.Count - 1) * -.375f * 100 * healthAndArmor.transform.localScale.x * healthAndArmor.transform.right);
            instance.transform.position += (i + BuffList.Count + BlueBuffList.Count) * .75f * 100 * healthAndArmor.transform.localScale.x * healthAndArmor.transform.right;
            array.Add(instance);
            array.Add(DebuffList[i].getName());
            globalTextures.SendMessage("setTexture", array);
            Images.Add(instance);
        }
        for (int i = 0; i < BurnList.Count; i++)
        {
            ArrayList array = new ArrayList();
            GlobalTextures.initialNumberedSprite.GetComponentInChildren<TextMeshPro>().SetText("" + BurnList[i].getAmount() * 1);
            switch (BurnList[i].color)
            {
                case "Green":
                    GlobalTextures.initialNumberedSprite2.GetComponent<SpriteRenderer>().sprite = GlobalTextures.Green;
                    break;
                case "Red":
                    GlobalTextures.initialNumberedSprite2.GetComponent<SpriteRenderer>().sprite = GlobalTextures.Red;
                    break;
                case "Blue":
                    GlobalTextures.initialNumberedSprite2.GetComponent<SpriteRenderer>().sprite = GlobalTextures.Blue;
                    break;
            }
            GameObject instance = Instantiate(GlobalTextures.initialNumberedSprite, healthAndArmor.transform);
            instance.transform.position = healthAndArmor.transform.position + (.8f * 100 * healthAndArmor.transform.localScale.x * healthAndArmor.transform.up) + ((BlueBuffList.Count + DebuffList.Count + BuffList.Count + BurnList.Count - 1) * -.375f * 100 * healthAndArmor.transform.localScale.x * healthAndArmor.transform.right);
            instance.transform.position += (i + BuffList.Count + DebuffList.Count + BlueBuffList.Count) * .75f * 100 * healthAndArmor.transform.localScale.x * healthAndArmor.transform.right;
            array.Add(instance);
            array.Add(BurnList[i].getName());
            globalTextures.SendMessage("setTexture", array);
            Images.Add(instance);
        }
    }

    // / / / / / / / / / / / / / / / / / / / / / / / / / / Start of Turn / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void StartOfTurn()
    {
        GlobalVariables.addToWhomstJustStartedTurn(character);
        List<Effect> tempList = new List<Effect>();
        str[0] = "White";
        str[1] = "me";
        foreach (Effect effect in BurnList)
        {
            this.SendMessage(effect.getName());
            if (effect.getAmount() == 0)
                this.SendMessage(effect.getName() + "Deactivate");
            else
                tempList.Add(effect);
        }
        BurnList = tempList;
        character.SendMessage("abilityUpdate");
        if (character.health < 0)
        {
            character.BurnedToDeath_SkullEmoji_();
            GlobalVariables.TakingTurn = false;
        }
    }

    // / / / / / / / / / / / / / / / / / / / / / / / / / / End of Turn / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void EndOfTurn()
    {
        GlobalVariables.addToWhomstJustEndedTurn(character);
        List<Effect> tempList = new List<Effect>();
        if (character.abilityBlock && !DebuffList.Contains(AbilityBlock))
        {
            character.abilityBlock = false;
            character.TauntIgnore = false;
        }
        foreach (Effect effect in BuffList)
        {
            effect.setAmount(effect.getAmount() - 1);
            if (effect.getAmount() == 0)
            {
                this.SendMessage(effect.getName() + "Deactivate");
            }
            else
            {
                tempList.Add(effect);
                if (effect.numbered)
                    this.SendMessage(effect.getName() + "Subtract");
            }
        }
        BuffList = tempList;
        List<Effect> tempList2 = new List<Effect>();
        foreach (Effect effect in DebuffList)
        {
            effect.setAmount(effect.getAmount() - 1);
            if (effect.getAmount() == 0)
            {
                this.SendMessage(effect.getName() + "Deactivate");
            }
            else
            {
                tempList2.Add(effect);
                if (effect.numbered)
                    this.SendMessage(effect.getName() + "Deactivate");
            }
        }
        DebuffList = tempList2;
        List<Effect> tempList3 = new List<Effect>();
        foreach (Effect effect in BlueBuffList)
        {
            if (effect.getName().Equals("guard") || effect.getName().Equals("channeling"))
            {
                effect.setAmount(effect.getAmount() - 1);
                if (effect.getAmount() == 0)
                {
                    this.SendMessage(effect.getName() + "Deactivate");
                }
                else
                    tempList3.Add(effect);
            }
            else
                tempList3.Add(effect);
        }
        BlueBuffList = tempList3;
        character.SendMessage("abilityUpdate");
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Cleanse Buff / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void cleanseBuffs()
    {
        foreach (Effect effect in BuffList)
        {
            this.SendMessage(effect.getName() + "Deactivate");
        }
        BuffList.Clear();
        if (BurnList.Contains(HealOverTime))
        {
            this.SendMessage("healDeactivate");
            BurnList.Remove(HealOverTime);
        }
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / Cleanse Debuff / / / / / / / / / / / / / / / / / / / / / / / / / / / / / 
    public void cleanseDebuffs()
    {
        foreach (Effect effect in DebuffList)
        {
            this.SendMessage(effect.getName() + "Deactivate");
        }
        DebuffList.Clear();
        if (!BurnList.Contains(HealOverTime))
        {
            BurnList.Clear();
        }
        else
        {
            BurnList.Clear();
            this.SendMessage("healActivate");
        }
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Buffs to Debuffs / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void buffsToDebuffs()
    {
        List<Effect> tempList = new List<Effect>();
        List<Effect> tempList2 = new List<Effect>();
        List<Effect> tempList3 = DebuffList;
        foreach (Effect effect in BuffList)
        {
            if (effect.getName().Contains("Up"))
            {
                this.SendMessage(effect.getName() + "Deactivate");
                this.SendMessage(effect.getName().Substring(0, effect.getName().Length - 2) + "DownActivate");
                tempList2.Add(DebuffList[DebuffList.Count - 1]);
                tempList2[tempList2.Count - 1].add(effect.getAmount());
            }
            else
            {
                tempList.Add(effect);
            }
        }
            BuffList = tempList;
        if (BurnList.Contains(HealOverTime))
        {
            Bleed.burnAdd(HealOverTime.getAmount());
            BurnList.Remove(HealOverTime);
            if (!BurnList.Contains(Bleed))
                SendMessage("bleedActivate");
        }
        foreach (Effect effect in tempList2)
        {
            if (tempList3.Contains(effect))
            {
                effect.add(tempList3[tempList3.IndexOf(effect)].getAmount());
                tempList3.Remove(effect);
            }
            tempList3.Add(effect);
        }
        DebuffList = tempList3;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Debuffs to Buffs / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void debuffsToBuffs()
    {
        List<Effect> tempList = new List<Effect>();
        List<Effect> tempList2 = new List<Effect>();
        List<Effect> tempList3 = BuffList;
        foreach (Effect effect in DebuffList)
        {
            if (effect.getName().Contains("Down"))
            {
                this.SendMessage(effect.getName() + "Deactivate");
                this.SendMessage(effect.getName().Substring(0, effect.getName().Length - 4) + "UpActivate");
                tempList2.Add(BuffList[BuffList.Count - 1]);
                tempList2[tempList2.Count - 1].add(effect.getAmount());
            }
            else
            {
                tempList.Add(effect);
            }
        }
        DebuffList = tempList;
        if (BurnList.Contains(Bleed))
        {
            HealOverTime.burnAdd(Bleed.getAmount());
            BurnList.Remove(Bleed);
            if (!BurnList.Contains(HealOverTime))
                SendMessage("healActivate");
        }
        foreach (Effect effect in tempList2)
        {
            if (tempList3.Contains(effect))
            {
                effect.add(tempList3[tempList3.IndexOf(effect)].getAmount());
                tempList3.Remove(effect);
            }
            tempList3.Add(effect);
        }
        BuffList = tempList3;
        ImageArray();
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Debuffs to Buffs / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void buffDebuffSwitch()
    { 
        //a good question, for another time.
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Bleed & Heal / / / / / / / / / / / / / / / / / / / / / / / / / / / / 
    void heal()
    {
        str[0] = "Green";
        GlobalVariables.DamageNumberGreen.GetComponent<TextMeshProUGUI>().text = "" + (int)(character.maxHealth / 20);
        for (int i = 0; i < HealOverTime.getAmount(); i++)
        {
            if (!character.healingImmunity)
            {
                character.health += (character.maxHealth / 20);
                character.SendMessage("damageNombers", str);
            }
        }
        str[0] = "White";
        if (HealOverTime.getAmount() > 0)
            HealOverTime.burnAdd(-1);
    }
    public void healActivate()
    {
        BurnList.Add(HealOverTime);
        GlobalVariables.addToWhomstJustGainedBuff(character, "heal");
    }
    void healDeactivate()
    {
        HealOverTime.setAmount(0);
    }
    void bleed()
    {
        GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "" + (int)(character.maxHealth / 20);
        for (int i = 0; i < Bleed.getAmount(); i++)
        {
            if ((character.protection > 0 || character.bonusProtection > 0))
            {
                if (character.bonusProtection - (character.maxHealth / 20) < 0)
                {
                    int b = (int)((character.maxHealth / 20) - character.bonusProtection);
                    character.bonusProtection = 0;
                    character.maxBonusProtection = 0;
                    if (character.protection - b < 0)
                    {
                        health -= (b - character.protection);
                        character.protection = 0;
                    }
                    else
                        character.protection -= b;
                }
                else
                    character.bonusProtection -= (character.maxHealth / 20);
            }
            else
                character.health -= (character.maxHealth / 20);
            character.SendMessage("damageNombers", str);
        }
        if (Bleed.getAmount() > 0)
            Bleed.burnAdd(-1);
    }
    public void bleedActivate()
    {
        BurnList.Add(Bleed);
        GlobalVariables.addToWhomstJustGainedDebuff(character, "bleed");
    }
    void bleedDeactivate()
    {
        Bleed.setAmount(0);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Poison / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    void poison()
    {
        GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "" + (int)(character.maxHealth / 25);
        for (int i = 0; i < Poison.getAmount(); i++)
        {
            if ((character.protection > 0 || character.bonusProtection > 0))
            {
                if (character.bonusProtection - (character.maxHealth / 25) < 0)
                {
                    int b = (int)((character.maxHealth / 25) - character.bonusProtection);
                    character.bonusProtection = 0;
                    character.maxBonusProtection = 0;
                    if (character.protection - b < 0)
                    {
                        health -= (b - character.protection);
                        character.protection = 0;
                    }
                    else
                        character.protection -= b;
                }
                else
                    character.bonusProtection -= (character.maxHealth / 25);
            }
            else
                character.health -= (character.maxHealth / 25);
            character.SendMessage("damageNombers", str);
        }
        if (Poison.getAmount() > 0)
            Poison.burnAdd(-1);
    }
    public void poisonActivate()
    {
        BurnList.Add(Poison);
        character.offense = (int)(character.offense * .8f);
        character.defense = (int)(character.defense * .8f);
        GlobalVariables.addToWhomstJustGainedDebuff(character, "poison");
    }
    void poisonDeactivate()
    {
        character.offense = (int)(character.offense / .8f);
        character.defense = (int)(character.defense / .8f);
        Poison.setAmount(0);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Burning / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void burning()
    {
        if (!character.dead) {
            for (int i = 0; i < Burning.getAmount(); i++)
            {
                GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "" + (int)(character.maxHealth / 50);
                if ((character.protection > 0 || character.bonusProtection > 0))
                {
                    if (character.bonusProtection - (character.maxHealth / 50) < 0)
                    {
                        int b = (int)((character.maxHealth / 50) - character.bonusProtection);
                        character.bonusProtection = 0;
                        character.maxBonusProtection = 0;
                        if (character.protection - b < 0)
                        {
                            health -= (b - character.protection);
                            character.protection = 0;
                        }
                        else
                            character.protection -= b;
                    }
                    else
                        character.bonusProtection -= (character.maxHealth / 50);
                }
                else
                    character.health -= (character.maxHealth / 50);
                str[0] = "White";
                str[1] = "me";
                character.SendMessage("damageNombers", str);
            }
            if (Burning.getAmount() > 0 && character.turnMeter >= 100)
                Burning.burnAdd(-1);
            if (character.health < 0)
            {
                character.BurnedToDeath_SkullEmoji_();
            }
        }
    }
    public void burningActivate()
    {
        BurnList.Add(Burning);
        GlobalVariables.addToWhomstJustGainedDebuff(character, "burning");
    }
    void burningDeactivate()
    {
        Burning.setAmount(0);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Freezing / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    void freezing()
    {
        GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "" + (int)(character.maxHealth / 33);
        for (int i = 0; i < Freezing.getAmount(); i++)
        {
            if ((character.protection > 0 || character.bonusProtection > 0))
            {
                if (character.bonusProtection - (character.maxHealth / 33) < 0)
                {
                    int b = (int)((character.maxHealth / 33) - character.bonusProtection);
                    character.bonusProtection = 0;
                    character.maxBonusProtection = 0;
                    if (character.protection - b < 0)
                    {
                        health -= (b - character.protection);
                        character.protection = 0;
                    }
                    else
                        character.protection -= b;
                }
                else
                    character.bonusProtection -= (character.maxHealth / 33);
            }
            else
                character.health -= (character.maxHealth / 33);
            character.SendMessage("damageNombers", str);
        }
        if (Freezing.getAmount() > 0)
            Freezing.burnAdd(-1);
    }
    public void freezingActivate()
    {
        BurnList.Add(Freezing);
        character.speed = (int)(character.speed * .8f);
        GlobalVariables.addToWhomstJustGainedDebuff(character, "freezing");
    }
    void freezingDeactivate()
    {
        character.speed = (int)(character.speed / .8f);
        Freezing.setAmount(0);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Shocked / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    void shocked()
    {
        GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "" + (int)(character.maxHealth / 25);
        for (int i = 0; i < Shocked.getAmount(); i++)
        {
            if ((character.protection > 0 || character.bonusProtection > 0))
            {
                if (character.bonusProtection - (character.maxHealth / 25) < 0)
                {
                    int b = (int)((character.maxHealth / 25) - character.bonusProtection);
                    character.bonusProtection = 0;
                    character.maxBonusProtection = 0;
                    if (character.protection - b < 0)
                    {
                        health -= (b - character.protection);
                        character.protection = 0;
                    }
                    else
                        character.protection -= b;
                }
                else
                    character.bonusProtection -= (character.maxHealth / 25);
            }
            else
                character.health -= (character.maxHealth / 25);
            character.SendMessage("damageNombers", str);
        }
        if (Shocked.getAmount() > 0)
            Shocked.burnAdd(-1);
    }
    public void shockedActivate()
    {
        BurnList.Add(Shocked);
        character.CriticalAvoidance = (int)(character.CriticalAvoidance * .8f);
        GlobalVariables.addToWhomstJustGainedDebuff(character, "shocked");
    }
    void shockedDeactivate()
    {
        character.CriticalAvoidance = (int)(character.CriticalAvoidance / .8f);
        Shocked.setAmount(0);
    }

    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Bonus Protection / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void bonusProtectionActivate(int percentage)
    {
        BuffList.Add(BonusProtection);
        character.maxBonusProtection += character.maxHealth * percentage / 100;
        character.bonusProtection += character.maxHealth * percentage / 100;
        GlobalVariables.addToWhomstJustGainedBuff(character, "bonusProtection");
    }
    public void bonusProtectionAdd(int percentage)
    {
        character.maxBonusProtection += character.maxHealth * percentage / 100;
        character.bonusProtection += character.maxHealth * percentage / 100;
        GlobalVariables.addToWhomstJustGainedBuff(character, "bonusProtection");
    }
    void bonusProtectionDeactivate()
    {
        BonusProtection.setAmount(0);
        character.maxBonusProtection = 0;
        character.bonusProtection = 0;
    }

    //                                                                                                                                                      BUFFS


    // / / / / / / / / / / / / / / / / / / / / / / / / / / Accuracy Up / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void accuracyUpActivate()
    {
        BuffList.Add(AccuracyUp);
        character.accuracy += 10;
        GlobalVariables.addToWhomstJustGainedBuff(character, "accuracyUp");
    }
    void accuracyUpDeactivate()
    {
        character.accuracy -= 10;
        AccuracyUp.setAmount(0);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Agility / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void agilityActivate()
    {
        BuffList.Add(Agility);
        character.Agility = true;
        GlobalVariables.addToWhomstJustGainedBuff(character, "agility");
    }
    void agilityDeactivate()
    {
        character.Agility = false;
        Agility.setAmount(0);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / CritChanceUp / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void critChanceUpActivate()
    {
        BuffList.Add(CritChanceUp);
        character.CriticalChance += 50;
        GlobalVariables.addToWhomstJustGainedBuff(character, "critChanceUp");
    }
    void critChanceUpDeactivate()
    {
        character.CriticalChance -= 50;
        CritChanceUp.setAmount(0);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / CritDamageUp / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void critDamageUpActivate()
    {
        BuffList.Add(CritDamageUp);
        character.CritMultiplier += .25f;
        GlobalVariables.addToWhomstJustGainedBuff(character, "critDamageUp");
    }
    void critDamageUpDeactivate()
    {
        character.CritMultiplier -= .25f;
        CritDamageUp.setAmount(0);
    }

    // / / / / / / / / / / / / / / / / / / / / / / / / / / / CritImmunity / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void criticalImmunityActivate()
    {
        BuffList.Add(CritImmunity);
        character.critImmunity = true;
        GlobalVariables.addToWhomstJustGainedBuff(character, "critImmunity");
    }
    void CritImmunityDeactivate()
    {
        character.critImmunity = false;
        CritImmunity.setAmount(0);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Defense Up / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void defenseUpActivate()
    {
        BuffList.Add(DefenseUp);
        character.defense = (int)(character.defense / .5f);
        GlobalVariables.addToWhomstJustGainedBuff(character, "defenseUp");
    }
    void defenseUpDeactivate()
    {
        character.defense = (int)(character.defense * .5f);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Evasion Up / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void evasionUpActivate()
    {
        BuffList.Add(EvasionUp);
        character.dodge += 30;
        GlobalVariables.addToWhomstJustGainedBuff(character, "evasionUp");
    }
    void evasionUpDeactivate()
    {
        character.dodge -= 30;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Focus / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void focusActivate()
    {
        BuffList.Add(Focus);
        character.Focus = true;
        GlobalVariables.addToWhomstJustGainedBuff(character, "focus");
    }
    void focusDeactivate()
    {
        character.Focus = false;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Health Up / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void healthUpActivate()
    {
        BuffList.Add(HealthUp);
        character.maxHealth *= 1.25f;
        character.health *= 1.25f;
        GlobalVariables.addToWhomstJustGainedBuff(character, "healthUp");
    }
    void healthUpDeactivate()
    {
        character.maxHealth /= 1.25f;
        character.health /= 1.25f;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Health Steal Up / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void healthStealUpActivate()
    {
        BuffList.Add(HealthStealUp);
        character.HealthSteal += 50;
        GlobalVariables.addToWhomstJustGainedBuff(character, "healthStealUp");
    }
    void healthStealUpDeactivate()
    {
        character.HealthSteal -= 50;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Offense Up / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void offenseUpActivate()
    {
        BuffList.Add(OffenseUp);
        character.offense = (int)(character.offense / .5f);
        GlobalVariables.addToWhomstJustGainedBuff(character, "offenseUp");
    }
    void offenseUpDeactivate()
    {
        character.offense = (int)(character.offense * .5f);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Potency Up / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void potencyUpActivate()
    {
        BuffList.Add(PotencyUp);
        character.Potency = (int)(character.Potency * 1.3f);
        GlobalVariables.addToWhomstJustGainedBuff(character, "potencyUp");
    }
    void potencyUpDeactivate()
    {
        character.Potency = (int)(character.Potency / 1.3f);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Rage / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void rageActivate()
    {
        BuffList.Add(Rage);
        character.offense = (int)(character.offense * 1.1f);
        character.Potency = (int)(character.Potency * 1.05f);
        character.speed = (int)(character.speed * 1.05f);
        GlobalVariables.addToWhomstJustGainedBuff(character, "rage");
    }
    public void rageAdd()
    {
        character.offense = (int)(character.offense * 1.1f);
        character.Potency = (int)(character.Potency * 1.05f);
        character.speed = (int)(character.speed * 1.05f);
        GlobalVariables.addToWhomstJustGainedBuff(character, "rage");
    }
    public void rageSubtract() 
    {
        character.offense = (int)(character.offense / 1.1f);
        character.Potency = (int)(character.Potency / 1.05f);
        character.speed = (int)(character.speed / 1.05f);
    }
    void rageDeactivate()
    {
        character.offense = (int)(character.offense / 1.1f);
        character.Potency = (int)(character.Potency / 1.05f);
        character.speed = (int)(character.speed / 1.05f);
        Rage.setAmount(0);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Resilience Up / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void resilienceUpActivate()
    {
        BuffList.Add(ResilienceUp);
        Resilience = character.Tenacity;
        character.Tenacity += 9999;
        GlobalVariables.addToWhomstJustGainedBuff(character, "resilienceUp");
    }
    void resilienceUpDeactivate()
    {
        character.Tenacity = Resilience;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Retribution / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void retributionActivate()
    {
        BuffList.Add(Retribution);
        counter = character.counter;
        character.counter = 100;
        GlobalVariables.addToWhomstJustGainedBuff(character, "retribution");
    }
    void retributionDeactivate()
    {
        character.counter = counter;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Speed Up / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void speedUpActivate()
    {
        BuffList.Add(SpeedUp);
        character.speed *= 1.2f;
        GlobalVariables.addToWhomstJustGainedBuff(character, "speedUp");
    }
    void speedUpDeactivate()
    {
        character.speed /= 1.2f;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Stealth / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void stealthActivate()
    {
        BuffList.Add(Stealth);
        character.Stealth = true;
        if (gameObject.CompareTag("EnemySelected"))
            gameObject.tag = "Enemy";
        if (gameObject.CompareTag("PlayerSelected"))
            gameObject.tag = "Ally";
        if (gameObject.CompareTag("Ally") || gameObject.CompareTag("AllyTurn") || gameObject.CompareTag("AllySelected") || gameObject.CompareTag("PlayerSelected"))
            GlobalVariables.AllyStealthers++;
        else
            GlobalVariables.EnemyStealthers++;
        GlobalVariables.addToWhomstJustGainedBuff(character, "stealth");
    }
    void stealthDeactivate()
    {
        character.Stealth = false;
        if (gameObject.CompareTag("Ally") || gameObject.CompareTag("AllyTurn") || gameObject.CompareTag("AllySelected") || gameObject.CompareTag("PlayerSelected"))
            GlobalVariables.AllyStealthers--;
        else
            GlobalVariables.EnemyStealthers--;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Taunt / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void tauntActivate()
    {
        BuffList.Add(Taunt);
        character.Taunt = true;
        if (gameObject.CompareTag("Ally") || gameObject.CompareTag("AllyTurn") || gameObject.CompareTag("AllySelected") || gameObject.CompareTag("PlayerSelected"))
            GlobalVariables.AllyTaunters++;
        else
            GlobalVariables.EnemyTaunters++;
        GlobalVariables.addToWhomstJustGainedBuff(character, "taunt");
    }
    void tauntDeactivate()
    {
        if (!BuffList.Contains(Marked))
            character.Taunt = false;
        if (gameObject.CompareTag("Ally") || gameObject.CompareTag("AllyTurn") || gameObject.CompareTag("AllySelected") || gameObject.CompareTag("PlayerSelected"))
            GlobalVariables.AllyTaunters--;
        else
            GlobalVariables.EnemyTaunters--;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Corrupted Taunt / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void corruptedTauntActivate()
    {
        BuffList.Add(CorruptedTaunt);
        character.Taunt = true;
        if (gameObject.CompareTag("Ally") || gameObject.CompareTag("AllyTurn") || gameObject.CompareTag("AllySelected") || gameObject.CompareTag("PlayerSelected"))
            GlobalVariables.AllyTaunters++;
        else
            GlobalVariables.EnemyTaunters++;
        GlobalVariables.addToWhomstJustGainedBuff(character, "corruptedTaunt");
    }
    void corruptedTauntDeactivate()
    {
        if (!BuffList.Contains(Marked))
            character.Taunt = false;
        if (gameObject.CompareTag("Ally") || gameObject.CompareTag("AllyTurn") || gameObject.CompareTag("AllySelected") || gameObject.CompareTag("PlayerSelected"))
            GlobalVariables.AllyTaunters--;
        else
            GlobalVariables.EnemyTaunters--;
    }

    //                                                                                                                                                    DEBUFFS



    // / / / / / / / / / / / / / / / / / / / / / / / / / / Ability Block / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void abilityBlockActivate()
    {
        DebuffList.Add(AbilityBlock);
        character.abilityBlock = true;
        GlobalVariables.addToWhomstJustGainedDebuff(character, "abilityBlock");
    }
    void abilityBlockDeactivate()
    {
        character.abilityBlock = false;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Accuracy Down / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void accuracyDownActivate()
    {
        DebuffList.Add(AccuracyDown);
        character.accuracy -= 10;
        GlobalVariables.addToWhomstJustGainedDebuff(character, "accuracyDown");
    }
    void accuracyDownDeactivate()
    {
        character.accuracy += 10;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / / Blind / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void blindActivate()
    {
        DebuffList.Add(Blind);
        character.Blind = true;
        counter = character.counter;
        character.counter = 0;
        dodge = character.dodge;
        character.dodge = 0;
        block = character.block;
        character.block = 0;
        parry = character.parry;
        character.parry = 0;
        GlobalVariables.addToWhomstJustGainedDebuff(character, "blind");
    }
    void blindDeactivate()
    {
        character.Blind = false;
        character.counter = counter;
        character.dodge = dodge;
        character.block = block;
        character.parry = parry;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Buff Immunity / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void buffImmunityActivate()
    {
        DebuffList.Add(BuffImmunity);
        character.buffImmunity = true;
        GlobalVariables.addToWhomstJustGainedDebuff(character, "buffImmunity");
    }
    void buffImmunityDeactivate()
    {
        character.buffImmunity = false;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Corruption / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void corruptionActivate()
    {
        DebuffList.Add(Corruption);
        character.Tenacity = (int)(character.Tenacity * .98f);
        character.defense = (int)(character.defense * .98f);
        character.offense = (int)(character.offense * .95f);
        GlobalVariables.addToWhomstJustGainedDebuff(character, "corruption");
    }
    public void corruptionAdd()
    {
        character.Tenacity = (int)(character.Tenacity * .98f);
        character.defense = (int)(character.defense * .98f);
        character.offense = (int)(character.offense * .95f);
        GlobalVariables.addToWhomstJustGainedDebuff(character, "corruption");
    }
    public void corruptionSubtract()
    {
        character.Tenacity = (int)(character.Tenacity / .98f);
        character.defense = (int)(character.defense / .98f);
        character.offense = (int)(character.offense / .95f);
    }
    void corruptionDeactivate()
    {
        character.Tenacity = (int)(character.Tenacity / .98f);
        character.defense = (int)(character.defense / .98f);
        character.offense = (int)(character.offense / .95f);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / / Daze / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void dazeActivate()
    {
        DebuffList.Add(Daze);
        counter = character.counter;
        character.counter = 0;
        character.turnMeterUpImmunity = true;
        character.canAttackOutOfTurn = false;
        GlobalVariables.addToWhomstJustGainedDebuff(character, "daze");
    }
    void dazeDeactivate()
    {
        character.counter = counter;
        character.turnMeterUpImmunity = false;
        character.canAttackOutOfTurn = true;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Defense Down / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void defenseDownActivate()
    {
        DebuffList.Add(DefenseDown);
        character.defense = (int)(character.defense * .5f);
        GlobalVariables.addToWhomstJustGainedDebuff(character, "defenseDown");
    }
    void defenseDownDeactivate()
    {
        character.defense = (int)(character.defense / .5f);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Disrupted / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void disruptedActivate()
    {
        DebuffList.Add(Disrupted);
        character.accuracy -= 50;
        GlobalVariables.addToWhomstJustGainedDebuff(character, "disrupted");
        if (!character.cooldownDecreaseImmunity)
        {
            this.SendMessage("cooldownChange", 1);
        }
    }
    void disruptedDeactivate()
    {
        character.accuracy += 50;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Evasion Down / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void evasionDownActivate()
    {
        DebuffList.Add(EvasionDown);
        character.dodge += 30;
        GlobalVariables.addToWhomstJustGainedDebuff(character, "evasionDown");
    }
    void evasionDownDeactivate()
    {
        character.dodge -= 30;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / / Frostbite / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void frostbiteActivate()
    {
        DebuffList.Add(Frostbite);
        character.Stun = true;
        dodge = character.dodge;
        character.dodge = 0;
        block = character.block;
        character.block = 0;
        parry = character.parry;
        character.parry = 0;
        speed = character.speed;
        character.speed /= 2;
        canAssist = character.canAttackOutOfTurn;
        character.canAttackOutOfTurn = false;
        GetComponentInChildren<Animator>().CrossFade("Hit", .1f);
        GetComponentInChildren<Animator>().speed = 0;
        GlobalVariables.addToWhomstJustGainedDebuff(character, "frostbite");
        GameObject instance = Instantiate(GlobalTextures.IceCube, transform);
        instance.transform.rotation = new Quaternion(0, Random.Range(0, 360), 0, 0);
        frostbiteCube = instance;
    }
    void frostbiteDeactivate()
    {
        character.Stun = false;
        character.dodge = dodge;
        character.block = block;
        character.parry = parry;
        character.speed = speed;
        character.canAttackOutOfTurn = canAssist;
        character.abilityUpdate();
        GetComponentInChildren<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
        GetComponentInChildren<Animator>().speed = 1;
        Destroy(frostbiteCube);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Marked / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void markedActivate()
    {
        DebuffList.Add(Marked);
        character.Taunt = true;
        if (gameObject.CompareTag("Ally") || gameObject.CompareTag("AllyTurn") || gameObject.CompareTag("AllySelected") || gameObject.CompareTag("PlayerSelected"))
            GlobalVariables.AllyTaunters++;
        else
            GlobalVariables.EnemyTaunters++;
        GlobalVariables.addToWhomstJustGainedDebuff(character, "marked");
    }
    void markedDeactivate()
    {
        if (!BuffList.Contains(Taunt))
            character.Taunt = false;
        if (gameObject.CompareTag("Ally") || gameObject.CompareTag("AllyTurn") || gameObject.CompareTag("AllySelected") || gameObject.CompareTag("PlayerSelected"))
            GlobalVariables.AllyTaunters--;
        else
            GlobalVariables.EnemyTaunters--;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Offense Down / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void offenseDownActivate()
    {
        DebuffList.Add(OffenseDown);
        character.offense = (int)(character.offense * .5f);
        GlobalVariables.addToWhomstJustGainedDebuff(character, "offenseDown");
    }
    void offenseDownDeactivate()
    {
        character.offense = (int)(character.offense / .5f);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Potency Down / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void potencyDownActivate()
    {
        BuffList.Add(PotencyDown);
        character.Potency = (int)(character.Potency / 1.3f);
        GlobalVariables.addToWhomstJustGainedBuff(character, "potencyDown");
    }
    void potencyDownDeactivate()
    {
        character.Potency = (int)(character.Potency * 1.3f);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Resilience Down / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void resilienceDownActivate()
    {
        BuffList.Add(ResilienceDown);
        Resilience = character.Tenacity;
        character.Tenacity -= 9999;
        GlobalVariables.addToWhomstJustGainedDebuff(character, "resilienceDown");
    }
    void resilienceDownDeactivate()
    {
        character.Tenacity = Resilience;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Speed Down / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void speedDownActivate()
    {
        DebuffList.Add(SpeedDown);
        character.speed /= 1.2f;
        GlobalVariables.addToWhomstJustGainedDebuff(character, "speedDown");
    }
    void speedDownDeactivate()
    {
        character.speed *= 1.2f;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Stagger / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void staggerActivate()
    {
        DebuffList.Add(Stagger);
        character.Stagger = true;
        GlobalVariables.addToWhomstJustGainedDebuff(character, "stagger");
    }
    void staggerDeactivate()
    {
        character.Stagger = false;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / / Stun / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void stunActivate()
    {
        DebuffList.Add(Stun);
        character.Stun = true;
        dodge = character.dodge;
        character.dodge = 0;
        block = character.block;
        character.block = 0;
        parry = character.parry;
        character.parry = 0;
        canAssist = character.canAttackOutOfTurn;
        character.canAttackOutOfTurn = false;
        GetComponentInChildren<Animator>().CrossFade("StunStart", .1f);
        GlobalVariables.addToWhomstJustGainedDebuff(character, "stun");
    }
    void stunDeactivate()
    {
        character.Stun = false;
        character.dodge = dodge;
        character.block = block;
        character.parry = parry;
        character.canAttackOutOfTurn = canAssist;
        character.abilityUpdate();
        GetComponentInChildren<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Vulnerable / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void vulnerableActivate() 
    {
        DebuffList.Add(Vulnerable);
        character.Vulnerable = true;
        GlobalVariables.addToWhomstJustGainedDebuff(character, "vulnerable");
    }
    void vulnerableDeactivate()
    {
        character.Vulnerable = false;
    }


    //                                                                                                                                               BLUE BUFFS

    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Broken / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void brokenActivate()
    {
        BlueBuffList.Add(Broken);
        if (!character.turnMeterDownImmunity)
            character.turnMeter = 0;
        dodge = character.dodge;
        character.dodge = 0;
        block = character.block;
        character.block = 0;
        parry = character.parry;
        character.parry = 0;
        canAssist = character.canAttackOutOfTurn;
        character.canAttackOutOfTurn = false;
        speed = character.speed;
        character.speed /= 3;
    }
    void brokenDeactivate()
    {
        Broken.setAmount(0);
        character.dodge = dodge;
        character.block = block;
        character.parry = parry;
        character.canAttackOutOfTurn = canAssist;
        character.speed = speed;
        character.abilityUpdate();
    }
        // / / / / / / / / / / / / / / / / / / / / / / / / / / / Guard / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
        public void guardActivate()
    {
        BlueBuffList.Add(Guard);
        character.Guard = true;
        character.block += 30;
        parry = character.parry;
        character.parry = 0;
        character.currentIdle = "Guard";
        GetComponentInChildren<Animator>().CrossFade("Guard", .5f);
    }
    void guardDeactivate()
    {
        character.block -= 30;
        character.Guard = false;
        character.parry = parry;
        character.abilityUpdate();
        GetComponentInChildren<Animator>().CrossFade("Base Layer." + character.currentIdle, .25f);
        Guard.setAmount(0);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Energy Absorption / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void energyAbsorptionActivate()
    {
        BlueBuffList.Add(EnergyAbsorption);
    }
    void energyAbsorptionDeactivate()
    {
        EnergyAbsorption.setAmount(0);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Dark Energy / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void darkEnergyActivate()
    {
        BlueBuffList.Add(DarkEnergy);
    }
    void darkEnergyDeactivate()
    {
        DarkEnergy.setAmount(0);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Clumsy / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void clumsyActivate()
    {
        BlueBuffList.Add(Clumsy);
        character.accuracy -= 10;
        character.offense = (int)(character.offense * 1.15f);
    }
    public void clumsyAdd()
    {
        character.accuracy -= 10;
        character.offense = (int)(character.offense * 1.15f);
    }
    public void clumsySubtract()
    {
        character.accuracy += 10;
        character.offense = (int)(character.offense / 1.15f);
    }
    void clumsyDeactivate()
    {
        character.accuracy += 10;
        character.offense = (int)(character.offense / 1.15f);
        Clumsy.setAmount(0);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / / Channeling / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void channelingActivate()
    {
        BlueBuffList.Add(Channeling);
        character.channeling = true;
        dodge = character.dodge;
        character.dodge = 0;
        block = character.block;
        character.block = 0;
        parry = character.parry;
        character.parry = 0;
        canAssist = character.canAttackOutOfTurn;
        character.canAttackOutOfTurn = false;
    }
    void channelingDeactivate()
    {
        character.channeling = false;
        character.dodge = dodge;
        character.block = block;
        character.parry = parry;
        character.canAttackOutOfTurn = canAssist;
        character.abilityUpdate();
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Insanity / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void insanityActivate()
    {
        BlueBuffList.Add(Insanity);
    }
    void insanityDeactivate()
    {
        Insanity.setAmount(0);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Trial / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void trialActivate()
    {
        BlueBuffList.Add(Trial);
        if (!gameObject.name.Contains("Enemy"))
        {
                BabyButton PleadButton = new BabyButton(GlobalTextures.Plead, 0, 0, 100, "Plead");
                character.boutons.Add(PleadButton);
        }
    }
    void trialDeactivate()
    {
        Trial.setAmount(0);
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / Uncoordinated / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void uncoordinatedActivate()
    {
        BlueBuffList.Add(Uncoordinated);
        character.offense = (int)(character.offense * 1.5f);
        character.speed = (int)(character.speed * 1.5f);
        character.Tenacity = (int)(character.Tenacity * 1.5f);
        character.defense = (int)(character.defense * .5f);
    }
    void uncoordinatedDeactivate()
    {
        Uncoordinated.setAmount(0);
        character.offense = (int)(character.offense / 1.5f);
        character.speed = (int)(character.speed / 1.5f);
        character.Tenacity = (int)(character.Tenacity / 1.5f);
        character.defense = (int)(character.defense / .5f);
    }
    // / / / / / / / / / / / / / / / / / / / /  Eat my entire asshole. Fuck you / / / / / / / / / / / / / / / / / / / / / / / / 
    private void Start()
    {
        GlobalVariables.OnStartTurn += burning;
    }
}
