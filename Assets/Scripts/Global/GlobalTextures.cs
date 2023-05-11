using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GlobalTextures : MonoBehaviour
{
    public GameObject initialSpriteBase;
    public GameObject initialNumberedSpriteBase;
    public GameObject initialNumberedSpriteBase2;
    public GameObject IceCubeBase;
    public static GameObject initialSprite;
    public static GameObject initialNumberedSprite;
    public static GameObject initialNumberedSprite2;
    public static GameObject IceCube;
    public Sprite RedBase;
    public Sprite BlueBase;
    public Sprite GreenBase;
    public Sprite AbilityBlockBase;
    public Sprite AbilityBlockButtonBase;
    public Sprite DazeBase;
    public Sprite StunBase;
    public Sprite StaggerBase;
    public Sprite BleedBase;
    public Sprite StealthBase;
    public Sprite OffenseDownBase;
    public Sprite OffenseUpBase;
    public Sprite DefenseDownBase;
    public Sprite DefenseUpBase;
    public Sprite BurningBase;
    public Sprite FreezingBase;
    public Sprite TrialBase;
    public Sprite PleadBase;
    public Sprite TauntBase;
    public Sprite GuardBase;
    public Sprite BlindBase;
    public Sprite HealOverTimeBase;
    public Sprite RetributionBase;
    public Sprite ResilienceUpBase;
    public Sprite ResilienceDownBase;
    public Sprite SpeedUpBase;
    public Sprite SpeedDownBase;
    public Sprite EvasionUpBase;
    public Sprite EvasionDownBase;
    public Sprite EnergyAbsorptionBase;
    public Sprite PotencyUpBase;
    public Sprite PotencyDownBase;
    public Sprite HealthUpBase;
    public Sprite HealthDownBase;
    public Sprite CorruptionBase;
    public Sprite InsanityBase;
    public Sprite VulnerableBase;
    public Sprite DisruptedBase;
    public Sprite DarkEnergyBase;
    public Sprite BonusProtectionBase;
    public Sprite ClumsyBase;
    public Sprite CritDamageUpBase;
    public Sprite CritChanceUpBase;
    public Sprite CritDamageDownBase;
    public Sprite CritChanceDownBase;
    public Sprite CritImmunityBase;
    public Sprite MarkedBase;
    public Sprite AccuracyUpBase;
    public Sprite AccuracyDownBase;
    public Sprite BrokenBase;
    public Sprite BuffImmunityBase;
    public Sprite FrostbiteBase;
    public static Sprite AbilityBlock;
    public static Sprite AbilityBlockButton;
    public static Sprite Daze;
    public static Sprite Stun;
    public static Sprite Stagger;
    public static Sprite Bleed;
    public static Sprite Stealth;
    public static Sprite OffenseDown;
    public static Sprite OffenseUp;
    public static Sprite DefenseDown;
    public static Sprite DefenseUp;
    public static Sprite Burning;
    public static Sprite Freezing;
    public static Sprite Trial;
    public static Sprite Taunt;
    public static Sprite Guard;
    public static Sprite HealOverTime;
    public static Sprite Retribution;
    public static Sprite ResilienceUp;
    public static Sprite ResilienceDown;
    public static Sprite SpeedUp;
    public static Sprite SpeedDown;
    public static Sprite Blind;
    public static Sprite EvasionUp;
    public static Sprite EvasionDown;
    public static Sprite Red;
    public static Sprite Blue;
    public static Sprite Green;
    public static Sprite EnergyAbsorption;
    public static Sprite PotencyUp;
    public static Sprite PotencyDown;
    public static Sprite HealthUp;
    public static Sprite HealthDown;
    public static Sprite Corruption;
    public static Sprite Insanity;
    public static Sprite Vulnerable;
    public static Sprite Disrupted;
    public static Sprite DarkEnergy;
    public static Sprite BonusProtection;
    public static Sprite Clumsy;
    public static Sprite CritDamageUp;
    public static Sprite CritChanceUp;
    public static Sprite CritDamageDown;
    public static Sprite CritChanceDown;
    public static Sprite CritImmunity;
    public static Sprite Marked;
    public static Sprite Plead;
    public  static Sprite AccuracyUp;
    public static Sprite AccuracyDown;
    public static Sprite Broken;
    public static Sprite BuffImmunity;
    public static Sprite Frostbite;
    void Start()
    {
        initialSprite = initialSpriteBase;
        initialNumberedSprite = initialNumberedSpriteBase;
        initialNumberedSprite2 = initialNumberedSpriteBase2;
        initialNumberedSprite.GetComponentInChildren<TextMeshPro>().outlineColor = new Color(255, 255, 255, 255);
        AbilityBlock= AbilityBlockBase;
        AbilityBlockButton = AbilityBlockButtonBase;
        Daze = DazeBase;
        Stun = StunBase;
        Stagger = StaggerBase;
        Bleed = BleedBase;
        Stealth = StealthBase;
        OffenseDown = OffenseDownBase;
        OffenseUp = OffenseUpBase;
        DefenseDown = DefenseDownBase;
        DefenseUp = DefenseUpBase;
        Burning = BurningBase;
        Freezing = FreezingBase;
        Trial = TrialBase;
        Taunt = TauntBase;
        Guard = GuardBase;
        HealOverTime = HealOverTimeBase;
        Retribution = RetributionBase;
        ResilienceUp = ResilienceUpBase;
        ResilienceDown = ResilienceDownBase;
        SpeedDown = SpeedDownBase;
        SpeedUp = SpeedUpBase;
        Blind = BlindBase;
        EvasionUp = EvasionUpBase;
        EvasionDown = EvasionDownBase;
        Red = RedBase;
        Blue = BlueBase;
        Green = GreenBase;
        EnergyAbsorption = EnergyAbsorptionBase;
        HealthUp = HealthUpBase;
        HealthDown = HealthDownBase;
        PotencyUp = PotencyUpBase;
        PotencyDown = PotencyDownBase;
        Corruption = CorruptionBase;
        Insanity = InsanityBase;
        Vulnerable = VulnerableBase;
        Disrupted = DisruptedBase;
        Clumsy = ClumsyBase;
        CritChanceDown = CritChanceDownBase;
        CritChanceUp = CritChanceUpBase;
        CritDamageDown = CritDamageDownBase;
        CritDamageUp = CritDamageUpBase;
        CritImmunity = CritImmunityBase;
        Marked = MarkedBase;
        DarkEnergy = DarkEnergyBase;
        BonusProtection = BonusProtectionBase;
        AccuracyDown = AccuracyDownBase;
        AccuracyUp = AccuracyUpBase;
        Plead = PleadBase;
        Broken = BrokenBase;
        BuffImmunity = BuffImmunityBase;
        Frostbite = FrostbiteBase;
        IceCube = IceCubeBase;
    }

    public void setTexture(ArrayList array)
    {
        GameObject icon = (GameObject)array[0];
        switch (array[1])
        {
            case "abilityBlock":
                icon.GetComponent<SpriteRenderer>().sprite = AbilityBlock;
                break;
            case "abilityBlockButton":
                icon.GetComponent<SpriteRenderer>().sprite = AbilityBlockButton;
                break;
            case "daze":
                icon.GetComponent<SpriteRenderer>().sprite = Daze;
                break;
            case "stagger":
                icon.GetComponent<SpriteRenderer>().sprite = Stagger;
                break;
            case "stun":
                icon.GetComponent<SpriteRenderer>().sprite = Stun;
                break;
            case "stealth":
                icon.GetComponent<SpriteRenderer>().sprite = Stealth;
                break;
            case "bleed":
                icon.GetComponent<SpriteRenderer>().sprite = Bleed;
                break;
            case "offenseDown":
                icon.GetComponent<SpriteRenderer>().sprite = OffenseDown;
                break;
            case "offenseUp":
                icon.GetComponent<SpriteRenderer>().sprite = OffenseUp;
                break;
            case "defenseDown":
                icon.GetComponent<SpriteRenderer>().sprite = DefenseDown;
                break;
            case "defenseUp":
                icon.GetComponent<SpriteRenderer>().sprite = DefenseUp;
                break;
            case "burning":
                icon.GetComponent<SpriteRenderer>().sprite = Burning;
                break;
            case "freezing":
                icon.GetComponent<SpriteRenderer>().sprite = Freezing;
                break;
            case "trial":
                icon.GetComponent<SpriteRenderer>().sprite = Trial;
                break;
            case "taunt":
                icon.GetComponent<SpriteRenderer>().sprite = Taunt;
                break;
            case "guard":
                icon.GetComponent<SpriteRenderer>().sprite = Guard;
                break;
            case "heal":
                icon.GetComponent<SpriteRenderer>().sprite = HealOverTime;
                break;
            case "retribution":
                icon.GetComponent<SpriteRenderer>().sprite = Retribution;
                break;
            case "resilienceUp":
                icon.GetComponent<SpriteRenderer>().sprite = ResilienceUp;
                break;
            case "resilienceDown":
                icon.GetComponent<SpriteRenderer>().sprite = ResilienceDown;
                break;
            case "speedUp":
                icon.GetComponent<SpriteRenderer>().sprite = SpeedUp;
                break;
            case "speedDown":
                icon.GetComponent<SpriteRenderer>().sprite = SpeedDown;
                break;
            case "blind":
                icon.GetComponent<SpriteRenderer>().sprite = Blind;
                break;
            case "evasionUp":
                icon.GetComponent<SpriteRenderer>().sprite = EvasionUp;
                break;
            case "evasionDown":
                icon.GetComponent<SpriteRenderer>().sprite = EvasionDown;
                break;
            case "energyAbsorption":
                icon.GetComponent<SpriteRenderer>().sprite = EnergyAbsorption;
                break;
            case "healthStealUp":
                icon.GetComponent<SpriteRenderer>().sprite = DefenseUp;
                break;
            case "potencyUp":
                icon.GetComponent<SpriteRenderer>().sprite = PotencyUp;
                break;
            case "potencyDown":
                icon.GetComponent<SpriteRenderer>().sprite = PotencyDown;
                break;
            case "healthUp":
                icon.GetComponent<SpriteRenderer>().sprite = HealthUp;
                break;
            case "healthDown":
                icon.GetComponent<SpriteRenderer>().sprite = HealthDown;
                break;
            case "corruption":
                icon.GetComponent<SpriteRenderer>().sprite = Corruption;
                break;
            case "insanity":
                icon.GetComponent<SpriteRenderer>().sprite = Insanity;
                break;
            case "vulnerable":
                icon.GetComponent<SpriteRenderer>().sprite = Vulnerable;
                break;
            case "disrupted":
                icon.GetComponent<SpriteRenderer>().sprite = Disrupted;
                break;
            case "darkEnergy":
                icon.GetComponent<SpriteRenderer>().sprite = DarkEnergy;
                break;
            case "bonusProtection":
                icon.GetComponent<SpriteRenderer>().sprite = BonusProtection;
                break;
            case "clumsy":
                icon.GetComponent<SpriteRenderer>().sprite = Clumsy;
                break;
            case "critChanceUp":
                icon.GetComponent<SpriteRenderer>().sprite = CritChanceUp;
                break;
            case "critChanceDown":
                icon.GetComponent<SpriteRenderer>().sprite = CritChanceDown;
                break;
            case "critDamageUp":
                icon.GetComponent<SpriteRenderer>().sprite = CritDamageUp;
                break;
            case "critDamageDown":
                icon.GetComponent<SpriteRenderer>().sprite = CritChanceDown;
                break;
            case "critImmunity":
                icon.GetComponent<SpriteRenderer>().sprite = CritImmunity;
                break;
            case "marked":
                icon.GetComponent<SpriteRenderer>().sprite = Marked;
                break;
            case "accuracyUp":
                icon.GetComponent<SpriteRenderer>().sprite = AccuracyUp;
                break;
            case "accuracyDown":
                icon.GetComponent<SpriteRenderer>().sprite = AccuracyDown;
                break;
            case "broken":
                icon.GetComponent<SpriteRenderer>().sprite = Broken;
                break;
            case "buffImmunity":
                icon.GetComponent<SpriteRenderer>().sprite = BuffImmunity;
                break;
            case "frostbite":
                icon.GetComponent<SpriteRenderer>().sprite = Frostbite;
                break;
        }
    }
}
