using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSounds : MonoBehaviour
{
    // / / / / / / / / / / / / / / / / / / / / / /  Physical Impact Sounds (AUDIOSOURCE 1) / / / / / / / / / / / / / / / / / / / / /
    //slashes and stabs
    public AudioClip slash1;
    public static AudioClip Slash1;
    public AudioClip stab1;
    public static AudioClip Stab1;
    public AudioClip stab2;
    public static AudioClip Stab2;
    public AudioClip stab3;
    public static AudioClip Stab3;
    public AudioClip stabIn;
    public static AudioClip StabIn;
    public AudioClip stabOut;
    public static AudioClip StabOut;
    public AudioClip quickStab1;
    public static AudioClip QuickStab1;
    //metal clangs
    public AudioClip anvil1;
    public static AudioClip Anvil1;
    public AudioClip clang1;
    public static AudioClip Clang1;
    public AudioClip dink1;
    public static AudioClip Dink1;
    public AudioClip dink2;
    public static AudioClip Dink2;
    public AudioClip dink3;
    public static AudioClip Dink3;
    public AudioClip metalOnFlesh1;
    public static AudioClip MetalOnFlesh1;
    public AudioClip metalOnFlesh2;
    public static AudioClip MetalOnFlesh2;

    //metal on flesh thunk, or something
    public AudioClip thunk1;
    public static AudioClip Thunk1;
    public AudioClip metalOnMetal1;
    public static AudioClip MetalOnMetal1;
    public AudioClip metalOnMetal2;
    public static AudioClip MetalOnMetal2;
    public AudioClip metalOnMetal3;
    public static AudioClip MetalOnMetal3;

    //flesh sounds
    public AudioClip fleshSquish1;
    public static AudioClip FleshSquish1;
    public AudioClip fleshSquish2;
    public static AudioClip FleshSquish2;
    // / / / / / / / / / / / / / / / / / / / / / / / / / Character Voicelines (AUDIOSOURCE 2) / / / / / / / / / / / / / / / / / / / / / /
    //public AudioClip GladGrunt;

    // / / / / / / / / / / / / / / / / / / / / / Human Hurt, Grunt, and Dying Sounds (AUDIOSOURCE 3) / / / / / / / / / / / / / / / / / / /
    //male grunts and hurt noises
    public AudioClip maleHurt1;
    public static AudioClip MaleHurt1;
    public AudioClip maleHurt2;
    public static AudioClip MaleHurt2;
    public AudioClip maleHurt3;
    public static AudioClip MaleHurt3;
    public AudioClip maleHurt4;
    public static AudioClip MaleHurt4;
    public AudioClip maleHurt5;
    public static AudioClip MaleHurt5;
    public AudioClip maleHurt6;
    public static AudioClip MaleHurt6;
    public AudioClip maleHurt7;
    public static AudioClip MaleHurt7;
    public AudioClip maleHurt8;
    public static AudioClip MaleHurt8;
    public AudioClip maleHurt9;
    public static AudioClip MaleHurt9;
    public AudioClip maleHurt10;
    public static AudioClip MaleHurt10;
    public AudioClip maleHurt11;
    public static AudioClip MaleHurt11;
    public AudioClip maleHurt12;
    public static AudioClip MaleHurt12;
    public AudioClip maleHurt13;
    public static AudioClip MaleHurt13;
    public AudioClip maleHurt14;
    public static AudioClip MaleHurt14;
    public AudioClip maleGrunt1;
    public static AudioClip MaleGrunt1;
    public AudioClip maleGrunt2;
    public static AudioClip MaleGrunt2;
    public AudioClip maleGrunt3;
    public static AudioClip MaleGrunt3;
    public AudioClip maleGrunt4;
    public static AudioClip MaleGrunt4;
    public AudioClip maleGrunt5;
    public static AudioClip MaleGrunt5;
    public AudioClip maleGrunt6;
    public static AudioClip MaleGrunt6;
    public AudioClip maleGrunt7;
    public static AudioClip MaleGrunt7;
    public AudioClip maleBattlecry1;
    public static AudioClip MaleBattlecry1;
    public AudioClip maleBattlecry2;
    public static AudioClip MaleBattlecry2;
    public AudioClip maleBattlecry3;
    public static AudioClip MaleBattlecry3;
    public AudioClip maleBattlecry4;
    public static AudioClip MaleBattlecry4;
    public AudioClip maleBattlecry5;
    public static AudioClip MaleBattlecry5;
    public AudioClip maleBattlecry6;
    public static AudioClip MaleBattlecry6;
    public AudioClip maleBattlecry7;
    public static AudioClip MaleBattlecry7;

    //female grunts and hurt noises
    //public AudioClip femaleHurt1;

    //male die noises
    public AudioClip maleDie1;
    public static AudioClip MaleDie1;
    public AudioClip maleDie2;
    public static AudioClip MaleDie2;
    public AudioClip maleDie3;
    public static AudioClip MaleDie3;
    public AudioClip maleDie4;
    public static AudioClip MaleDie4;
    public AudioClip maleDie5;
    public static AudioClip MaleDie5;

    //female die noises
    //public AudioClip femaleDie1;

    //other
    public AudioClip fwoo1;
    public static AudioClip Fwoo1;
    public AudioClip fwoo2;
    public static AudioClip Fwoo2;
    public AudioClip woosh1;
    public static AudioClip Woosh1;
    public AudioClip woosh2;
    public static AudioClip Woosh2;
    public AudioClip woosh3;
    public static AudioClip Woosh3;
    public AudioClip woosh4;
    public static AudioClip Woosh4;
    public AudioClip woosh5;
    public static AudioClip Woosh5;
    public AudioClip woosh6;
    public static AudioClip Woosh6;
    void Start()
    {
        Slash1 = slash1;
        Stab1 = stab1;
        Stab2 = stab2;
        Stab3 = stab3;
        StabIn = stabIn;
        StabOut = stabOut;
        QuickStab1 = quickStab1;
        Anvil1 = anvil1;
        Clang1 = clang1;
        Dink1 = dink1;
        Dink2 = dink2;
        Dink3 = dink3;
        MetalOnFlesh1 = metalOnFlesh1;
        MetalOnFlesh2 = metalOnFlesh2;
        Thunk1 = thunk1;
        MetalOnMetal1 = metalOnMetal1;
        MetalOnMetal2 = metalOnMetal2;
        MetalOnMetal3 = metalOnMetal3;
        FleshSquish1 = fleshSquish1;
        FleshSquish2 = fleshSquish2;
        MaleHurt1 = maleHurt1;
        MaleHurt2 = maleHurt2;
        MaleHurt3 = maleHurt3;
        MaleHurt4 = maleHurt4;
        MaleHurt5 = maleHurt5;
        MaleHurt6 = maleHurt6;
        MaleHurt7 = maleHurt7;
        MaleHurt8 = maleHurt8;
        MaleHurt9 = maleHurt9;
        MaleHurt10 = maleHurt10;
        MaleHurt11 = maleHurt11;
        MaleHurt12 = maleHurt12;
        MaleHurt13 = maleHurt13;
        MaleHurt14 = maleHurt14;
        MaleGrunt1 = maleGrunt1;
        MaleGrunt2 = maleGrunt2;
        MaleGrunt3 = maleGrunt3;
        MaleGrunt4 = maleGrunt4;
        MaleGrunt5 = maleGrunt5;
        MaleGrunt6 = maleGrunt6;
        MaleGrunt7 = maleGrunt7;
        MaleBattlecry1 = maleBattlecry1;
        MaleBattlecry2 = maleBattlecry2;
        MaleBattlecry3 = maleBattlecry3;
        MaleBattlecry4 = maleBattlecry4;
        MaleBattlecry5 = maleBattlecry5;
        MaleBattlecry6 = maleBattlecry6;
        MaleBattlecry7 = maleBattlecry7;
        MaleDie1 = maleDie1;
        MaleDie2 = maleDie2;
        MaleDie3 = maleDie3;
        MaleDie4 = maleDie4;
        MaleDie5 = maleDie5;
        Fwoo1 = fwoo1;
        Fwoo2 = fwoo2;
        Woosh1 = woosh1;
        Woosh2 = woosh2;
        Woosh3 = woosh3;
        Woosh4 = woosh4;
        Woosh5 = woosh5;
        Woosh6 = woosh6;
    }
    public static int random;
    public static void playSlash(AudioSource audioSource)
    {
        random = (int)Random.Range(1, 2);
        switch (random)
        {
            case 1:
                audioSource.clip = Slash1;
                audioSource.Play();
                break;
        }
    }
    public static void playStab(AudioSource audioSource)
    {
        random = (int)Random.Range(1, 4);
        switch (random)
        {
            case 1:
                audioSource.clip = Stab1;
                audioSource.Play();
                break;
            case 2:
                audioSource.clip = Stab2;
                audioSource.Play();
                break;
            case 3:
                audioSource.clip = Stab3;
                audioSource.Play();
                break;
        }
    }
    public static void playQuickStab(AudioSource audioSource)
    {
        random = (int)Random.Range(1, 2);
        switch (random)
        {
            case 1:
                audioSource.clip = QuickStab1;
                audioSource.Play();
                break;
        }
    }
    public static void playClang(AudioSource audioSource)
    {
        random = (int)Random.Range(1, 2);
        switch (random)
        {
            case 1:
                audioSource.clip = Clang1;
                audioSource.Play();
                break;
        }
    }
    public static void playMetalOnFlesh(AudioSource audioSource)
    {
        random = (int)Random.Range(1, 3);
        switch (random)
        {
            case 1:
                audioSource.clip = MetalOnFlesh1;
                audioSource.Play();
                break;
            case 2:
                audioSource.clip = MetalOnFlesh2;
                audioSource.Play();
                break;
        }
    }
    public static void playThunk(AudioSource audioSource)
    {
        random = (int)Random.Range(1, 2);
        switch (random)
        {
            case 1:
                audioSource.clip = Thunk1;
                audioSource.Play();
                break;
        }
    }
    public static void playFleshSquish(AudioSource audioSource)
    {
        random = (int)Random.Range(1, 3);
        switch (random)
        {
            case 1:
                audioSource.clip = FleshSquish1;
                audioSource.Play();
                break;
            case 2:
                audioSource.clip = FleshSquish2;
                audioSource.Play();
                break;
        }
    }
    public static void playMaleHurt(AudioSource audioSource)
    {
        random = (int)Random.Range(1, 4);
        switch (random)
        {
            case 1:
                audioSource.clip = MaleHurt1;
                audioSource.Play();
                break;
            case 2:
                audioSource.clip = MaleHurt2;
                audioSource.Play();
                break;
            case 3:
                audioSource.clip = MaleHurt3;
                audioSource.Play();
                break;
            case 4:
                audioSource.clip = MaleHurt4;
                audioSource.Play();
                break;
            case 5:
                audioSource.clip = MaleHurt5;
                audioSource.Play();
                break;
            case 6:
                audioSource.clip = MaleHurt6;
                audioSource.Play();
                break;
            case 7:
                audioSource.clip = MaleHurt7;
                audioSource.Play();
                break;
            case 8:
                audioSource.clip = MaleHurt8;
                audioSource.Play();
                break;
            case 9:
                audioSource.clip = MaleHurt9;
                audioSource.Play();
                break;
            case 10:
                audioSource.clip = MaleHurt10;
                audioSource.Play();
                break;
            case 11:
                audioSource.clip = MaleHurt11;
                audioSource.Play();
                break;
            case 12:
                audioSource.clip = MaleHurt12;
                audioSource.Play();
                break;
            case 13:
                audioSource.clip = MaleHurt13;
                audioSource.Play();
                break;
            case 14:
                audioSource.clip = MaleHurt14;
                audioSource.Play();
                break;
        }
    }
    public static void playFemaleHurt(AudioSource audioSource)
    {
        random = (int)Random.Range(1, 2);
        switch (random)
        {
            case 1:
                //audioSource.clip = femaleHurt1;
                audioSource.Play();
                break;
        }
    }
    public static void playMaleDie(AudioSource audioSource)
    {
        random = (int)Random.Range(1, 6);
        switch (random)
        {
            case 1:
                audioSource.clip = MaleDie1;
                audioSource.Play();
                break;
            case 2:
                audioSource.clip = MaleDie2;
                audioSource.Play();
                break;
            case 3:
                audioSource.clip = MaleDie3;
                audioSource.Play();
                break;
            case 4:
                audioSource.clip = MaleDie4;
                audioSource.Play();
                break;
            case 5:
                audioSource.clip = MaleDie5;
                audioSource.Play();
                break;
        }
    }
    public static void playFemaleDie(AudioSource audioSource)
    {
        random = (int)Random.Range(1, 2);
        switch (random)
        {
            case 1:
                //audioSource.clip = femaleDie1;
                audioSource.Play();
                break;
        }
    }

    public static void playMaleBattlecry(AudioSource audioSource)
    {
        random = (int)Random.Range(1, 8);
        switch (random)
        {
            case 1:
                audioSource.clip = MaleBattlecry1;
                audioSource.Play();
                break;
            case 2:
                audioSource.clip = MaleBattlecry2;
                audioSource.Play();
                break;
            case 3:
                audioSource.clip = MaleBattlecry3;
                audioSource.Play();
                break;
            case 4:
                audioSource.clip = MaleBattlecry4;
                audioSource.Play();
                break;
            case 5:
                audioSource.clip = MaleBattlecry5;
                audioSource.Play();
                break;
            case 6:
                audioSource.clip = MaleBattlecry6;
                audioSource.Play();
                break;
            case 7:
                audioSource.clip = MaleBattlecry7;
                audioSource.Play();
                break;
        }
    }
    public static void playMaleGrunt(AudioSource audioSource)
    {
        random = (int)Random.Range(1, 8);
        switch (random)
        {
            case 1:
                audioSource.clip = MaleGrunt1;
                audioSource.Play();
                break;
            case 2:
                audioSource.clip = MaleGrunt2;
                audioSource.Play();
                break;
            case 3:
                audioSource.clip = MaleGrunt3;
                audioSource.Play();
                break;
            case 4:
                audioSource.clip = MaleGrunt4;
                audioSource.Play();
                break;
            case 5:
                audioSource.clip = MaleGrunt5;
                audioSource.Play();
                break;
            case 6:
                audioSource.clip = MaleGrunt6;
                audioSource.Play();
                break;
            case 7:
                audioSource.clip = MaleGrunt7;
                audioSource.Play();
                break;
        }
    }
}
