using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{

    // MAIN MENU

    [SerializeField] GlobalVariables _globalVariables;      // GlobalVariables's script
    [SerializeField] Animator _animator;                    // Button's animator
    [SerializeField] int Id;                                // Button's ID

    [SerializeField] Transform MainMenuPanel;               // Main Menu Interface
    [SerializeField] Transform SettingsPanel;               // Settings Interface
    [SerializeField] GameObject SelectPlayersPanel;         // Select Players Interface
    [SerializeField] GameObject TypePlayerPanel;            // Type Player Interface
    [SerializeField] List<GameObject> PlayersNamesList;     // Type Player Interface
    [SerializeField] List<GameObject> PlayersTypeList;      // Type Player Interface
    [SerializeField] Transform PauseContainer;              // Pause Container
    [SerializeField] Transform PausePanel;                  // Pause Interface
    [SerializeField] Transform FinalScorePanel;            // Final Score Panel

    private bool ReactivateFinalScore;                      // If true, Final Score Panel needs to be reactivated
    private bool FinalScoreDisabled;


    // Builder
    private void Awake()
    {

        _globalVariables = GameObject.Find("GlobalVariables").GetComponent<GlobalVariables>();

        if(SceneManager.GetActiveScene().name == "MainMenuScene")
            MainMenuPanel = GameObject.Find("MainMenuCanvas").transform.GetChild(1);

        SettingsPanel = GameObject.Find("PauseCanvas").transform.GetChild(1);
        PauseContainer = GameObject.Find("PauseCanvas").transform.GetChild(0);
        PausePanel = GameObject.Find("PauseCanvas").transform.GetChild(0).transform.GetChild(1);
        FinalScorePanel = GameObject.Find("PauseCanvas").transform.GetChild(3);

        ReactivateFinalScore = false;
        FinalScoreDisabled = false;

    }

    /*********************************************************************************************/
    /* ANIMATIONS */
    public void OnPointerEnter(PointerEventData eventData)
    {

        if(_animator != null)
        {

            _animator.SetBool("Deselected", false);
            _animator.SetBool("Selected", true);

        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if (_animator != null)
        {

            _animator.SetBool("Selected", false);
            _animator.SetBool("Deselected", true);

        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (_animator != null)
            _animator.SetBool("Pressed", true);

    }

    public void OnPointerUp(PointerEventData eventData)
    {

        if (_animator != null)
            _animator.SetBool("Pressed", false);

    }

    // Use when the user has clicked a button and it's animation has ended
    public void EndClickAnimation()
    {

        if (Id == 0)       // Play Button (Main Menu Interface)
            Play();
        else if (Id == 1)  // Settings Button (Main Menu Interface)
            Settings();
        else if (Id == 2)  // Quit Button (Main Menu Interface)
            Quit();
        else if (Id == 3)  // Return Button (Settings Interface)
            SettingsReturnPause();
        else if (Id == 4)  // Return Button (Select Player Interface)
            SelectPlayerReturn();
        else if (Id == 5)  // 1 Player Button (Select Player Interface)
            OnePlayer();
        else if (Id == 6)  // 2 Players Button (Select Player Interface)
            TwoPlayer();
        else if (Id == 7)  // 3 Players Button (Select Player Interface)
            ThreePlayer();
        else if (Id == 8)  // 4 Players Button (Select Player Interface)
            FourPlayer();
        else if (Id == 9)  // Return Button (Type Player Interface)
            TypePlayerReturn();
        else if (Id == 10) // Play Button (Type Player Interface)
            TypePlayerPlay();
        else if (Id == 11) // Resume Button (Pause Interface)
            Pause();
        else if (Id == 12) // Settings Button (Pause Interface)
            SettingsPause();
        else if (Id == 13 || Id == 14) // Main Menu Button (Pause Interface) || Main Menu Button (Final Score Interface)
            MainMenuPause();

    }

    /*********************************************************************************************/

    /* When clicking the Main Menu Button, shows the Select Player Interface */
    private void Play()
    {

        MainMenuPanel.gameObject.SetActive(!MainMenuPanel.gameObject.activeSelf);
        SelectPlayersPanel.SetActive(!SelectPlayersPanel.activeSelf);

    }

    /* When clicking Exit Button, exits the game */
    private void Quit()
    {

        Application.Quit();

    }

    /* When clicking Main Menu Button, loads the Main Menu Scene */
    private void MainMenu()
    {

        SceneManager.LoadScene("MainMenuScene");

    }

    /* When clicking the Controls Button, shows the Controls Interface hidding the Settings Interface */
    private void Settings()
    {

        MainMenuPanel.gameObject.SetActive(!MainMenuPanel.gameObject.activeSelf);
        SettingsPanel.gameObject.SetActive(!SettingsPanel.gameObject.activeSelf);

    }

    /*************************************************************************************************************/

    // SELECT PLAYERS

    /* When clicking the 1 Player Button, shows the Type Player Interface with only one player */
    private void OnePlayer()
    {

        SelectPlayersPanel.SetActive(!SelectPlayersPanel.activeSelf);
        TypePlayerPanel.SetActive(!TypePlayerPanel.activeSelf);
        _globalVariables.NumPlayers = 1;                              // Sets 1 as the number of Players

        // Shows the Name Input and the Type Dropdown of the number of player selected
        for(int i = 0; i < _globalVariables.NumPlayers; i++)
        {

            PlayersNamesList[i].SetActive(true);
            PlayersTypeList[i].SetActive(true);

        }

    }

    /* When clicking the 2 Player Button, shows the Type Player Interface with two players */
    private void TwoPlayer()
    {

        SelectPlayersPanel.SetActive(!SelectPlayersPanel.activeSelf);
        TypePlayerPanel.SetActive(!TypePlayerPanel.activeSelf);
        _globalVariables.NumPlayers = 2;                              // Sets 2 as the number of Players

        // Shows the Name Input and the Type Dropdown of the number of player selected
        for (int i = 0; i < _globalVariables.NumPlayers; i++)
        {

            PlayersNamesList[i].SetActive(true);
            PlayersTypeList[i].SetActive(true);

        }

    }

    /* When clicking the 3 Player Button, shows the Type Player Interface with three player */
    private void ThreePlayer()
    {

        SelectPlayersPanel.SetActive(!SelectPlayersPanel.activeSelf);
        TypePlayerPanel.SetActive(!TypePlayerPanel.activeSelf);
        _globalVariables.NumPlayers = 3;                              // Sets 3 as the number of Players

        // Shows the Name Input and the Type Dropdown of the number of player selected
        for (int i = 0; i < _globalVariables.NumPlayers; i++)
        {

            PlayersNamesList[i].SetActive(true);
            PlayersTypeList[i].SetActive(true);

        }

    }

    /* When clicking the 4 Player Button, shows the Type Player Interface with four player */
    private void FourPlayer()
    {

        SelectPlayersPanel.SetActive(!SelectPlayersPanel.activeSelf);
        TypePlayerPanel.SetActive(!TypePlayerPanel.activeSelf);
        _globalVariables.NumPlayers = 4;                              // Sets 4 as the number of Players

        // Shows the Name Input and the Type Dropdown of the number of player selected
        for (int i = 0; i < _globalVariables.NumPlayers; i++)
        {

            PlayersNamesList[i].SetActive(true);
            PlayersTypeList[i].SetActive(true);

        }

    }

    /* When clicking the Select Player Return Button, shows the Main Menu Interface hidding the Select Player Interface */
    private void SelectPlayerReturn()
    {

        SelectPlayersPanel.SetActive(!SelectPlayersPanel.activeSelf);
        MainMenuPanel.gameObject.SetActive(!MainMenuPanel.gameObject.activeSelf);

    }

    /*************************************************************************************************************/

    // TYPE PLAYER

    /* When clicking the Type Player Play Button, loads the Bowling Scene with the settings customized */
    private void TypePlayerPlay()
    {

        // Sets the Player's Info into the GlobalVariables script
        for(int i = 0; i < _globalVariables.NumPlayers; i++)
        {

            // If the user did't put a name, the progrma sets a generic one, if not, sets the one that the user decided
            if (PlayersNamesList[i].GetComponent<TMP_InputField>().text == "")
            {

                if(PlayersTypeList[i].GetComponent<TMP_Dropdown>().value == 0)
                    _globalVariables.PlayersNamesList.Add("Player" + (i + 1));
                else
                    _globalVariables.PlayersNamesList.Add("CPU" + (i + 1));

            }
            else
                _globalVariables.PlayersNamesList.Add(PlayersNamesList[i].GetComponent<TMP_InputField>().text);

            // Set the type of Player depending on the value selected by the user
            _globalVariables.PlayersTypeList.Add(PlayersTypeList[i].GetComponent<TMP_Dropdown>().value);

        }

        PauseContainer.gameObject.SetActive(true);
        SceneManager.LoadScene("BowlingScene");

    }

    /* When clicking the Type Player Return Button, shows the Select Players Interface hidding the Type Player Interface */
    private void TypePlayerReturn()
    {

        TypePlayerPanel.SetActive(!TypePlayerPanel.activeSelf);
        SelectPlayersPanel.SetActive(!SelectPlayersPanel.activeSelf);

        // Hides the Name Input and the Type Dropdown of the number of player selected
        for (int i = 0; i < _globalVariables.NumPlayers; i++)
        {

            // Resets the InputField
            PlayersNamesList[i].GetComponent<TMP_InputField>().text = "";
            PlayersNamesList[i].transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>().text = "Enter a name";
            
            // Hides the InputField and it's Dropdown
            PlayersNamesList[i].SetActive(false);
            PlayersTypeList[i].SetActive(false);

        }

    }

    /*************************************************************************************************************/

    // BOWLING GAME

    /* When clicking the Pause or Resume Button, stop/resumes the game and shows/hides the Pause Interface */
    public void Pause()
    {

        if(!_globalVariables.PauseActivated || _globalVariables.PauseActivated && PausePanel.gameObject.activeSelf)
        {

            if (Time.timeScale != 0)
                Time.timeScale = 0; // Pauses the game
            else
                Time.timeScale = 1; // Resumes the game

            if (ReactivateFinalScore)
            {

                ReactivateFinalScore = false;
                FinalScorePanel.gameObject.SetActive(true);

            }

            if (FinalScorePanel.gameObject.activeSelf && !FinalScoreDisabled)
            {

                ReactivateFinalScore = true;
                FinalScoreDisabled = true;
                FinalScorePanel.gameObject.SetActive(false);

            }

            PausePanel.gameObject.SetActive(!PausePanel.gameObject.activeSelf);
            _globalVariables.PauseActivated = !_globalVariables.PauseActivated;

        }

    }

    /* Resumes the game and loads the Main Menu Scene */
    private void MainMenuPause()
    {

        Time.timeScale = 1; // Resumes the game
        if(PausePanel != null)
            PausePanel.gameObject.SetActive(false);
        PauseContainer.gameObject.SetActive(false);
        FinalScorePanel.gameObject.SetActive(false);
        _globalVariables.PauseActivated = !_globalVariables.PauseActivated;
        SceneManager.LoadScene("MainMenuScene");

    }

    /*************************************************************************************************************/

    // PAUSE

    /* When clicking the Pause or Resume Button, stop/resumes the game and shows/hides the Pause Interface */
    private void SettingsPause()
    {

        PausePanel.gameObject.SetActive(!PausePanel.gameObject.activeSelf);
        SettingsPanel.gameObject.SetActive(!SettingsPanel.gameObject.activeSelf);

    }

    /* When clicking the Pause or Resume Button, stop/resumes the game and shows/hides the Pause Interface */
    private void SettingsReturnPause()
    {

        if(SceneManager.GetActiveScene().name == "MainMenuScene")
        {

            SettingsPanel.gameObject.SetActive(!SettingsPanel.gameObject.activeSelf);
            if (MainMenuPanel == null)
                MainMenuPanel = GameObject.Find("MainMenuCanvas").transform.GetChild(1);
            MainMenuPanel.gameObject.SetActive(!MainMenuPanel.gameObject.activeSelf);

        }
        else
        {

            PausePanel.gameObject.SetActive(!PausePanel.gameObject.activeSelf);
            SettingsPanel.gameObject.SetActive(!SettingsPanel.gameObject.activeSelf);

        }

    }

}
