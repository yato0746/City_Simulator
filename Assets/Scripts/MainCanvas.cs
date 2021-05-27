using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    [SerializeField] GameObject dropDownObjects;
    bool isDropDown = true;

    public void Button_DropDown()
    {
        dropDownObjects.SetActive(!isDropDown);
        isDropDown = !isDropDown;
    }
}
