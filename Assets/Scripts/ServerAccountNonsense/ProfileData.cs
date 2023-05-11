using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProfileData { 

    public string profileName;
    public int profileLevel;
    public int profileIcon;
    public string profileTitle;

    /* character string 000.0.0.000.000.00000.0.0.0.0.0.0.0
     * Digits 1-3 : Character ID
     * Digit 4 : Binary Activated or not
     * Digit 5 : Stars
     * Digits 6-8 : Number of Shards (total out of 500)
     * Digits 9-11 : Level
     * Digits 12-16 : XP (Just for that level ig)
     * Digit 17 : Number of Ascension Levels Ascension
     * Digit 18 : Binary Ascension 1
     * Digit 19 : Binary Ascension 2
     * Digit 20 : Binary Ascension 3
     * Digit 21 : Binary Ascension 4
     * Digit 22 : Binary Ascension 5
     * Digit 23 : Primary Element
     */

    public string[] characters;
    public ProfileData(ProfileInfo profile)
    {
        profileName = profile.profileName;
        profileLevel = profile.profileLevel;
        profileIcon = profile.profileIcon;
        profileTitle = profile.profileTitle;
        characters = ProfileInfo.characters;
    }
}
