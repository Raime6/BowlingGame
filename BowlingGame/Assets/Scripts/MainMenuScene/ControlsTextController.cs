using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlsTextController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] TMP_Text DescriptiveText; // Text that will be modified
    [SerializeField] string Description;       // Text used to descrive the Control's texts


    public void OnPointerEnter(PointerEventData eventData)
    {

        DescriptiveText.text = Description;

    }

    public void OnPointerExit(PointerEventData eventData)
    {

        DescriptiveText.text = "";

    }

}
