using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public void Roster()
    {
        SceneManager.LoadScene(5);
    }
    public void GoToRoster()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
