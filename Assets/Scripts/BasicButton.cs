using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicButton : MonoBehaviour
{
    GameObject currentUnit;
    
    public static int activatedBinary = 0;
    //Button Textures 
    public Texture2D Null;
    public Sprite texture;
    public Vector3 initialPos;
    public GameObject Buttons;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = Buttons.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        GlobalVariables.playerArray = GameObject.FindGameObjectsWithTag("AllyTurn");
        if (GlobalVariables.playerArray.Length == 0)
        {
            GetComponent<Image>().enabled = false;
        }
        else
        {
                GetComponent<Image>().enabled = true;
                currentUnit = GlobalVariables.playerArray[0];
                GetComponent<Image>().sprite = texture;
                Buttons.transform.position = initialPos + Buttons.transform.right * -.60f * currentUnit.GetComponent<Character>().boutons.Count;
        }
    }
    public void basic()
    {
        activatedBinary += 1;
        StartCoroutine(Wait());
        
    }
    public void moveDownAndBack(int i, GameObject b)
    {
        b.transform.position += this.transform.right * .6f * i;
        //b.transform.position += Vector3.down * 600;
        //Debug.Log("down");
    }
    public void moveUp(GameObject b)
    {
        //b.transform.position += Vector3.up * 600;
        //Debug.Log("up");
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(.2f);
        activatedBinary -= 1;
    }
    float time = 0;
    bool canPop = false;
    public void OnMouseDrag()
    {
        if (canPop)
        {
            if (time < .5f)
            {
                time += Time.deltaTime;
            }
            else
            {
                time = 0;
                currentUnit = GlobalVariables.playerArray[0];
                StatusPopup.playerText.text = currentUnit.GetComponent<Character>().abilities.AbilityTexts[0].Replace("\\n", "\n"); ;
                StatusPopup.AbilityPop();
                canPop = false;
            }
        }
    }
    public void OnMouseDown()
    {
        canPop = true;
    }
}
