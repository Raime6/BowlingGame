using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalVariables : MonoBehaviour
{

    public static GlobalVariables Instance;

    public bool EffectShoot;              // (Can be activated through Controls panel) If true, the user can control the ball's effecto with Bezier Curves, if false the ball will be shot without effect
    public bool MinicameraEnabled;        // (Can be changed through Controls panel) Enables/Disables the top-left minicamera during the game
    public bool ControlsWindowEnabled;    // (Can be changed through Controls panel) Enables/Disables the Controls Window durinf the game
    public int NumPlayers;                // Number of Players in the Game
    public int NumCPUs;                   // Number of CPUs in the Game
    public bool PauseActivated;           // Use to control Pausa Panel, if true, it cannot be activated again
    public List<string> PlayersNamesList; // Player's names list
    public List<int> PlayersTypeList;     // Players type list (0 - Player | 1 - easy CPU | 2 - normal CPU | 3 - hard CPU | 4 - very hard CPU)


    /* Singleton */
    private void Awake()
    {
    
        // If there's no GlobalVariables yet, creates it
        if(Instance == null)
        {

            DontDestroyOnLoad(gameObject);
            Instance = this;

        }
        else
        {

            // If it's a copy
            if(Instance != this)
            {

                // Resets the original variables
                Instance.NumPlayers = 0;
                Instance.NumCPUs = 0;
                Instance.PlayersNamesList.Clear();
                Instance.PlayersTypeList.Clear();
                PauseActivated = false;

                // Destroys the copy
                Destroy(gameObject);

            }

        }

        EffectShoot = false;
        MinicameraEnabled = true;
        ControlsWindowEnabled = true;
        NumPlayers = 0;
        NumCPUs = 0;
        PauseActivated = false;

    }

}
