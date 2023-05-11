using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class GlobalVariables : MonoBehaviour
{
    public static bool TakingTurn;
    public bool tt;
    public Camera normalCamera;
    public Camera cinematicCamera;
    public GameObject InvertedSphere;
    public static Camera nCamera;
    public static Camera cCamera;
    public static GameObject iSphere;
    PostProcessVolume m_Volume;
    Vignette m_Vignette;
    public static PostProcessVolume mVolume;
    public static Vignette mVignette;
    public static GameObject[] playerArray;
    public static GameObject[] enemyArray;
    public static GameObject[] friendlyArray;
    public static GameObject[] hostileArray;
    public static GameObject[] enemiesSelected;
    public static GameObject[] playersSelected; 
    public static string[] allies;
    public static string[] enemies;
    public static string[] Leaders = new string[5];
    public static string[] EnemyLeaders = new string[5];
    public static Button[] buttons;
    public TextMeshProUGUI DamageNumber1;
    public TextMeshProUGUI DamageNumber2;
    public TextMeshProUGUI DamageNumber3;
    public TextMeshProUGUI DamageNumber4;
    public GameObject DamageNumbrs0;
    public GameObject DamageNumbrs1;
    public GameObject DamageNumbrs2;
    public GameObject DamageNumbrs3;
    public GameObject RedTxtCamera;
    public GameObject WhiteTxtCamera;
    public GameObject GreenTxtCamera;
    public GameObject BlueTxtCamera;
    public static TextMeshProUGUI DamageNumberRed;
    public static TextMeshProUGUI DamageNumberWhite;
    public static TextMeshProUGUI DamageNumberGreen;
    public static TextMeshProUGUI DamageNumberBlue;
    public static GameObject DamageNumbers;
    public static GameObject DamageNumbers1;
    public static GameObject DamageNumbers2;
    public static GameObject DamageNumbers3;
    public static GameObject RedTextCamera;
    public static GameObject WhiteTextCamera;
    public static GameObject GreenTextCamera;
    public static GameObject BlueTextCamera;
    public static bool allySelectable;
    public bool aS;
    public static bool countering;
    public bool ct;
    public static int numCounters;
    public static int assisters = 0;
    // Health Bars :(
    public Sprite h1;
    public Sprite h2;
    public Sprite h3;
    public Sprite h4;
    public Sprite h5;
    public Sprite h6;
    public Sprite h7;
    public Sprite h8;
    public Sprite h9;
    public Sprite h10;
    public Sprite h11;
    public Sprite h12;
    public Sprite h13;
    public static Sprite health1;
    public static Sprite health2;
    public static Sprite health3;
    public static Sprite health4;
    public static Sprite health5;
    public static Sprite health6;
    public static Sprite health7;
    public static Sprite health8;
    public static Sprite health9;
    public static Sprite health10;
    public static Sprite health11;
    public static Sprite health12;
    public static Sprite health13;
    public static bool allStealthed;
    public static int EnemyTaunters;
    public static int AllyTaunters;
    public static int EnemyStealthers;
    public static int AllyStealthers;
    public static bool SummonSlot = false;
    // Start is called before the first frame update
    static public GlobalVariables instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        TakingTurn = true;
        StartCoroutine(takingTurnDelay());

        Leaders[0] = "";
        EnemyLeaders[0] = "";
        allySelectable = false;
        playerArray = GameObject.FindGameObjectsWithTag("AllyTurn");
        enemyArray = GameObject.FindGameObjectsWithTag("EnemyTurn");
        friendlyArray = GameObject.FindGameObjectsWithTag("Ally");
        hostileArray = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesSelected = GameObject.FindGameObjectsWithTag("EnemySelected");
        playersSelected = GameObject.FindGameObjectsWithTag("PlayerSelected");
        DamageNumberRed = DamageNumber1;
        DamageNumberWhite = DamageNumber2;
        DamageNumberGreen = DamageNumber3;
        DamageNumberBlue = DamageNumber4;
        DamageNumbers = DamageNumbrs0;
        DamageNumbers1 = DamageNumbrs1;
        DamageNumbers2 = DamageNumbrs2;
        DamageNumbers3 = DamageNumbrs3;
        RedTextCamera = RedTxtCamera;
        WhiteTextCamera = WhiteTxtCamera;
        GreenTextCamera = GreenTxtCamera;
        BlueTextCamera = BlueTxtCamera;
        health1 = h1;
        health2 = h2;
        health3 = h3;
        health4 = h4;
        health5 = h5;
        health6 = h6;
        health7 = h7;
        health8 = h8;
        health9 = h9;
        health10 = h10;
        health11 = h11;
        health12 = h12;
        health13 = h13;

        nCamera = normalCamera;
        cCamera = cinematicCamera;
        iSphere = InvertedSphere;
        mVignette = m_Vignette;
        mVolume = m_Volume;

        m_Vignette = ScriptableObject.CreateInstance<Vignette>();
        m_Vignette.enabled.Override(true);
        m_Vignette.intensity.Override(1f);
        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_Vignette);
    }
    IEnumerator takingTurnDelay()
    {
        yield return new WaitForSeconds(.5f);
        TakingTurn = false;
    }
    // Update is called once per frame
    void Update()
    {
        //character arrays
        playerArray = GameObject.FindGameObjectsWithTag("AllyTurn");
        enemyArray = GameObject.FindGameObjectsWithTag("EnemyTurn");
        friendlyArray = GameObject.FindGameObjectsWithTag("Ally");
        hostileArray = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesSelected = GameObject.FindGameObjectsWithTag("EnemySelected");
        playersSelected = GameObject.FindGameObjectsWithTag("PlayerSelected");
        ct = countering;
        tt = TakingTurn;
        aS = allySelectable;
        if (numCounters > 0)
            countering = true;
        else
            countering = false;
    }
    public static void ClearAllEvents()
    {
        OnUsedBasic = null;
        OnUsedSpecial = null;
        OnStartTurn = null;
        OnSelectedAlly = null;
        OnParry = null;
        OnKilled = null;
        OnInflictedDebuff = null;
        OnInflictedBuff = null;
        OnHit = null;
        OnGotSelectedAlly = null;
        OnGotHit = null;
        OnGotCrit = null;
        OnGainedDebuff = null;
        OnGainedBuff = null;
        OnEndTurn = null;
        OnDodge = null;
        OnDied = null;
        OnCrit = null;
        OnBlock = null;
        OnAttackedOutOfTurn = null;
    }
    // / / / / / / / / / / / / / / / / / / / For when you're checking when people do shit, I guess / / / / / / / / / / / / / / / / / /

    public static List<Character> whomstJustDodged = new List<Character>();
    public delegate void DoEventDodge();
    public static event DoEventDodge OnDodge;
    public static void addToWhomstJustDodged(Character dude)
    {
        whomstJustDodged.Add(dude);
        if (OnDodge != null)
            OnDodge();
        whomstJustDodged.Clear();
    }

    public static List<Character> whomstJustBlocked = new List<Character>();
    public delegate void DoEventBlock();
    public static event DoEventBlock OnBlock;
    public static void addToWhomstJustBlocked(Character dude)
    {
        whomstJustBlocked.Add(dude);
        if (OnBlock != null)
            OnBlock();
        whomstJustBlocked.Clear();
    }

    public static List<Character> whomstJustParried = new List<Character>();
    public delegate void DoEventParry();
    public static event DoEventParry OnParry;
    public static void addToWhomstJustParried(Character dude)
    {
        whomstJustParried.Add(dude);
        if (OnParry != null)
            OnParry();
        whomstJustParried.Clear();
    }

    public static List<Character> whomstJustCrit = new List<Character>();
    public delegate void DoEventCrit();
    public static event DoEventCrit OnCrit;
    public static void addToWhomstJustCrit(Character dude)
    {
        whomstJustCrit.Add(dude);
        if (OnCrit != null)
            OnCrit();
        whomstJustCrit.Clear();
    }

    public static List<Character> whomstJustGotCrit = new List<Character>();
    public delegate void DoEventGotCrit();
    public static event DoEventGotCrit OnGotCrit;
    public static void addToWhomstJustGotCrit(Character dude)
    {
        whomstJustGotCrit.Add(dude);
        if (OnGotCrit != null)
            OnGotCrit();
        whomstJustGotCrit.Clear();
    }

    public static List<Character> whomstJustHit = new List<Character>();
    public delegate void DoEventHit();
    public static event DoEventHit OnHit;
    public static void addToWhomstJustHit(Character dude)
    {
        whomstJustHit.Add(dude);
        if (OnHit != null)
            OnHit();
        whomstJustHit.Clear();
    }

    public static List<Character> whomstJustGotHit = new List<Character>();
    public delegate void DoEventGotHit();
    public static event DoEventGotHit OnGotHit;
    public static void addToWhomstJustGotHit(Character dude)
    {
        whomstJustGotHit.Add(dude);
        if (OnGotHit != null)
            OnGotHit();
        whomstJustGotHit.Clear();
    }

    public static List<Character> whomstJustUsedBasic = new List<Character>();
    public delegate void DoEventUsedBasic();
    public static event DoEventUsedBasic OnUsedBasic;
    public static void addToWhomstJustUsedBasic(Character dude)
    {
        whomstJustUsedBasic.Add(dude);
        if (OnUsedBasic != null)
            OnUsedBasic();
        whomstJustUsedBasic.Clear();
    }

    public static List<Character> whomstJustUsedSpecial = new List<Character>();
    public delegate void DoEventUsedSpecial();
    public static event DoEventUsedSpecial OnUsedSpecial;
    public static void addToWhomstJustUsedSpecial(Character dude)
    {
        whomstJustUsedSpecial.Add(dude);
        if (OnUsedSpecial != null)
            OnUsedSpecial();
        whomstJustUsedSpecial.Clear();
    }

    public static List<Character> whomstJustAttackedOutOfTurn = new List<Character>();
    public delegate void DoEventAttackedOutOfTurn();
    public static event DoEventAttackedOutOfTurn OnAttackedOutOfTurn;
    public static void addToWhomstJustAttackedOutOfTurn(Character dude)
    {
        whomstJustAttackedOutOfTurn.Add(dude);
        if (OnAttackedOutOfTurn != null)
            OnAttackedOutOfTurn();
        whomstJustAttackedOutOfTurn.Clear();
    }

    public static List<Character> whomstJustGainedBuff = new List<Character>();
    public static List<string> buffGained = new List<string>();
    public delegate void DoEventGainedBuff();
    public static event DoEventGainedBuff OnGainedBuff;
    public static void addToWhomstJustGainedBuff(Character dude, string buffname)
    {
        whomstJustGainedBuff.Add(dude); 
        buffGained.Add(buffname);
        if (OnGainedBuff != null)
            OnGainedBuff();
        instance.StartCoroutine("clearGainedBuff");
    }
    IEnumerator clearGainedBuff()
    {
        yield return new WaitForSeconds(.05f);
        whomstJustGainedBuff.Clear();
        buffGained.Clear();
    }

    public static List<Character> whomstJustGainedDebuff = new List<Character>();
    public static List<string> debuffGained = new List<string>();
    public delegate void DoEventGainedDebuff();
    public static event DoEventGainedDebuff OnGainedDebuff;
    public static void addToWhomstJustGainedDebuff(Character dude, string debuffname)
    {
        whomstJustGainedDebuff.Add(dude);
        debuffGained.Add(debuffname);
        if (OnGainedDebuff != null)
            OnGainedDebuff();
        whomstJustGainedDebuff.Clear();
        debuffGained.Clear();
    }

    public static List<Character> whomstJustInflictedBuff = new List<Character>();
    public static List<string> buffInflicted = new List<string>();
    public delegate void DoEventInflictedBuff();
    public static event DoEventInflictedBuff OnInflictedBuff;
    public static void addToWhomstJustInflictedBuff(Character dude, string buffname)
    {
        whomstJustInflictedBuff.Add(dude);
        buffInflicted.Add(buffname);
        if (OnInflictedBuff != null)
            OnInflictedBuff();
        whomstJustInflictedBuff.Clear();
        buffInflicted.Clear();
    }

    public static List<Character> whomstJustInflictedDebuff = new List<Character>();
    public static List<string> debuffinflicted = new List<string>();
    public delegate void DoEventInflictedDebuff();
    public static event DoEventInflictedDebuff OnInflictedDebuff;
    public static void addToWhomstJustInflictedDebuff(Character dude, string debuffname)
    {
        whomstJustInflictedDebuff.Add(dude);
        debuffinflicted.Add(debuffname);
        if (OnInflictedDebuff != null)
            OnInflictedDebuff();
        whomstJustInflictedDebuff.Clear();
        debuffinflicted.Clear();
    }

    public static List<Character> whomstJustDied = new List<Character>();
    public delegate void DoEventDied();
    public static event DoEventDied OnDied;
    public static void addToWhomstJustDied(Character dude)
    {
        whomstJustDied.Add(dude);
        if (OnDied != null)
            OnDied();
        whomstJustDied.Clear();
    }

    public static List<Character> whomstJustKilled = new List<Character>();
    public delegate void DoEventKilled();
    public static event DoEventKilled OnKilled;
    public static void addToWhomstJustKilled(Character dude)
    {
        whomstJustKilled.Add(dude);
        if (OnKilled != null)
            OnKilled();
        whomstJustKilled.Clear();
    }

    public static List<Character> whomstJustSelectedAlly = new List<Character>();
    public delegate void DoEventSelectedAlly();
    public static event DoEventSelectedAlly OnSelectedAlly;
    public static void addToWhomstJustSelectedAlly(Character dude)
    {
        whomstJustSelectedAlly.Add(dude);
        if (OnSelectedAlly != null)
            OnSelectedAlly();
        whomstJustSelectedAlly.Clear();
    }

    public static List<Character> whomstJustGotSelectedAlly = new List<Character>();
    public delegate void DoEventGotSelectedAlly();
    public static event DoEventGotSelectedAlly OnGotSelectedAlly;
    public static void addToWhomstJustGotSelectedAlly(Character dude)
    {
        whomstJustGotSelectedAlly.Add(dude);
        if (OnGotSelectedAlly != null)
            OnGotSelectedAlly();
        whomstJustGotSelectedAlly.Clear();
    }

    public static List<Character> whomstJustStartedTurn = new List<Character>();
    public delegate void DoEventStartTurn();
    public static event DoEventStartTurn OnStartTurn;
    public static void addToWhomstJustStartedTurn(Character dude)
    {
        whomstJustStartedTurn.Add(dude);
        if (OnStartTurn != null)
            OnStartTurn();
        whomstJustStartedTurn.Clear();
    }

    public static List<Character> whomstJustEndedTurn = new List<Character>();
    public delegate void DoEventEndTurn();
    public static event DoEventEndTurn OnEndTurn;
    public static void addToWhomstJustEndedTurn(Character dude)
    {
        whomstJustEndedTurn.Add(dude);
        if (OnEndTurn != null)
            OnEndTurn();
        whomstJustEndedTurn.Clear();
    }
}
