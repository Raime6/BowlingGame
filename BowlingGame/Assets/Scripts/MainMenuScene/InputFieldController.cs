using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldController : MonoBehaviour
{

    public Transform InputFieldPlaceholder; // When selecting the InputField, "Enter name" text will disapear

    private void Awake()
    {

        InputFieldPlaceholder = transform.GetChild(0).transform.GetChild(0);

    }

    // This method is called when the User selects the InputField. Cleans the Text Area to indicate the User that he can write
    public void OnSelectingInputField()
    {
        
        InputFieldPlaceholder.GetComponent<TMP_Text>().text = string.Empty;

    }

    // This method is called when the User deselects the InputField. If the User hasn't write anything, restores the generic text
    public void OnDeselectingInputField()
    {
        
        if(this.GetComponent<TMP_InputField>().text == "")
            InputFieldPlaceholder.GetComponent<TMP_Text>().text = "Enter a name";

    }

}
