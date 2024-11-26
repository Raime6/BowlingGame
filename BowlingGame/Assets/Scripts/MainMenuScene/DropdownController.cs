using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownController : MonoBehaviour
{
    public GlobalVariables _globalVariables;        // GlobalVariable's script
    [SerializeField] TMP_InputField PlayerNameText; // Player Name asociated with this Dropdown
    private bool IncreaseCPUs;                      // If true, this dropdown has already increase the numCPUs so in case it's changed to another type of CPU, it cannot increase it again

    /* If the User selects a CPU, sets a default name that can be modified later. If changes to a Player, sets a blank player name */
    public void UpdateInputText()
    {

        if (gameObject.GetComponent<TMP_Dropdown>().value != 0)
        {

            if(!IncreaseCPUs) // If the dropdown value was another type of CPU, it's not necessary to change values
            {

                _globalVariables.NumCPUs++;
                IncreaseCPUs = true;
                PlayerNameText.text = "CPU" + _globalVariables.NumCPUs;

            }

        }
        else
        {

            _globalVariables.NumCPUs--;
            IncreaseCPUs = false;       // The value has changed to a Player type so this indicates it will modify values again in case of changinf it to a CPU again
            PlayerNameText.text = "";

        }

    }

}
