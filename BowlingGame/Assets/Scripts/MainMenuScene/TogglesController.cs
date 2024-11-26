using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TogglesController : MonoBehaviour
{

    [SerializeField] GlobalVariables _globalVariables; // GlobalVariables's script
    private Transform Minicamera;                      // Top-left Minicamera during the game
    private Transform ControlsWindow;                  // Controls info window during the game


    // Builder
    private void Awake()
    {

        _globalVariables = GameObject.Find("GlobalVariables").GetComponent<GlobalVariables>();

    }

    /* Changes the game's type of shooting */
    public void SetEffectShootToggle()
    {

        _globalVariables.EffectShoot = !_globalVariables.EffectShoot;

    }

    public void EnableDisableMinicamera()
    {

        _globalVariables.MinicameraEnabled = !_globalVariables.MinicameraEnabled;

        if(SceneManager.GetActiveScene().name == "BowlingScene")
        {

            // If the Minicamera is not assigned, finds it
            if (Minicamera == null)
                Minicamera = GameObject.Find("Main Camera").transform.GetChild(0);

            Minicamera.gameObject.SetActive(!Minicamera.gameObject.activeSelf);

        }

    }

    public void EnableDisableControlsWindow()
    {

        _globalVariables.ControlsWindowEnabled = !_globalVariables.ControlsWindowEnabled;

        if (SceneManager.GetActiveScene().name == "BowlingScene")
        {

            // If the Control Window is not assigned, finds it
            if (ControlsWindow == null)
                ControlsWindow = GameObject.Find("UI").transform.GetChild(2);

            ControlsWindow.gameObject.SetActive(!ControlsWindow.gameObject.activeSelf);

        }

    }

}
