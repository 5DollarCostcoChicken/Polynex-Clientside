using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // this is for textmesh pro
using UnityEngine.UI; // This is so you can mess with UI elements such as Text and Images
public class Character : MonoBehaviour
{
    //stats
    public int level;
    public int star;
    public string nameOfChar;

    public bool isLeader;
    public bool Armor;
    public float turnMeter;
    public float health;
    public float maxHealth;
    public float protection;
    public bool physArmor;
    public bool magArmor;
    public float physArmorAmount;
    public float magArmorAmount;
    public float maxProtection;
    public float bonusProtection;
    public float maxBonusProtection;
    public int offense;
    public int defense;
    public float speed;
    public float CriticalChance;
    public float CriticalAvoidance;
    public float CritMultiplier;
    public float armor;
    public int DamageAmount;
    public GameObject TurnMeter;
    public GameObject HealthBar;
    public GameObject ArmorBar;
    public GameObject ArmorBg;
    public GameObject BonusArmorBar;
    public int damage;
    public bool dead;
    public float PhysResist;
    public float MagResist;
    public float Potency;
    public float Tenacity;
    public float dodge;
    public float block;
    public float counter;
    public float parry;
    public float HealthSteal;
    public int accuracy;
    public string mainElement;
    public Vector3 initial;
    public SpriteRenderer[] healthArray;
    public SpriteRenderer healthBars;
    public string currentIdle = "BattleIdle";
    public List<BabyButton> boutons = new List<BabyButton>();
    public bool turnUpdate = true;
    //buffs and debuffs
    public bool Focus = false;
    public bool Agility = false;
    public bool TrueSight = false;
    public bool Stagger = false;
    public bool Blind = false;
    public bool Vulnerable = false;
    public bool Stun = false;
    public bool Guard = false;
    public bool Taunt = false;
    public bool Stealth = false;
    public bool abilityBlock = false;
    public bool untargetable = false;
    public int actionQueue;
    readonly ArrayList buffsDebuffs = new ArrayList();
    public BuffsDebuffs buffsAndDebuffs;
    float chanceToLand = 0;
    public bool critical = false;
    public bool attackingOutOfTurn = false;
    public bool canAttackOutOfTurn = false;
    //immunities n whatever
    public bool TauntIgnore = false;
    public bool StealthIgnore = false;
    public bool turnMeterDownImmunity = false;
    public bool turnMeterUpImmunity = false;
    public bool critImmunity = false;
    public bool buffImmunity = false;
    public bool healingImmunity = false;
    public bool stunImmunity = false;
    public bool abilityBlockImmunity = false;
    public bool bypassArmor = false;
    public bool cooldownDecreaseImmunity = false;
    public bool cooldownIncreaseImmunity = false;
    public string CutsceneAble;
    public bool channeling = false;

    //tags
    public List<string> tags = new List<string>();
    public AbilityImageList abilities;

    //Sound Effect stuff
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource audioSource3;
    public AudioClip blockSound;
    public AudioClip parrySound;
    public string Gender;
    //particle shnozz
    public ParticleSystem hitSmoke;

    void Start()
    {
        maxProtection++;
        maxHealth++;
        var hitSmokeEmmision = hitSmoke.emission;
        hitSmokeEmmision.enabled = false;
        GetComponentInChildren<Animator>().CrossFade("Base Layer.BattleIdle", .01f);
        dead = false;
        healthArray = HealthBar.GetComponentsInChildren<SpriteRenderer>();
        healthBars.transform.localScale = new Vector3(.1983501f, .1885f, 1f);
        Color tmp = healthBars.GetComponent<SpriteRenderer>().color;
        tmp.a = .9f;
        healthBars.GetComponent<SpriteRenderer>().color = tmp;
        StartCoroutine(helthbars());
        initial = transform.position;
        CritMultiplier = 1.5f;
        StartCoroutine(test());
        maxBonusProtection = bonusProtection;
        audioSource1.volume = .5f;
        audioSource2.volume = .3f;
        audioSource3.volume = .1f;
        canAttackOutOfTurn = true;
        actionQueue = 0;
    }
    IEnumerator test()
    {
        yield return new WaitForSeconds(.01f);
        if (Armor == false)
        {
            protection = 0;
            maxProtection = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (turnMeter < 0)
            turnMeter = 0;
        if (health < 0)
            gameObject.tag = "Untagged";
        else if (health > maxHealth && maxHealth > 499)
            health = maxHealth;
        if (maxProtection + maxBonusProtection == 0)
        {
            ArmorBar.transform.localPosition = new Vector3(.756f, -10, .015f);
            ArmorBg.transform.localPosition = new Vector3(0, -10, 0);
        }
        else
        {
            ArmorBar.transform.localPosition = new Vector3(.756f, .28f, .015f);
            ArmorBg.transform.localPosition = new Vector3(0, .28f, 0);
        }
        if (turnMeter < 101)
            TurnMeter.transform.localScale = new Vector3(1.551842f * (turnMeter / 102.5f), 0.09402987f, 1f);
        else
            TurnMeter.transform.localScale = new Vector3(1.551842f, 0.09402987f, 1f);
        if (health < maxHealth / 7)
            HealthBar.transform.localScale = new Vector3(1.692429f * .15f, .265558f, 1f);
        else
            HealthBar.transform.localScale = new Vector3(1.692429f * (health / maxHealth) + .0001f, .265558f, 1f);
        if (bonusProtection > 0)
        {
            if (protection > 0)
            {
                ArmorBar.transform.localScale = new Vector3(1.515892f * (protection / (maxProtection + maxBonusProtection + .01f)), 0.09508187f, 1f);
                BonusArmorBar.transform.localScale = new Vector3(1f * (bonusProtection / (maxProtection + maxBonusProtection + .01f)) / ((protection + .01f) / (maxProtection + maxBonusProtection + .01f)), 1f, 1f);
            }
            else
            {
                ArmorBar.transform.localScale = new Vector3(.1f, 0.09508187f, 1f);
                BonusArmorBar.transform.localScale = new Vector3(1f * (bonusProtection / (maxBonusProtection + .01f)) / .069f, 1f, 1f);
            }
        }
        else
        {
            ArmorBar.transform.localScale = new Vector3(1.515892f * (protection / (maxProtection + maxBonusProtection + .01f)), 0.09508187f, 1f);
            BonusArmorBar.transform.localScale = new Vector3(0, 1f, 1f);
        }
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Damage Numbers / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void damageNombers(string[] str)
    {
        StartCoroutine(DamageNumbers(str));
    }
    IEnumerator DamageNumbers(string[] str)
    {
        string color = str[0];
        string who = str[1];
        Vector3 Position = transform.position;
        GameObject DamageNumbers = GlobalVariables.DamageNumbers;
        switch (color)
        {
            case "Red":
                GameObject red = Instantiate(GlobalVariables.RedTextCamera, new Vector3(transform.position.x, transform.position.y + 200, transform.position.z), GlobalVariables.DamageNumbers.transform.rotation);
                TextMeshProUGUI redText = red.GetComponentInChildren<TextMeshProUGUI>();
                RenderTexture newRedTexture = Instantiate(red.GetComponent<Camera>().targetTexture);
                yield return new WaitForSeconds(.02f);
                redText.text = GlobalVariables.DamageNumberRed.GetComponent<TextMeshProUGUI>().text;
                red.GetComponent<Camera>().targetTexture = newRedTexture;
                redText.fontSize = 60;
                redText.color = new Color(255, 255, 255, 255);
                Material r = new Material(GlobalVariables.DamageNumbers.GetComponent<ParticleSystemRenderer>().material)
                {
                    mainTexture = red.GetComponent<Camera>().targetTexture
                };
                GlobalVariables.DamageNumbers.GetComponent<ParticleSystemRenderer>().material = r;
                DamageNumbers = GlobalVariables.DamageNumbers;
                break;
            case "Critical":
                GameObject critical = Instantiate(GlobalVariables.RedTextCamera, new Vector3(transform.position.x, transform.position.y + 200, transform.position.z), GlobalVariables.DamageNumbers.transform.rotation);
                TextMeshProUGUI criticalText = critical.GetComponentInChildren<TextMeshProUGUI>();
                RenderTexture newCriticalTexture = Instantiate(critical.GetComponent<Camera>().targetTexture);
                yield return new WaitForSeconds(.02f);
                criticalText.text = GlobalVariables.DamageNumberRed.GetComponent<TextMeshProUGUI>().text;
                critical.GetComponent<Camera>().targetTexture = newCriticalTexture;
                criticalText.fontSize = 70;
                criticalText.color = new Color(255, 204, 0, 255);
                Material c = new Material(GlobalVariables.DamageNumbers.GetComponent<ParticleSystemRenderer>().material)
                {
                    mainTexture = critical.GetComponent<Camera>().targetTexture
                };
                GlobalVariables.DamageNumbers.GetComponent<ParticleSystemRenderer>().material = c;
                DamageNumbers = GlobalVariables.DamageNumbers;
                break;
            case "White":
                GameObject white = Instantiate(GlobalVariables.WhiteTextCamera, new Vector3(transform.position.x, transform.position.y + 200, transform.position.z), GlobalVariables.DamageNumbers1.transform.rotation);
                TextMeshProUGUI whiteText = white.GetComponentInChildren<TextMeshProUGUI>();
                RenderTexture newWhiteTexture = Instantiate(white.GetComponent<Camera>().targetTexture);
                yield return new WaitForSeconds(.02f);
                whiteText.text = GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text;
                white.GetComponent<Camera>().targetTexture = newWhiteTexture;
                whiteText.fontSize = 35;
                Material w = new Material(GlobalVariables.DamageNumbers1.GetComponent<ParticleSystemRenderer>().material)
                {
                    mainTexture = white.GetComponent<Camera>().targetTexture
                };
                GlobalVariables.DamageNumbers1.GetComponent<ParticleSystemRenderer>().material = w;
                DamageNumbers = GlobalVariables.DamageNumbers1;
                break;
            case "Green":
                GameObject green = Instantiate(GlobalVariables.GreenTextCamera, new Vector3(transform.position.x, transform.position.y + 200, transform.position.z), GlobalVariables.DamageNumbers2.transform.rotation);
                TextMeshProUGUI greenText = green.GetComponentInChildren<TextMeshProUGUI>();
                RenderTexture newGreenTexture = Instantiate(green.GetComponent<Camera>().targetTexture);
                yield return new WaitForSeconds(.02f);
                greenText.text = GlobalVariables.DamageNumberGreen.GetComponent<TextMeshProUGUI>().text;
                green.GetComponent<Camera>().targetTexture = newGreenTexture;
                greenText.fontSize = 40;
                Material g = new Material(GlobalVariables.DamageNumbers2.GetComponent<ParticleSystemRenderer>().material)
                {
                    mainTexture = green.GetComponent<Camera>().targetTexture
                };
                GlobalVariables.DamageNumbers2.GetComponent<ParticleSystemRenderer>().material = g;
                DamageNumbers = GlobalVariables.DamageNumbers2;
                break;
            case "Blue":
                GameObject blue = Instantiate(GlobalVariables.BlueTextCamera, new Vector3(transform.position.x, transform.position.y + 200, transform.position.z), GlobalVariables.DamageNumbers.transform.rotation);
                TextMeshProUGUI blueText = blue.GetComponentInChildren<TextMeshProUGUI>();
                RenderTexture newBlueTexture = Instantiate(blue.GetComponent<Camera>().targetTexture);
                yield return new WaitForSeconds(.02f);
                blueText.text = GlobalVariables.DamageNumberBlue.GetComponent<TextMeshProUGUI>().text;
                blue.GetComponent<Camera>().targetTexture = newBlueTexture;
                blueText.fontSize = 60;
                blueText.color = new Color(255, 255, 255, 255);
                Material b = new Material(GlobalVariables.DamageNumbers.GetComponent<ParticleSystemRenderer>().material)
                {
                    mainTexture = blue.GetComponent<Camera>().targetTexture
                };
                GlobalVariables.DamageNumbers.GetComponent<ParticleSystemRenderer>().material = b;
                DamageNumbers = GlobalVariables.DamageNumbers;
                break;
            case "CriticalBlue":
                GameObject criticalBlue = Instantiate(GlobalVariables.BlueTextCamera, new Vector3(transform.position.x, transform.position.y + 200, transform.position.z), GlobalVariables.DamageNumbers.transform.rotation);
                TextMeshProUGUI criticalBlueText = criticalBlue.GetComponentInChildren<TextMeshProUGUI>();
                RenderTexture newCriticalBlueTexture = Instantiate(criticalBlue.GetComponent<Camera>().targetTexture);
                yield return new WaitForSeconds(.02f);
                criticalBlueText.text = GlobalVariables.DamageNumberBlue.GetComponent<TextMeshProUGUI>().text;
                criticalBlue.GetComponent<Camera>().targetTexture = newCriticalBlueTexture;
                criticalBlueText.fontSize = 70;
                criticalBlueText.color = new Color(80, 100, 255, 255);
                Material cb = new Material(GlobalVariables.DamageNumbers.GetComponent<ParticleSystemRenderer>().material)
                {
                    mainTexture = criticalBlue.GetComponent<Camera>().targetTexture
                };
                GlobalVariables.DamageNumbers.GetComponent<ParticleSystemRenderer>().material = cb;
                DamageNumbers = GlobalVariables.DamageNumbers;
                break;
        }
        switch (who)
        {
            case "enemy":
                if (GlobalVariables.enemiesSelected.Length > 0)
                    Position = new Vector3(GlobalVariables.enemiesSelected[0].transform.position.x, GlobalVariables.DamageNumbers.transform.position.y, GlobalVariables.enemiesSelected[0].transform.transform.position.z);
                else
                    Position = new Vector3(transform.position.x, GlobalVariables.DamageNumbers.transform.position.y, transform.transform.position.z);
                break;
            case "me":
                Position = new Vector3(transform.position.x, GlobalVariables.DamageNumbers.transform.position.y, transform.transform.position.z);
                break;
            case "player":
                if (GlobalVariables.playersSelected.Length > 0)
                    Position = new Vector3(GlobalVariables.playersSelected[0].transform.position.x, GlobalVariables.DamageNumbers.transform.position.y, GlobalVariables.playersSelected[0].transform.transform.position.z);
                else
                    Position = new Vector3(transform.position.x, GlobalVariables.DamageNumbers.transform.position.y, transform.transform.position.z);
                break;
        }
        GameObject instance = Instantiate(DamageNumbers, Position, GlobalVariables.DamageNumbers.transform.rotation);
        yield return new WaitForSeconds(1);
        GlobalVariables.DamageNumberRed.fontSize = 0;
        GlobalVariables.DamageNumberWhite.fontSize = 0;
        GlobalVariables.DamageNumberGreen.fontSize = 0;
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Other Processes / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    IEnumerator hit(int damage, Character enemy, string attackType)
    {
        if (enemy != null) {
            if (!buffsAndDebuffs.BuffList.Contains(buffsAndDebuffs.DamageImmunity))
            {
                if ((protection > 0 || bonusProtection > 0) && !enemy.bypassArmor)
                {
                    switch (attackType)
                    {
                        case "physical":
                            if (physArmor)
                                damage = (int)(damage * physArmorAmount);
                            break;
                        case "magical":
                            if (magArmor)
                                damage = (int)(damage * magArmorAmount);
                            break;
                    }
                    if (bonusProtection - damage < 0)
                    {
                        int b = damage - (int)bonusProtection;
                        bonusProtection = 0;
                        maxBonusProtection = 0;
                        if (protection - b < 0)
                        {
                            health -= (b - protection);
                            protection = 0;
                        }
                        else
                            protection -= b;
                    }
                    else
                        bonusProtection -= damage;
                }
                else
                    health -= damage;
                string[] str = new string[2];
                str[1] = "me";
                if (attackType.Equals("magical"))
                {
                    str[0] = "Blue";
                    GlobalVariables.DamageNumberBlue.GetComponent<TextMeshProUGUI>().text = "" + damage;
                    if (enemy.critical)
                        str[0] = "CriticalBlue";
                }
                else if (attackType.Equals("physical"))
                {
                    str[0] = "Red";
                    GlobalVariables.DamageNumberRed.GetComponent<TextMeshProUGUI>().text = "" + damage;
                    if (enemy.critical)
                        str[0] = "Critical";
                }
                else
                {
                    GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "" + damage;
                    str[0] = "White";
                }
                StartCoroutine(DamageNumbers(str));
                GlobalVariables.addToWhomstJustGotHit(this.GetComponent<Character>());
                GlobalVariables.addToWhomstJustHit(enemy.GetComponent<Character>());
            }
        }
        if (health > 0)
        {
            if (health < maxHealth / 2 && buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Marked))
            {
                buffsAndDebuffs.Marked.setAmount(0);
                buffsAndDebuffs.SendMessage("markedDeactivate");
                buffsAndDebuffs.DebuffList.Remove(buffsAndDebuffs.Marked);
                buffsAndDebuffs.SendMessage("ImageArray");
            }
            GetComponentInChildren<Animator>().Play("Base Layer.Hit");
            if (!attackType.Equals("silent"))
            {
                switch (Gender)
                {
                    case "male":
                        GlobalSounds.playMaleHurt(audioSource3);
                        break;
                    case "female":
                        GlobalSounds.playFemaleHurt(audioSource3);
                        break;
                }
            }
            if (Stagger)
            {
                if (!turnMeterDownImmunity)
                    turnMeter = 0;
                buffsAndDebuffs.Stagger.setAmount(0);
                buffsAndDebuffs.SendMessage("staggerDeactivate");
                buffsAndDebuffs.DebuffList.Remove(buffsAndDebuffs.Stagger);
                buffsAndDebuffs.SendMessage("ImageArray");
            }
            if (buffsAndDebuffs.BuffList.Contains(buffsAndDebuffs.CorruptedTaunt))
            {
                buffsAndDebuffs.CorruptedTaunt.setAmount(0);
                buffsAndDebuffs.SendMessage("corruptedTauntDeactivate");
                buffsAndDebuffs.BuffList.Remove(buffsAndDebuffs.CorruptedTaunt);
                buffsAndDebuffs.SendMessage("ImageArray");                
            }
            if (Vulnerable)
            {
                GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "" + (int)(maxHealth / 5);                
                string[] e = { "White", "me" };
                for (int i = 0; i < buffsAndDebuffs.Vulnerable.getAmount(); i++) {
                    damage = (int)(maxHealth / 5);
                    if (!buffsAndDebuffs.BuffList.Contains(buffsAndDebuffs.DamageImmunity))
                    {
                        damageNombers(e);
                        if (protection > 0 || bonusProtection > 0)
                        {
                            if (bonusProtection - damage < 0)
                            {
                                int b = damage - (int)bonusProtection;
                                bonusProtection = 0;
                                maxBonusProtection = 0;
                                if (protection - b < 0)
                                {
                                    health -= (b - protection);
                                    protection = 0;
                                }
                                else
                                    protection -= b;
                            }
                            else
                                bonusProtection -= damage;
                        }
                        else
                            health -= damage;
                    }
                }
                buffsAndDebuffs.Vulnerable.setAmount(0);
                buffsAndDebuffs.SendMessage("vulnerableDeactivate");
                buffsAndDebuffs.DebuffList.Remove(buffsAndDebuffs.Vulnerable);
                buffsAndDebuffs.SendMessage("ImageArray");
            }
            if (enemy.buffsAndDebuffs.BlueBuffList.Contains(enemy.buffsAndDebuffs.DarkEnergy))
            {
                GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "" + (int)(maxHealth / 20);
                string[] e = { "White", "me" };
                for (int i = 0; i < enemy.buffsAndDebuffs.DarkEnergy.getAmount(); i++)
                {                    
                    damage = (int)(maxHealth / 20);
                    if (!buffsAndDebuffs.BuffList.Contains(buffsAndDebuffs.DamageImmunity))
                    {
                        damageNombers(e);
                        if (protection > 0 || bonusProtection > 0)
                        {
                            if (bonusProtection - damage < 0)
                            {
                                int b = damage - (int)bonusProtection;
                                bonusProtection = 0;
                                maxBonusProtection = 0;
                                if (protection - b < 0)
                                {
                                    health -= (b - protection);
                                    protection = 0;
                                }
                                else
                                    protection -= b;
                            }
                            else
                                bonusProtection -= damage;
                        }
                        else
                            health -= damage;
                    }
                }
                if ((enemy.name.Contains("Enemy") && GlobalVariables.EnemyLeaders[0].Equals("Superion")) || (!enemy.name.Contains("Enemy") && GlobalVariables.Leaders[0].Equals("Superion"))) {
                    enemy.buffsAndDebuffs.SendMessage("ImageArray");
                }
                else
                {
                    enemy.buffsAndDebuffs.DarkEnergy.setAmount(0);
                    enemy.buffsAndDebuffs.SendMessage("darkEnergyDeactivate");
                    enemy.buffsAndDebuffs.BlueBuffList.Remove(enemy.buffsAndDebuffs.DarkEnergy);
                    enemy.buffsAndDebuffs.SendMessage("ImageArray");
                }
            }
            var hitSmokeEmmision = hitSmoke.emission;
            hitSmokeEmmision.enabled = true;

            yield return new WaitForSeconds(.6f);
            hitSmokeEmmision.enabled = false;
            GetComponentInChildren<Animator>().CrossFade("Base Layer." + currentIdle, .25f);
        }
        else
        {
            StartCoroutine(ded());
            if (enemy != null)
                GlobalVariables.addToWhomstJustKilled(enemy);
        }

    }
    IEnumerator ded()
    {
        canAttackOutOfTurn = false;
        dead = true;
        block = 0;
        parry = 0;
        counter = 0;
        dodge = 0;
        if (GlobalVariables.assisters > 0)
        {
            while (GlobalVariables.assisters > 0)
                yield return new WaitForSeconds(.05f);
        }
        GlobalVariables.addToWhomstJustDied(this);
        Taunt = false;
        speed = 0;
        GetComponentInChildren<Animator>().Play("Base Layer.Ded");
        switch (Gender)
        {
            case "male":
                GlobalSounds.playMaleDie(audioSource3);
                break;
            case "female":
                GlobalSounds.playFemaleDie(audioSource3);
                break;
        }
        buffsAndDebuffs.cleanseBuffs();
        buffsAndDebuffs.cleanseDebuffs();
        foreach (Transform child in transform)
            Destroy(child.GetComponent<GameObject>());
        Collider[] colliders;
        colliders = this.GetComponentsInChildren<Collider>();
        foreach (Collider c in colliders)
        {
            c.enabled = false;
        }
        yield return new WaitForSeconds(.7f);
        if (GlobalVariables.enemiesSelected.Length == 0 || GlobalVariables.enemiesSelected[0] == this.gameObject)
        {  
            if (GlobalVariables.hostileArray.Length > 0)
                GlobalVariables.hostileArray[Random.Range(0, GlobalVariables.hostileArray.Length)].tag = "EnemySelected";
        }
        this.transform.position = new Vector3(transform.position.x, transform.position.y - 30f, transform.position.z);
        gameObject.tag = "Untagged";
        //Destroy(gameObject);
    }
    #region HealthBars
    IEnumerator helthbars()
    {
        yield return new WaitForSeconds(.02f);
        healthbars();
    }
    public void healthbars()
    {
        int count = (int)Mathf.Sqrt((maxHealth - 300) / 180);
        switch (count)
        {
            case 1:
                healthBars.sprite = GlobalVariables.health1;
                break;
            case 2:
                healthBars.sprite = GlobalVariables.health2;
                break;
            case 3:
                healthBars.sprite = GlobalVariables.health3;
                break;
            case 4:
                healthBars.sprite = GlobalVariables.health4;
                break;
            case 5:
                healthBars.sprite = GlobalVariables.health5;
                break;
            case 6:
                healthBars.sprite = GlobalVariables.health6;
                break;
            case 7:
                healthBars.sprite = GlobalVariables.health7;
                break;
            case 8:
                healthBars.sprite = GlobalVariables.health8;
                break;
            case 9:
                healthBars.sprite = GlobalVariables.health9;
                break;
            case 10:
                healthBars.sprite = GlobalVariables.health10;
                break;
            case 11:
                healthBars.sprite = GlobalVariables.health11;
                break;
            case 12:
                healthBars.sprite = GlobalVariables.health12;
                break;
            case 13:
                healthBars.sprite = GlobalVariables.health13;
                break;
        }
        if (count > 13)
            healthBars.sprite = GlobalVariables.health13;
    }
    #endregion
    public void abilityUpdate() //should be called whenever an ally or enemy uses an ability on them, to refresh max health and debuffs
    {
        if (this.name.Contains("Enemy"))
        {
            GlobalVariables.allStealthed = Stealth;
            for (int i = 0; i < GlobalVariables.hostileArray.Length; i++)
            {
                if (!GlobalVariables.hostileArray[i].GetComponent<Character>().Stealth)
                    GlobalVariables.allStealthed = false;
            }
        }
        if (health <= maxHealth / 4)
        {
            healthArray[0].color = Color.red;
            currentIdle = "hurtIdle";
        }
        else if (health <= maxHealth / 2)
        {
            healthArray[0].color = Color.yellow;
            currentIdle = "hurtIdle";
        }
        else
        {
            healthArray[0].color = Color.green;
            currentIdle = "BattleIdle";
        }
        if (Guard)
            currentIdle = "Guard";
        if (Stun)
            currentIdle = "StunLoop";
        GetComponentInChildren<Animator>().CrossFade("Base Layer." + currentIdle, .25f);
        healthbars();
        buffsAndDebuffs.ImageArray();
    }
    #region Avoiding Attacks
    IEnumerator Evasion()
    {
        audioSource2.clip = GlobalSounds.Woosh6;
        audioSource2.Play();
        GetComponentInChildren<Animator>().CrossFade("Base Layer.Dodge", .1f);
        yield return new WaitForSeconds(.6f);
        GetComponentInChildren<Animator>().CrossFade("Base Layer." + currentIdle, .25f);
    }
    IEnumerator Blocking()
    {
        audioSource1.clip = blockSound;
        audioSource1.Play();
        GetComponentInChildren<Animator>().CrossFade("Base Layer.Block", .025f);
        yield return new WaitForSeconds(.6f);
        GetComponentInChildren<Animator>().CrossFade("Base Layer." + currentIdle, .25f);
    }
    IEnumerator Parrying(GameObject enemy)
    {
        audioSource1.clip = parrySound;
        audioSource1.Play();
        GetComponentInChildren<Animator>().CrossFade("Base Layer.Parry", .1f);
        yield return new WaitForSeconds(.3f);
        enemy.GetComponentInChildren<Animator>().Play("Base Layer.Hit");
        if (nameOfChar.Equals("TempComm"))
        {
            DamageAmount = (int)(((((100 * ((level - 1) * 14 / 99f + 1) * ((Mathf.Pow((float)star - 1, 2) / 4f) + 1)) + Random.Range(0, 26)) * 1.75f) * (100 - enemy.GetComponent<Character>().PhysResist) / 100f) * (1 + Mathf.Log10((float)offense / enemy.GetComponent<Character>().defense) / 2.5f));
            string[] damageNumbah = new string[2];
            damageNumbah[0] = "Red";
            damageNumbah[1] = "me";
            damageNombers(damageNumbah);
            enemy.GetComponent<Character>().TempCommBasic(this.GetComponent<Character>());
        }
        GlobalSounds.playSlash(enemy.GetComponent<Character>().audioSource2);
        switch (enemy.GetComponent<Character>().Gender)
        {
            case "male":
                GlobalSounds.playMaleHurt(enemy.GetComponent<Character>().audioSource3);
                break;
            case "female":
                GlobalSounds.playFemaleHurt(enemy.GetComponent<Character>().audioSource3);
                break;
        }
        yield return new WaitForSeconds(.3f);
        GetComponentInChildren<Animator>().CrossFade("Base Layer." + currentIdle, .5f);
    }
    #endregion

    // / / / / / / / / / / / / / / / / / / / / / / / / / / Enemy Abilities / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    /*
     public void Character_Name_SimpleHit(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
    }
    public void Character_Name_DebuffInflicted(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
    if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
                chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
            else
                chanceToLand = 85;
            if (Random.Range(0, 100) < chanceToLand)
            {
                buffsAndDebuffs.OffenseDown.add(1);
                if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.OffenseDown))
                    buffsAndDebuffs.SendMessage("offenseDownActivate");
                GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "offenseDown");
            }
            else
            {
                GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
                string[] bonk = { "White", "me" };
                damageNombers(bonk); 
            }
    }
     */
    #region Glad
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Gladiator / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void GladBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playStab(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Random.Range(0, 100) < 70) && !turnMeterDownImmunity)
            turnMeter -= 20;
    }
    public void GladSpecial1(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playMetalOnFlesh(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.Stagger.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Stagger))
                buffsAndDebuffs.SendMessage("staggerActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "stagger");
            GlobalVariables.addToWhomstJustGainedDebuff(this, "stagger");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.Daze.add(1);
            if (buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Daze) && !stunImmunity)
            {
                buffsAndDebuffs.Stun.add(1);
                if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Stun))
                    buffsAndDebuffs.SendMessage("stunActivate");
                GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "stun");
                GlobalVariables.addToWhomstJustGainedDebuff(this, "stun");
            }
            else
            {
                buffsAndDebuffs.SendMessage("dazeActivate");
                GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "daze");
                GlobalVariables.addToWhomstJustGainedDebuff(this, "daze");
            }
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }

    }
    public void GladSpecial2(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        audioSource1.clip = GlobalSounds.StabIn;
        audioSource1.Play();
        GlobalSounds.playFleshSquish(audioSource2);
        StartCoroutine(hit(damage, enemy, "physical"));
        //calculating chance for debuff to land
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        //the bleed
        for (int i = 0; i < 3; i++)
        {
            if (Random.Range(0, 100) < chanceToLand && !nameOfChar.Contains("PlagueDoctor")){
                buffsAndDebuffs.Bleed.burnAdd(1);
                if (!buffsAndDebuffs.BurnList.Contains(buffsAndDebuffs.Bleed))
                    buffsAndDebuffs.SendMessage("bleedActivate");
                GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "bleed");
                GlobalVariables.addToWhomstJustGainedDebuff(this, "bleed");
            }
            else
            {
                GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
                string[] bonk = {"White","me"};
                damageNombers(bonk);
            }
        }
    }
    #endregion
    #region Frontline Grunt
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Frontline Grunt / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void FrontGruntBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.DefenseDown.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.DefenseDown))
                buffsAndDebuffs.SendMessage("defenseDownActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "defenseDown");
            GlobalVariables.addToWhomstJustGainedDebuff(this, "defenseDown");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    #endregion
    #region Crusaders
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Templar Paladin / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void TempPalBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
                chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
            else
                chanceToLand = 85;
            if (Random.Range(0, 100) < chanceToLand)
            {
                buffsAndDebuffs.OffenseDown.add(1);
                if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.OffenseDown))
                    buffsAndDebuffs.SendMessage("offenseDownActivate");
                GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "offenseDown");
                GlobalVariables.addToWhomstJustGainedDebuff(this, "offenseDown");
            }
            else
            {
                GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
                string[] bonk = { "White", "me" };
                damageNombers(bonk);
            }
    }
    public void TempPalSpecial1(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        //GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Templar Protector / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void TempProtBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
    }
    public void TempProtSpecial1(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        audioSource1.clip = enemy.GetComponent<Character>().blockSound;
        audioSource1.Play();
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.Stagger.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Stagger))
                buffsAndDebuffs.SendMessage("staggerActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "stagger");
            GlobalVariables.addToWhomstJustGainedDebuff(this, "stagger");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Templar Pikeman / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void TempPikeBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Templar Priest / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void TempPriestBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Templar Champion / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void TempChampBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Templar Commander / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void TempCommBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand && !abilityBlockImmunity)
        {
            buffsAndDebuffs.AbilityBlock.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.AbilityBlock))
                buffsAndDebuffs.SendMessage("abilityBlockActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "abilityBlock");
            GlobalVariables.addToWhomstJustGainedDebuff(this, "abilityBlock");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void TempCommSpecial1(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        audioSource1.clip = enemy.GetComponent<Character>().blockSound;
        audioSource1.Play();
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.Stagger.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Stagger))
                buffsAndDebuffs.SendMessage("staggerActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "stagger");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
        if (Random.Range(0, 100) < chanceToLand && !stunImmunity)
        {
            buffsAndDebuffs.Stun.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Stun))
                buffsAndDebuffs.SendMessage("stunActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "stun");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void TempCommSpecial2(Character enemy)
    {
        damage = (int)(enemy.maxHealth * .75f);
        //GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "true"));
    }
    #endregion
    #region Wolfpack
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Executioner / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void ExecutionerBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f) * .6f);
        if (enemy.abilityBlock && !enemy.buffsAndDebuffs.DebuffList.Contains(enemy.buffsAndDebuffs.AbilityBlock))
        {
            damage = (int)((enemy.DamageAmount) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f) * .6f);
            enemy.abilityBlock = false;
            enemy.TauntIgnore = false;
        }
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        float chance = 0.2f;
        if (health == maxHealth)
            chance *= 2;
        if (Random.Range(0, 100) < chanceToLand * chance)
        {
            buffsAndDebuffs.Blind.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Blind))
                buffsAndDebuffs.SendMessage("blindActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "blind");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
        if (Random.Range(0, 100) < chanceToLand * chance)
        {
            buffsAndDebuffs.Stagger.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Stagger))
                buffsAndDebuffs.SendMessage("staggerActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "stagger");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void ExecutionerSpecial1(Character enemy)
    {
        if (!turnMeterDownImmunity)
        {
            turnMeter -= 50;
            if (turnMeter < 0)
                turnMeter = 0;
        }
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.DefenseDown.add(2);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.DefenseDown))
                buffsAndDebuffs.SendMessage("defenseDownActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "defenseDown");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.OffenseDown.add(2);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.OffenseDown))
                buffsAndDebuffs.SendMessage("offenseDownActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "offenseDown");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void ExecutionerSpecial2(Character enemy)
    {
        GlobalSounds.playSlash(audioSource1);
        if (health > maxHealth * .25f)
        {
            damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
            StartCoroutine(hit(damage, enemy, "physical"));
        }
        else
        {
            StartCoroutine(ded());
            GlobalVariables.addToWhomstJustKilled(enemy);
        }
    }
    public void ExecutionerPassive(Character enemy)
    {
        damage = (int)(maxHealth * .25f);
        StartCoroutine(hit(damage, enemy, "silent"));
    }
    public void ExecutionerPassive2(Character enemy)
    {
        damage = (int)(maxHealth * .5f);
        StartCoroutine(hit(damage, enemy, "silent"));
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Bloodletter / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void BloodletterBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        if (enemy.abilityBlock && !enemy.buffsAndDebuffs.DebuffList.Contains(enemy.buffsAndDebuffs.AbilityBlock))
        {
            damage = (int)((enemy.DamageAmount) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
            enemy.abilityBlock = false;
            enemy.TauntIgnore = false;
        }
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand && !nameOfChar.Contains("PlagueDoctor"))
        {
            buffsAndDebuffs.Bleed.burnAdd(1);
            if (!buffsAndDebuffs.BurnList.Contains(buffsAndDebuffs.Bleed))
                buffsAndDebuffs.SendMessage("bleedActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "bleed");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void BloodletterSpecial2(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f) * .4f);
        GlobalSounds.playSlash(audioSource1);
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        for (int i = 0; i < 3; i++) {
            StartCoroutine(hit(damage, enemy, "physical"));
            if (Random.Range(0, 100) < chanceToLand && !nameOfChar.Contains("PlagueDoctor"))
            {
                buffsAndDebuffs.Bleed.burnAdd(1);
                if (!buffsAndDebuffs.BurnList.Contains(buffsAndDebuffs.Bleed))
                    buffsAndDebuffs.SendMessage("bleedActivate");
                GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "bleed");
            }
            else
            {
                GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
                string[] bonk = { "White", "me" };
                damageNombers(bonk);
            }
        }
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Wolfpack Archer / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void WolfArchBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        if (enemy.abilityBlock && !enemy.buffsAndDebuffs.DebuffList.Contains(enemy.buffsAndDebuffs.AbilityBlock))
        {
            damage = (int)((enemy.DamageAmount) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
            enemy.abilityBlock = false;
            enemy.TauntIgnore = false;
        }
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.DefenseDown.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.DefenseDown))
                buffsAndDebuffs.SendMessage("defenseDownActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "defenseDown");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void WolfArchSpecial2(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.Burning.burnAdd(3);
            if (!buffsAndDebuffs.BurnList.Contains(buffsAndDebuffs.Burning))
                buffsAndDebuffs.SendMessage("burningActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "burning");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }

    // / / / / / / / / / / / / / / / / / / / / / / / / / / Cursed Guardian / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void CGuardBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        if (enemy.abilityBlock && !enemy.buffsAndDebuffs.DebuffList.Contains(enemy.buffsAndDebuffs.AbilityBlock))
        {
            damage = (int)((enemy.DamageAmount) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
            enemy.abilityBlock = false;
            enemy.TauntIgnore = false;
        }
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
    }
    public void CGuardSpecial2(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - MagResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "magical"));
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Wolfmother / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void WolfmotherBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f) / 2);
        if (enemy.abilityBlock && !enemy.buffsAndDebuffs.DebuffList.Contains(enemy.buffsAndDebuffs.AbilityBlock))
        {
            damage = (int)((enemy.DamageAmount) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f) / 2);
            enemy.abilityBlock = false;
            enemy.TauntIgnore = false;
        }
        if (isLeader)
        {
            damage *= 2;
        }
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.Blind.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Blind))
                buffsAndDebuffs.SendMessage("blindActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "blind");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Caesar / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void CaesarBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        if (enemy.abilityBlock && !enemy.buffsAndDebuffs.DebuffList.Contains(enemy.buffsAndDebuffs.AbilityBlock))
        {
            damage = (int)((enemy.DamageAmount) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
            enemy.abilityBlock = false;
            enemy.TauntIgnore = false;
        }
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand * .7f && !nameOfChar.Contains("PlagueDoctor"))
        {
            buffsAndDebuffs.Bleed.burnAdd(1);
            if (!buffsAndDebuffs.BurnList.Contains(buffsAndDebuffs.Bleed))
                buffsAndDebuffs.SendMessage("bleedActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "bleed");
            GlobalVariables.addToWhomstJustGainedDebuff(this, "bleed");
        }
        if (Random.Range(0, 100) < chanceToLand * .7f && !nameOfChar.Contains("PlagueDoctor"))
        {
            buffsAndDebuffs.Bleed.burnAdd(1);
            if (!buffsAndDebuffs.BurnList.Contains(buffsAndDebuffs.Bleed))
                buffsAndDebuffs.SendMessage("bleedActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "bleed");
            GlobalVariables.addToWhomstJustGainedDebuff(this, "bleed");
        }
    }
    public void CaesarSpecialPunch(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if (!stunImmunity)
        {
            buffsAndDebuffs.Stun.burnAdd(1);
            if (!buffsAndDebuffs.BurnList.Contains(buffsAndDebuffs.Stun))
                buffsAndDebuffs.SendMessage("stunActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "stun");
        }
            buffsAndDebuffs.Stagger.burnAdd(1);
            if (!buffsAndDebuffs.BurnList.Contains(buffsAndDebuffs.Stagger))
                buffsAndDebuffs.SendMessage("staggerActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "stagger");
    }
    #endregion
    #region Shaubeny
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Shaubeny / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void ShaubenyBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
    }
    public void ShaubenySpecial1(Character enemy)
    {
        buffsAndDebuffs.Trial.add(2);
        if (!buffsAndDebuffs.BlueBuffList.Contains(buffsAndDebuffs.Trial))
            buffsAndDebuffs.SendMessage("trialActivate");
    }
    public void ShaubenySpecial2(Character enemy)
    {
        GlobalSounds.playSlash(audioSource1);
        if (!buffsAndDebuffs.BlueBuffList.Contains(buffsAndDebuffs.Trial))
        {
            damage = (int)(maxHealth * .3f);
            StartCoroutine(hit(damage, enemy, "physical"));
        }
        else
        {
            StartCoroutine(ded());
            GlobalVariables.addToWhomstJustKilled(enemy);
        }
    }
    public void Plead()
    {
        StartCoroutine(AbilityTimer());
        if (gameObject.name.Contains("Enemy"))
        {
            buffsAndDebuffs.SendMessage("trialDeactivate");
            buffsAndDebuffs.BlueBuffList.Remove(buffsAndDebuffs.Trial);
            buffsAndDebuffs.ImageArray();
            for (int i = 0; i < GlobalVariables.friendlyArray.Length; i++)
            {
                if (!GlobalVariables.friendlyArray[i].GetComponent<Character>().buffImmunity)
                {
                    GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.OffenseUp.add(3);
                    if (!GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.OffenseUp))
                        GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.SendMessage("offenseUpActivate");
                    GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.DefenseUp.add(3);
                    if (!GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.DefenseUp))
                        GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.SendMessage("defenseUpActivate");
                    GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.HealthStealUp.add(3);
                    if (!GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.HealthStealUp))
                        GlobalVariables.friendlyArray[i].GetComponent<Character>().buffsAndDebuffs.SendMessage("healthStealUpActivate");
                    GlobalVariables.friendlyArray[i].GetComponent<Character>().abilityUpdate();
                }
            }
        }
        else
        {
            buffsAndDebuffs.SendMessage("trialDeactivate");
            buffsAndDebuffs.BlueBuffList.Remove(buffsAndDebuffs.Trial);
            buffsAndDebuffs.ImageArray();
            GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.OffenseUp.add(3);
            if (!GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(buffsAndDebuffs.OffenseUp))
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.SendMessage("offenseUpActivate");
            GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.DefenseUp.add(3);
            if (!GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(buffsAndDebuffs.DefenseUp))
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.SendMessage("defenseUpActivate");
            GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.HealthStealUp.add(3);
            if (!GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(buffsAndDebuffs.HealthStealUp))
                GlobalVariables.enemiesSelected[0].GetComponent<Character>().buffsAndDebuffs.SendMessage("healthStealUpActivate");
            GlobalVariables.enemiesSelected[0].GetComponent<Character>().abilityUpdate();
            for (int i = 0; i < GlobalVariables.hostileArray.Length; i++)
            {
                if (!GlobalVariables.hostileArray[i].GetComponent<Character>().buffImmunity)
                {
                    GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.OffenseUp.add(3);
                    if (!GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(buffsAndDebuffs.OffenseUp))
                        GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.SendMessage("offenseUpActivate");
                    GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.DefenseUp.add(3);
                    if (!GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(buffsAndDebuffs.DefenseUp))
                        GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.SendMessage("defenseUpActivate");
                    GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.HealthStealUp.add(3);
                    if (!GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.BuffList.Contains(buffsAndDebuffs.HealthStealUp))
                        GlobalVariables.hostileArray[i].GetComponent<Character>().buffsAndDebuffs.SendMessage("healthStealUpActivate");
                    GlobalVariables.hostileArray[i].GetComponent<Character>().abilityUpdate();
                }
            }
        }
    }
    IEnumerator AbilityTimer()
    {
        turnMeter = 0;
        yield return new WaitForSeconds(.4f);
        GlobalVariables.TakingTurn = false;
        buffsAndDebuffs.SendMessage("EndOfTurn");
        turnUpdate = true;
    }
    #endregion
    #region Cultists
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Fanatic / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void FanaticBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
    }
    public void FanaticSpecial1(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        buffsAndDebuffs.Burning.burnAdd(3);
        if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Burning))
            buffsAndDebuffs.SendMessage("burningActivate");
        GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "burning");
        GlobalVariables.addToWhomstJustGainedDebuff(this, "burning");
    }
    public void FanaticSpecial1Armuk(Character enemy)
    {
        buffsAndDebuffs.EvasionDown.burnAdd(3);
        if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.EvasionDown))
            buffsAndDebuffs.SendMessage("evasionDownActivate");
        GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "evasionDown");
        GlobalVariables.addToWhomstJustGainedDebuff(this, "evasionDown");
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Fanatic's Warchief / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void WarchiefBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        StartCoroutine(hit((int)(maxHealth * .05f), enemy, "true"));
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Fanatic's Creation / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void FCreationBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand && Random.Range(0, 100) < 100)
        {
            buffsAndDebuffs.Burning.burnAdd(1);
            if (!buffsAndDebuffs.BurnList.Contains(buffsAndDebuffs.Burning))
                buffsAndDebuffs.SendMessage("burningActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "burning");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Corrupt Cultist / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void CorruptCultistBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - MagResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "magical"));
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Cultist Caster / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void CasterBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - MagResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "magical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.OffenseDown.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.OffenseDown))
                buffsAndDebuffs.SendMessage("offenseDownActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "offenseDown");
            GlobalVariables.addToWhomstJustGainedDebuff(this, "offenseDown");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void CasterSpecial1Target(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - MagResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "magical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand && buffsAndDebuffs.Corruption.getAmount() < 5 && !nameOfChar.Contains("PlagueDoctor"))
        {
            buffsAndDebuffs.Corruption.burnAdd(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Corruption))
                buffsAndDebuffs.SendMessage("corruptionActivate");
            else
                buffsAndDebuffs.SendMessage("corruptionAdd");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "corruption");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Cultist Summoner / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void SummonerBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - MagResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "magical"));
    }
    public void SummonerSpecial1(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - MagResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "magical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (buffsAndDebuffs.DebuffList.Count > 0)
        {
            if (Random.Range(0, 100) < chanceToLand && buffsAndDebuffs.Corruption.getAmount() < 5 && !nameOfChar.Contains("PlagueDoctor"))
            {
                buffsAndDebuffs.Corruption.burnAdd(1);
                if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Corruption))
                    buffsAndDebuffs.SendMessage("corruptionActivate");
                else
                    buffsAndDebuffs.SendMessage("corruptionAdd");
                GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "corruption");
            }
            else
            {
                GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
                string[] bonk = { "White", "me" };
                damageNombers(bonk);
            }
        }
        if (buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Corruption))
        {
            Character target = this;
            if (gameObject.name.Contains("Enemy"))
            {
                for (int i = 0; i < GlobalVariables.friendlyArray.Length; i++)
                {
                    if (GlobalVariables.friendlyArray[i].GetComponent<Character>().health / GlobalVariables.friendlyArray[i].GetComponent<Character>().maxHealth > target.health / target.maxHealth)
                        target = GlobalVariables.friendlyArray[i].GetComponent<Character>();
                }
            }
            else
            {
                for (int i = 0; i < GlobalVariables.hostileArray.Length; i++)
                {
                    if (GlobalVariables.hostileArray[i].GetComponent<Character>().health / GlobalVariables.hostileArray[i].GetComponent<Character>().maxHealth > target.health / target.maxHealth)
                        target = GlobalVariables.hostileArray[i].GetComponent<Character>();
                }
            }
            if ((target.Tenacity - enemy.GetComponent<Character>().Potency) > 15)
                chanceToLand = 100 - target.Tenacity + enemy.GetComponent<Character>().Potency;
            else
                chanceToLand = 85;
            if (target.buffsAndDebuffs.DebuffList.Count > 0)
            {
                if (Random.Range(0, 100) < chanceToLand && buffsAndDebuffs.Corruption.getAmount() < 5 && !nameOfChar.Contains("PlagueDoctor"))
                {
                    target.buffsAndDebuffs.Corruption.burnAdd(1);
                    if (!target.buffsAndDebuffs.DebuffList.Contains(target.buffsAndDebuffs.Corruption))
                        target.buffsAndDebuffs.SendMessage("corruptionActivate");
                    else
                        target.buffsAndDebuffs.SendMessage("corruptionAdd");
                    GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "corruption");
                }
                else
                {
                    GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
                    string[] bonk = { "White", "me" };
                    target.damageNombers(bonk);
                }
            }
        }
    }
    #endregion
    #region Plague Doctor
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Plague Doctor / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void PlagueDoctorBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        if (tags.Contains("Nightmare")) damage *= 2;
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.PotencyDown.add(2);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.PotencyDown))
                buffsAndDebuffs.SendMessage("potencyDownActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "potencyDown");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }

        Character target = this;
        if (gameObject.name.Contains("Enemy"))
        {
            if (GlobalVariables.hostileArray.Length > 0)
            target = GlobalVariables.hostileArray[Random.Range(0, GlobalVariables.hostileArray.Length - 1)].GetComponent<Character>();            
        }
        else
        {
            if (GlobalVariables.friendlyArray.Length > 0)
                target = GlobalVariables.friendlyArray[Random.Range(0, GlobalVariables.friendlyArray.Length - 1)].GetComponent<Character>();
        }
        if (target != null)
        {
            if ((target.Tenacity - enemy.GetComponent<Character>().Potency) > 15)
                chanceToLand = 100 - target.Tenacity + enemy.GetComponent<Character>().Potency;
            else
                chanceToLand = 85;
            if (Random.Range(0, 100) < chanceToLand)
            {
                target.buffsAndDebuffs.PotencyDown.add(2);
                if (!target.buffsAndDebuffs.DebuffList.Contains(target.buffsAndDebuffs.PotencyDown))
                    target.buffsAndDebuffs.SendMessage("potencyDownActivate");
                GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "potencyDown");
            }
            else
            {
                GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
                string[] bonk = { "White", "me" };
                target.damageNombers(bonk);
            }
            target.buffsAndDebuffs.ImageArray();
        }
        buffsAndDebuffs.ImageArray();
    }
    public void PlagueDoctorSpecial1(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - MagResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "magical"));
    }
    public void PlagueDoctorSpecial2a(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - MagResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "magical"));
    }
    public void PlagueDoctorSpecial2b(Character enemy)
    {
        damage = (int)((enemy.DamageAmount / 2) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f) * .025f);
        StartCoroutine(hit(damage, enemy, "true"));
    }
    #endregion
    #region Resistance
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Foppish Knight / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void FoppishBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playMetalOnFlesh(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.Vulnerable.add(2);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Vulnerable))
                buffsAndDebuffs.SendMessage("vulnerableActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "vulnerable");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void FoppishSpecial2(Character enemy)
    {
        if (!turnMeterDownImmunity)
            turnMeter -= 30;
        if (turnMeter < 0)
            turnMeter = 0;
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand && !abilityBlockImmunity)
        {
            buffsAndDebuffs.AbilityBlock.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.AbilityBlock))
                buffsAndDebuffs.SendMessage("abilityBlockActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "abilityBlock");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Jester Knight / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void JesterBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playMetalOnFlesh(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.Daze.add(2);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Daze))
                buffsAndDebuffs.SendMessage("dazeActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "daze");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void JesterSpecial1(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playMetalOnFlesh(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand && !stunImmunity)
        {
            buffsAndDebuffs.Stun.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Stun))
                buffsAndDebuffs.SendMessage("stunActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "stun");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void JesterSpecial1b(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
    }
    public void JesterSpecial2(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.SpeedDown.add(2);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.SpeedDown))
                buffsAndDebuffs.SendMessage("speedDownActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "speedDown");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    #endregion
    #region Black Legion
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Dark Templar / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void DarkTemplarBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - MagResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "magical"));
    }
    public void DarkTemplarSpecial1(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - MagResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "magical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.Daze.add(2);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Daze))
                buffsAndDebuffs.SendMessage("dazeActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "daze");
            GlobalVariables.addToWhomstJustGainedDebuff(this, "daze");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.Disrupted.add(2);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Disrupted))
                buffsAndDebuffs.SendMessage("disruptedActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "disrupted");
            GlobalVariables.addToWhomstJustGainedDebuff(this, "disrupted");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void DarkTemplarSpecial2(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - MagResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "magical"));
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Dark Praetorian / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void PraetorianBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Random.Range(0, 100) < 50) && !turnMeterDownImmunity)
            turnMeter -= 50;
    }
    public void PraetorianSpecial2(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - MagResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "magical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand && Random.Range(0, 100) < 30 && !stunImmunity)
        {
            buffsAndDebuffs.Stun.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Stun))
                buffsAndDebuffs.SendMessage("stunActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "stun");
            GlobalVariables.addToWhomstJustGainedDebuff(this, "stun");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.Stagger.add(2);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Stagger))
                buffsAndDebuffs.SendMessage("staggerActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "stagger");
            GlobalVariables.addToWhomstJustGainedDebuff(this, "stagger");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Dark Spartan / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void DarkSpartanBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f) / 2);
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        StartCoroutine(hit(damage, enemy, "physical"));
    }
    public void DarkSpartanSpecial2(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - MagResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "magical"));
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Dark Sentinel / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void SentinelBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
    }
    public void SentinelSpecial2(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - MagResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "magical"));
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / The Red Hand / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void RedHandBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
    }
    // / / / / / / / / / / / / / / / / / / / / / / / / / / Phantom / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void PhantomBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand && Random.Range(0, 100) < 31)
        {
            buffsAndDebuffs.Daze.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Daze))
                buffsAndDebuffs.SendMessage("dazeActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "daze");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
        if (Random.Range(0, 100) < chanceToLand && !stunImmunity && Random.Range(0, 100) < 31)
        {
            buffsAndDebuffs.Stun.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Stun))
                buffsAndDebuffs.SendMessage("stunActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "stun");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
        if (Random.Range(0, 100) < chanceToLand && !nameOfChar.Contains("PlagueDoctor") && Random.Range(0, 100) < 31)
        {
            buffsAndDebuffs.Bleed.burnAdd(1);
            if (!buffsAndDebuffs.BurnList.Contains(buffsAndDebuffs.Bleed))
                buffsAndDebuffs.SendMessage("bleedActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "bleed");
            GlobalVariables.addToWhomstJustGainedDebuff(this, "bleed");
        }
    }
    public void PhantomSpecial1(Character enemy)
    {
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        for (int i = 0; i < 2; i++)
        {
            if (Random.Range(0, 100) < chanceToLand)
            {
                buffsAndDebuffs.Vulnerable.burnAdd(1);
                if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Vulnerable))
                    buffsAndDebuffs.SendMessage("vulnerableActivate");
                GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "vulnerable");
            }
            else
            {
                GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
                string[] bonk = { "White", "me" };
                damageNombers(bonk);
            }
        }
    }
    public void SuperionSpecial1(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - MagResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "magical"));
    }
    public void SuperionSpecial2(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - MagResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "magical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.Disrupted.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Disrupted))
                buffsAndDebuffs.SendMessage("disruptedActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "disrupted");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    #endregion
    #region Monsters
    public void GPeonBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand && Random.Range(0, 100) < 30)
        {
            buffsAndDebuffs.Stagger.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Stagger))
                buffsAndDebuffs.SendMessage("staggerActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "stagger");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void GPeonSpecial1(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f)/3);
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        StartCoroutine(hit(damage, enemy, "physical"));
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.OffenseDown.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.OffenseDown))
                buffsAndDebuffs.SendMessage("offenseDownActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "offenseDown");
            GlobalVariables.addToWhomstJustGainedDebuff(this, "offenseDown");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }

    public void GSpiderBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand && !nameOfChar.Contains("PlagueDoctor"))
        {
            buffsAndDebuffs.Poison.burnAdd(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Poison))
                buffsAndDebuffs.SendMessage("poisonActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "poison");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void GSpiderSpecial1(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f) / 3);
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand && !abilityBlockImmunity)
        {
            buffsAndDebuffs.AbilityBlock.add(3);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.AbilityBlock))
                buffsAndDebuffs.SendMessage("abilityBlockActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "abilityBlock");
        }
        if (Random.Range(0, 100) < chanceToLand && !stunImmunity)
        {
            buffsAndDebuffs.Stun.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Stun))
                buffsAndDebuffs.SendMessage("stunActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "stun");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void VarusSpecial1(Character enemy)
    {
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.Stagger.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Stagger))
                buffsAndDebuffs.SendMessage("staggerActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "stagger");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.Daze.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Daze))
                buffsAndDebuffs.SendMessage("dazeActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "daze");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void OgreSpecial1(Character enemy)
    {
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand && Random.Range(0, 100) < 50)
        {
            buffsAndDebuffs.ResilienceDown.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.ResilienceDown))
                buffsAndDebuffs.SendMessage("resilienceDownActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "resilienceDown");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void OgreSpecial2(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand && Random.Range(0, 100) < 50)
        {
            buffsAndDebuffs.Stagger.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Stagger))
                buffsAndDebuffs.SendMessage("staggerActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "stagger");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void GBruteBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.Daze.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Daze))
                buffsAndDebuffs.SendMessage("dazeActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "daze");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void GBruteSpecial1(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand && Random.Range(0, 100) < 35)
        {
            buffsAndDebuffs.Stagger.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Stagger))
                buffsAndDebuffs.SendMessage("staggerActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "stagger");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    #endregion
    #region Virion
    public void VirionBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f) * .6f);
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.BuffImmunity.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.BuffImmunity))
                buffsAndDebuffs.SendMessage("buffImmunityActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "buffImmunity");
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void VirionCritBasic(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f) * .6f);
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        if (Random.Range(0, 100) < chanceToLand)
        {
            buffsAndDebuffs.BuffImmunity.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.BuffImmunity))
                buffsAndDebuffs.SendMessage("buffImmunityActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "buffImmunity");
            VirionPassive2(enemy);
        }
        else
        {
            GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
            string[] bonk = { "White", "me" };
            damageNombers(bonk);
        }
    }
    public void VirionSpecial1(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - MagResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "magical"));
        buffsAndDebuffs.Shocked.burnAdd(2);
        if (!buffsAndDebuffs.BurnList.Contains(buffsAndDebuffs.Shocked))
            buffsAndDebuffs.SendMessage("shockedActivate");
        GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "shocked");
        buffsAndDebuffs.Broken.add(1);
            if (!buffsAndDebuffs.BlueBuffList.Contains(buffsAndDebuffs.Broken))
                buffsAndDebuffs.SendMessage("brokenActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "broken");
    }
    public void VirionSpecial2(Character enemy)
    {
        damage = (int)(maxHealth);
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "true"));
        buffsAndDebuffs.Broken.add(1);
        if (!buffsAndDebuffs.BlueBuffList.Contains(buffsAndDebuffs.Broken))
            buffsAndDebuffs.SendMessage("brokenActivate");
        GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "broken");
    }
    public void VirionPassive2(Character enemy)
    {
        int i = 1;
        if (tags.Contains("Leader"))
            i++;
        if (tags.Contains("Attacker"))
            offense = (int)(offense * (1 - .1f * i));
        if (tags.Contains("Support"))
        {
            speed -= 10 * i;
            if (speed <= 0)
                speed = 1;
        }
        if (tags.Contains("Tank"))
        {
            maxProtection = (int)(maxProtection * (1 - .15f * i));
            if (protection > maxProtection)
                protection = maxProtection;
        }
    }
    #endregion
    #region Northmen 
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / / / Berserkir / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void BerserkirSpecial1(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if ((Tenacity - enemy.GetComponent<Character>().Potency) > 15)
            chanceToLand = 100 - Tenacity + enemy.GetComponent<Character>().Potency;
        else
            chanceToLand = 85;
        //the bleed
        for (int i = 0; i < 2; i++)
        {
            if (Random.Range(0, 100) < chanceToLand)
            {
                buffsAndDebuffs.Freezing.burnAdd(1);
                if (!buffsAndDebuffs.BurnList.Contains(buffsAndDebuffs.Freezing))
                    buffsAndDebuffs.SendMessage("freezingActivate");
                GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "freezing");
            }
            else
            {
                GlobalVariables.DamageNumberWhite.GetComponent<TextMeshProUGUI>().text = "Resisted!";
                string[] bonk = { "White", "me" };
                damageNombers(bonk);
            }
        }
    }
    public void BerserkirSpecial2(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
        if (buffsAndDebuffs.BurnList.Contains(buffsAndDebuffs.Freezing))
        {
            buffsAndDebuffs.Frostbite.add(1);
            if (!buffsAndDebuffs.DebuffList.Contains(buffsAndDebuffs.Frostbite))
                buffsAndDebuffs.SendMessage("frostbiteActivate");
            GlobalVariables.addToWhomstJustInflictedDebuff(enemy, "frostbite");
        }
    }
    #endregion
    // / / / / / / / / / / / / / / / / / / / / / / / / / / / / / Other / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /
    public void BurnedToDeath_SkullEmoji_()
    {
        StartCoroutine(hit(0, null, "true"));
    }
    public void SimplePhysical(Character enemy)
    {
        damage = (int)((enemy.DamageAmount * (100 - PhysResist) / 100f) * (1 + Mathf.Log10((float)enemy.offense / defense) / 2.5f));
        GlobalSounds.playSlash(audioSource1);
        StartCoroutine(hit(damage, enemy, "physical"));
    }
    public int elementDamage(string element, List<string> enemyTags)
    {
        switch (element)
        {
            case "Metal":
                if (enemyTags.Contains("Nightmare") || enemyTags.Contains("Magic"))
                    DamageAmount = (int)(DamageAmount * 1.15f);
                if (enemyTags.Contains("Life") || enemyTags.Contains("Ice") || enemyTags.Contains("Lightning"))
                    DamageAmount = (int)(DamageAmount * .9f);
                break;
            case "Magic":
                if (enemyTags.Contains("Earth") || enemyTags.Contains("Lightning"))
                    DamageAmount = (int)(DamageAmount * 1.15f);
                if (enemyTags.Contains("Metal") || enemyTags.Contains("Earth") || enemyTags.Contains("Nightmare"))
                    DamageAmount = (int)(DamageAmount * .9f);
                break;
            case "Fire":
                if (enemyTags.Contains("Ice") || enemyTags.Contains("Life"))
                    DamageAmount = (int)(DamageAmount * 1.15f);
                if (enemyTags.Contains("Nightmare") || enemyTags.Contains("Ice") || enemyTags.Contains("Metal"))
                    DamageAmount = (int)(DamageAmount * .9f);
                break;
            case "Ice":
                if (enemyTags.Contains("Fire") || enemyTags.Contains("Metal"))
                    DamageAmount = (int)(DamageAmount * 1.15f);
                if (enemyTags.Contains("Fire") || enemyTags.Contains("Earth") || enemyTags.Contains("Lightning"))
                    DamageAmount = (int)(DamageAmount * .9f);
                break;
            case "Earth":
                if (enemyTags.Contains("Lightning") || enemyTags.Contains("Magic"))
                    DamageAmount = (int)(DamageAmount * 1.15f);
                if (enemyTags.Contains("Life") || enemyTags.Contains("Ice") || enemyTags.Contains("Magic"))
                    DamageAmount = (int)(DamageAmount * .9f);
                break;
            case "Lightning":
                if (enemyTags.Contains("Ice") || enemyTags.Contains("Metal"))
                    DamageAmount = (int)(DamageAmount * 1.15f);
                if (enemyTags.Contains("Fire") || enemyTags.Contains("Earth") || enemyTags.Contains("Magic"))
                    DamageAmount = (int)(DamageAmount * .9f);
                break;
            case "Life":
                if (enemyTags.Contains("Nightmare") || enemyTags.Contains("Earth"))
                    DamageAmount = (int)(DamageAmount * 1.15f);
                if (enemyTags.Contains("Nightmare") || enemyTags.Contains("Fire") || enemyTags.Contains("Lightning"))
                    DamageAmount = (int)(DamageAmount * .9f);
                break;
            case "Nightmare":
                if (enemyTags.Contains("Life") || enemyTags.Contains("Fire"))
                    DamageAmount = (int)(DamageAmount * 1.15f);
                if (enemyTags.Contains("Life") || enemyTags.Contains("Metal") || enemyTags.Contains("Magic"))
                    DamageAmount = (int)(DamageAmount * .9f);
                break;
        }
        return 5;
    }
}
