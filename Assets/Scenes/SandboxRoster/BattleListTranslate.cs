using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleListTranslate : MonoBehaviour
{
    public GameObject portraitList;
    public GameObject Slider;
    Vector3 portraitsInitial;
    float mousePosInitial;
    float deltaMousePos = 0;

    private void Start()
    {
        portraitsInitial = portraitList.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (Slider.GetComponent<Scrollbar>().value + (Input.GetAxis("Mouse ScrollWheel") * -.39f) < 0)
                Slider.GetComponent<Scrollbar>().value = 0;
            else if (Slider.GetComponent<Scrollbar>().value + (Input.GetAxis("Mouse ScrollWheel") * -.39f) > 1)
                Slider.GetComponent<Scrollbar>().value = 1;
            else
                Slider.GetComponent<Scrollbar>().value += (Input.GetAxis("Mouse ScrollWheel") * -.39f);
        }
    }

    private void OnMouseDown()
    {
        mousePosInitial = Input.mousePosition.y;
    }

    private void OnMouseDrag()
    {
        if ((portraitList.transform.position.y + (deltaMousePos * -.007f)) > 7.943961 && (portraitList.transform.position.y + (deltaMousePos * -.007f)) < (2.99f * (this.GetComponent<BattlePortraits>().modifiableArray.Count - 1) / 2))
        {
            Slider.GetComponent<Scrollbar>().value += (deltaMousePos * (-0.000134183223818f + -.020418142149f * Mathf.Pow(((this.GetComponent<BattlePortraits>().modifiableArray.Count - 1) / 2), -1.653934971f)));
        }
        deltaMousePos = mousePosInitial - Input.mousePosition.y;
        mousePosInitial = Input.mousePosition.y;
    }
    public void OnSlider()
    {
        if ((this.GetComponent<BattlePortraits>().modifiableArray.Count - 1) / 2 >= 3)
            portraitList.transform.position = portraitsInitial + (portraitList.transform.up * (Slider.GetComponent<Scrollbar>().value) * (-5.12f + 2.99f * ((this.GetComponent<BattlePortraits>().modifiableArray.Count - 1) / 2)) * portraitList.transform.localScale.x);
    }
}
