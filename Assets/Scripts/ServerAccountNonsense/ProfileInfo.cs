using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileInfo : MonoBehaviour
{
    #region Variables
    public string profileName;
    public GameObject username;
    public int profileLevel;
    public int profileIcon;
    public string profileTitle;
    #endregion
    public static string[] characters = new string[41]; //Ryan and Roman, this is the only line you edit
    //                                                  The Index should be greater than or equal to the latest index on the Spreadsheet on PROC Mechanics
    #region ¯\_(ツ)_/¯
    private void Start()
    {
        profileName = "marcus, as per gNoam's requests"; //dw about this line
        for (int i = 0; i < characters.Length; i++)
        {
            string pre_char_string = "i hate c#";
            if (i < 10)
                pre_char_string = "00" + i.ToString();
            if (10 <= i && i < 100)
                pre_char_string = "0" + i.ToString();
            if (100 <= i)
                pre_char_string = i.ToString();
            characters[i] = "" + pre_char_string + ".1.7.000.100.00000.5.0.0.0.0.0.8";
        }
        username.GetComponent<Text>().text = profileName;
    }
    public void SaveProfile()
    {
        SaveSystem.SaveProfile(this);
    }

    public void LoadProfile()
    {
        ProfileData data = SaveSystem.LoadProfile();

        profileName = data.profileName;
    }
    #endregion
}
