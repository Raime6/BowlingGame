//using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
//using TMPro.EditorUtilities;
using UnityEditor;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{

    // BOWLING BALL
    [SerializeField] GameObject BowlingBallPrefab; // Bowling Ball Prefab
    public Rigidbody BowlingBallRB;                // Bowling Ball's RigidBody
    private Vector3 BallInitialPosition;           // Ball's Initial position when creating it
    public Vector3 BowlingBallOriginalPosition;    // Original position of the ball
    public Quaternion BowlingBallOriginalRotation; // Original orientation of the ball
    /*******************************************************************************************************************/
    // BOWLING PIN
    [SerializeField] GameObject Dummy;             // Father of the Pin, is the one whose transform is modified. Necessary to be able to move vertically the Pins correctly
    [SerializeField] GameObject BowlingPin;        // Bowling Pin Prefab
    // Bowling Pin Coordinates. Although it is the Dummy's transform that is modified
    private List<float> PinPositionX;              // Coord X of the Pin
    private float PinPositionY;                    // Coord Y of the Pin
    private List<float> PinPositionZ;              // Coord Z of the Pin
    // Bowling Pin Materials
    [SerializeField] List<Material> PinMaterials;  // Pin's different materials, modified in the script so there is more variety
    // List of Bowling Pins
    public List<GameObject> ListPins;              // List of Pins that hasn't fallen in the current try
    public int NumPinsFallen;                      // Conts the num of fallen Pins to select what animation is going to play
    /*******************************************************************************************************************/
    // BOWLING PICKER
    public GameObject BowlingPicker;               // Bowling Picker to control its animations
    /*******************************************************************************************************************/
    // SWEEPERVAGON
    public GameObject SweeperVagon;                // Sweeper Vagon to control its animations
    /*******************************************************************************************************************/
    // UI
    public List<GameObject> PointSheetPanelList;   // List of Panels that shows the Points sheet of the game
    public List<TMP_Text> PlayerNameTexts;         // List that contains the Name of the Players in the Game
    public List<TMP_Text> PointsSheetList1;        // List that contains all the TMP_Text of the Points Sheet in the UI
    public List<TMP_Text> PointsSheetList2;        // List that contains all the TMP_Text of the Points Sheet in the UI
    public List<TMP_Text> PointsSheetList3;        // List that contains all the TMP_Text of the Points Sheet in the UI
    public List<TMP_Text> PointsSheetList4;        // List that contains all the TMP_Text of the Points Sheet in the UI
    public Transform FinalScorePanel;              // Panel that shows the Final Score when the Game has ended
    public TMP_Text FinalScore;                    // Player's Final Score text shown at the end of the game
    public GameObject PauseCanvas;                 // Canvas Pause
    public GameObject PauseButton;                 // Pause Button
    public TMP_Text WinnerPlayerText;              // Text that shows the name of the Player who won at the end of the game
    public Transform BallEffectPanel;              // Ball's Effect Panel so it can be controlled if it's shown or not
    public Transform BallEffectSlider;             // Ball's effect slider
    [SerializeField] TMP_Text NameTurnText;        // Shows the name of the current turn's player
    [SerializeField] GameObject Minicamera;        // Top-left minicamera
    [SerializeField] GameObject ControlsWindow;    // Controls info Window
    /*******************************************************************************************************************/
    // GAME CONTROLLER VARIABLES
    public GlobalVariables _globalVariables;       // GlobalVariables's script
    public int Turn;                               // Determines the turn of the player
    public bool StartGame;                         // If true, it's the beggining of the game so the flow of animation it's different only this time
    public bool ShootEnable;                       // false: Shoot Disabled | true: Shoot Enabled
    public bool AnimationOn;                       // If true, an animation is playing
    public bool SweepEnded;                        // If true, sweep has ended so the animation flow changes
    public int CurrentFrame;                       // Actual frame of the game, the game consists on 10 frames (value between 1 and 10) 
    public int CurrentTry;                         // Actual try of the frame, each Frame consists on two Tries, except the 10th Frame in which Tries are up to three (value between 1 and 3)
    public List<int> PlayerTotalPointsList;        // Current points of each Player
    public List<int> Player1PointsList;            // List of the Player's Points in every Frame
    public List<int> Player2PointsList;            // List of the Player's Points in every Frame
    public List<int> Player3PointsList;            // List of the Player's Points in every Frame
    public List<int> Player4PointsList;            // List of the Player's Points in every Frame
    public List<int> PlayerStrikeSpareList;        // List of Players who has done a strike/spare
    public List<int> FrameStrikeSpareList;         // List of Frames where there's been a strike/spare and it's needed to add points
    public List<int> StrikeSpareList;              // List of strikes/spares (1 = spare | 2 = strike)
    public bool StrikeInLastFrame;                 // If true, there's been a strike in the 10th Frame so the game continues to a maximum of 3 tries
    public bool RenableFinalScorePanel;            // If true, the Final Score Panel needs to be re-activated when pressing TAB for the second time
    /*******************************************************************************************************************/


    // Builder
    private void Awake()
    {

        // BOWLING BALL
        BallInitialPosition = new Vector3 (9.558f, 0.2100252f, 0);

        // BOWLING PIN
        PinPositionX = new List<float> { -9.011f, -9.161f, -9.161f, -9.311f, -9.311f, -9.311f, -9.461f, -9.461f, -9.461f, -9.461f };
        PinPositionY = 0.1415551f;
        PinPositionZ = new List<float> { 0, 0.15f, -0.15f, 0.3f, 0, -0.3f, 0.45f, 0.15f, -0.15f, -0.45f };
        ListPins = new List<GameObject> { };
        NumPinsFallen = 0;

        // BOWLING PICKER
        BowlingPicker.GetComponent<PickerController>()._gameController = this; // Saves the Game Controller in the Bowling Picker

        // SWEEPER VAGON
        SweeperVagon.GetComponent<SweepController>()._gameController = this;   // Saves the Game Controller in the Bowling Picker

        // UI
        PauseCanvas = GameObject.Find("PauseCanvas");
        PauseButton = GameObject.Find("PauseButton");
        BallEffectPanel = PauseCanvas.transform.GetChild(2);
        BallEffectSlider = BallEffectPanel.transform.GetChild(1);
        WinnerPlayerText = PauseCanvas.transform.GetChild(3).transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>();
        FinalScore = PauseCanvas.transform.GetChild(3).transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>();
        FinalScorePanel = PauseCanvas.transform.GetChild(3);

        // GAME CONTROLLER VARIABLES
        _globalVariables = GameObject.Find("GlobalVariables").GetComponent<GlobalVariables>();
        Turn = 1;
        StartGame = true;
        ShootEnable = false; // Shoot disabled
        AnimationOn = false; // There's no animation playing
        SweepEnded = false;
        CurrentFrame = 1;
        CurrentTry = 1;
        PlayerTotalPointsList = new List<int> { 0, 0, 0, 0};
        Player1PointsList = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        Player2PointsList = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        Player3PointsList = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        Player4PointsList = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        StrikeInLastFrame = false;
        RenableFinalScorePanel = false;

    }

    // Start is called before the first frame update
    void Start()
    {

        // Minicamera Enabled/Disabled
        if(_globalVariables.MinicameraEnabled)
            Minicamera.SetActive(true);
        else
            Minicamera.SetActive(false);

        // Controls Window Enabled/Disabled
        if (_globalVariables.ControlsWindowEnabled)
            ControlsWindow.SetActive(true);
        else
            ControlsWindow.SetActive(false);

        // Sets the names of the players
        for (int i = 0; i < _globalVariables.NumPlayers; i++)
            PlayerNameTexts[i].text = _globalVariables.PlayersNamesList[i];

        // METHODS
        CreateBall(BallInitialPosition);
        CreatePins();
        SweeperVagon.GetComponent<SweepController>().SweepEnd();

    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
            PauseButton.GetComponent<ButtonsController>().Pause();

        StartAnimations();
        ShowHidePointSheet();

        if(BallEffectPanel != null)
        {

            if (_globalVariables.EffectShoot && ShootEnable)
                BallEffectPanel.gameObject.SetActive(true);
            else
                BallEffectPanel.gameObject.SetActive(false);

        }

    }

    /* Creates de Bowling Ball */
    public void CreateBall(Vector3 position)
    {

        // Creates the ball
        GameObject clon = Instantiate(BowlingBallPrefab);
        clon.GetComponent<BallController>()._gameController = this;
        clon.transform.position = position;                             // Places the ball

        // Saves ball's info to control it later
        BowlingBallRB = clon.GetComponent<Rigidbody>();
        BowlingBallOriginalPosition = BowlingBallRB.transform.position; // Original position of the ball
        BowlingBallOriginalRotation = BowlingBallRB.transform.rotation; // Original orientation of the ball

        // Saves the baa into the ball's effect slider so it can be controlled correctly
        BallEffectSlider.GetComponent<BallEffectSlider>().PlayerBallCenterPoint = clon.transform.GetChild(1);

    }

    /* Creates 10 Bowling Pins */
    public void CreatePins()
    {

        GameObject clonDummy, clonPin;

        // If every pin has fallen, creates all of them again
        if (ListPins.Count == 0)
        {

            for (int i = 0; i < 10; i++)
            {

                // Create the Dummy father
                clonDummy = Instantiate(Dummy);

                // Create the pin
                clonPin = Instantiate(BowlingPin);

                // Implements randomly the pin material so that there is variety
                clonPin.GetComponent<MeshRenderer>().material = PinMaterials[Random.Range(0, 3)];

                // Set the Dummy as father of the Pin
                clonPin.transform.parent = clonDummy.transform;
                clonDummy.transform.position = new Vector3(PinPositionX[i], PinPositionY, PinPositionZ[i]);

                // Saves the Game Controller in the Pin
                clonPin.GetComponent<PinController>()._gameController = this;

                // Add the pin to a list so it can be controlled later
                ListPins.Add(clonDummy);

            }

        }

    }

    /* Destroys the Bowling Pins */
    public void DestroyPins()
    {

        // Destroys the pins that has fallen
        for (int i = ListPins.Count - 1; i >= 0; i--)
        {

            Transform aux = ListPins[i].transform.GetChild(0);

            // Removes the fallen Pins
            if (aux.GetComponent<PinController>().Fallen)
            {

                Destroy(ListPins[i]);
                ListPins.RemoveAt(i); // Removes the pin from the list

            }

        }

    }

    public void StartAnimations()
    {

        // If BALL's X coord is near the first PIN, starts the SWEEPER's animation
        if (BowlingBallRB.transform.position.x <= -8.694f && !AnimationOn)
            SweeperVagon.GetComponent<SweepController>().MoveDownStart();

    }

    /* Resets shooting unless the 10th Frame has ended */
    public void ResetShoot()
    {

        // Updates the text with the current turn's player
        NameTurnText.text = _globalVariables.PlayersNamesList[Turn - 1];

        // Reset Ball's variables to enable shooting again
        BowlingBallRB.useGravity = false;
        BowlingBallRB.transform.position = BowlingBallOriginalPosition;
        BowlingBallRB.transform.rotation = BowlingBallOriginalRotation;
        BowlingBallRB.GetComponent<BallController>().BallLine.SetActive(true);

        // Stop Ball's movement
        BowlingBallRB.isKinematic = true;
        BowlingBallRB.isKinematic = false;

        // Resets the strike boolean in the 10th Frame
        StrikeInLastFrame = false;

        // Resets ball's Bezier Central Point
        BallEffectSlider.GetComponent<BallEffectSlider>().SetValue(0);
        BowlingBallRB.transform.GetChild(1).transform.position = new Vector3(BowlingBallRB.transform.GetChild(1).transform.position.x, BowlingBallRB.transform.GetChild(1).transform.position.y, 0);

        // Reset the pins unless the 10th Frame has ended
        DestroyPins();
        CreatePins();

        // Checks if the current Turn is a Player or a the CPU
        if (_globalVariables.PlayersTypeList[Turn - 1] == 0)
            ShootEnable = true;
        else
            StartCoroutine(BowlingBallRB.GetComponent<BallController>().CpuShoot(_globalVariables.PlayersTypeList[Turn - 1]));

    }

    /* Updates the Points Sheet with the current Player's points depending on the actual Frame or Try */
    public void SetPointsInSheet(string points)
    {

        if(Turn == 1)
        {

            // POINTS SHEET
            // To prevent updating every text "by hand", the formula to access every text depends on the CurrentTry and the CurrentFrame
            // Formula: (CurrentTry - 1) + (3 * (CurrentFrame - 1)) --> Ex: Frame 4/Try 2 = PointsSheetList[(2 - 1) + (3 * (4 - 1))] = PointsSheetList[10]
            // Try's Points
            PointsSheetList1[(CurrentTry - 1) + (3 * (CurrentFrame - 1))].text = points;
            // Frame's Points
            if (CurrentFrame == 10)                                                               // Case is the last Frame, text positions change a little bit
                PointsSheetList1[2 + (3 * (CurrentFrame - 1)) + 1].text = "" + PlayerTotalPointsList[Turn - 1];
            else
                PointsSheetList1[2 + (3 * (CurrentFrame - 1))].text = "" + PlayerTotalPointsList[Turn - 1];

            Player1PointsList[CurrentFrame - 1] = PlayerTotalPointsList[Turn - 1];                     // Saves the Player's Points in this Frame in the list
            PointsSheetList1[PointsSheetList1.Count - 1].text = "" + PlayerTotalPointsList[Turn - 1]; // Game's Points

        }
        else if (Turn == 2)
        {

            // POINTS SHEET
            // To prevent updating every text "by hand", the formula to access every text depends on the CurrentTry and the CurrentFrame
            // Formula: (CurrentTry - 1) + (3 * (CurrentFrame - 1)) --> Ex: Frame 4/Try 2 = PointsSheetList[(2 - 1) + (3 * (4 - 1))] = PointsSheetList[10]
            // Try's Points
            PointsSheetList2[(CurrentTry - 1) + (3 * (CurrentFrame - 1))].text = points;
            // Frame's Points
            if (CurrentFrame == 10)                                                               // Case is the last Frame, text positions change a little bit
                PointsSheetList2[2 + (3 * (CurrentFrame - 1)) + 1].text = "" + PlayerTotalPointsList[Turn - 1];
            else
                PointsSheetList2[2 + (3 * (CurrentFrame - 1))].text = "" + PlayerTotalPointsList[Turn - 1];

            Player2PointsList[CurrentFrame - 1] = PlayerTotalPointsList[Turn - 1];                     // Saves the Player's Points in this Frame in the list
            PointsSheetList2[PointsSheetList2.Count - 1].text = "" + PlayerTotalPointsList[Turn - 1]; // Game's Points

        }
        else if (Turn == 3)
        {

            // POINTS SHEET
            // To prevent updating every text "by hand", the formula to access every text depends on the CurrentTry and the CurrentFrame
            // Formula: (CurrentTry - 1) + (3 * (CurrentFrame - 1)) --> Ex: Frame 4/Try 2 = PointsSheetList[(2 - 1) + (3 * (4 - 1))] = PointsSheetList[10]
            // Try's Points
            PointsSheetList3[(CurrentTry - 1) + (3 * (CurrentFrame - 1))].text = points;
            // Frame's Points
            if (CurrentFrame == 10)                                                               // Case is the last Frame, text positions change a little bit
                PointsSheetList3[2 + (3 * (CurrentFrame - 1)) + 1].text = "" + PlayerTotalPointsList[Turn - 1];
            else
                PointsSheetList3[2 + (3 * (CurrentFrame - 1))].text = "" + PlayerTotalPointsList[Turn - 1];

            Player3PointsList[CurrentFrame - 1] = PlayerTotalPointsList[Turn - 1];                     // Saves the Player's Points in this Frame in the list
            PointsSheetList3[PointsSheetList3.Count - 1].text = "" + PlayerTotalPointsList[Turn - 1]; // Game's Points

        }
        else
        {

            // POINTS SHEET
            // To prevent updating every text "by hand", the formula to access every text depends on the CurrentTry and the CurrentFrame
            // Formula: (CurrentTry - 1) + (3 * (CurrentFrame - 1)) --> Ex: Frame 4/Try 2 = PointsSheetList[(2 - 1) + (3 * (4 - 1))] = PointsSheetList[10]
            // Try's Points
            PointsSheetList4[(CurrentTry - 1) + (3 * (CurrentFrame - 1))].text = points;
            // Frame's Points
            if (CurrentFrame == 10)                                                               // Case is the last Frame, text positions change a little bit
                PointsSheetList4[2 + (3 * (CurrentFrame - 1)) + 1].text = "" + PlayerTotalPointsList[Turn - 1];
            else
                PointsSheetList4[2 + (3 * (CurrentFrame - 1))].text = "" + PlayerTotalPointsList[Turn - 1];

            Player4PointsList[CurrentFrame - 1] = PlayerTotalPointsList[Turn - 1];                     // Saves the Player's Points in this Frame in the list
            PointsSheetList4[PointsSheetList4.Count - 1].text = "" + PlayerTotalPointsList[Turn - 1]; // Game's Points

        }

    }

    /* Updates the Points Sheet with the bonus points in the frames where there's been a strike/spare */
    public void SetBonusPointsInSheet()
    {

        for(int i = FrameStrikeSpareList.Count - 1; i >= 0; i--)
        {

            // Adds the points only if the current turn's Player is the one who did the strike/spare
            if (PlayerStrikeSpareList[i] == Turn)
            {

                int aux = NumPinsFallen;                              // Calculates the Pins fallen in this try
                PlayerTotalPointsList[Turn - 1] += aux;               // Adds those points to the total amount of Player's points

                if (Turn == 1)
                {

                    aux += Player1PointsList[FrameStrikeSpareList[i] - 1]; // Adds the points made by the Player in the last frame to the ones made in this try
                    Player1PointsList[FrameStrikeSpareList[i] - 1] = aux;  // Updates the list of Player's points per Frame

                    // If it's a strike and it's the second Frame since then
                    if (StrikeSpareList[i] == 2 && ((CurrentFrame - FrameStrikeSpareList[i]) == 2))
                    {

                        Player1PointsList[FrameStrikeSpareList[i]] += NumPinsFallen;                                                 // Updates the list of Player's points per Frame
                        PointsSheetList1[2 + (3 * (FrameStrikeSpareList[i]))].text = "" + Player1PointsList[FrameStrikeSpareList[i]]; // Updates the Points Sheet UI

                    }

                    PointsSheetList1[2 + (3 * (FrameStrikeSpareList[i] - 1))].text = "" + aux; // Updates the Points Sheet UI

                }
                else if (Turn == 2)
                {

                    aux += Player2PointsList[FrameStrikeSpareList[i] - 1]; // Adds the points made by the PLayer in the last frame to the ones made in this try
                    Player2PointsList[FrameStrikeSpareList[i] - 1] = aux;  // Updates the list of Player's points per Frame

                    // If it's a strike and it's the second Frame since then
                    if (StrikeSpareList[i] == 2 && ((CurrentFrame - FrameStrikeSpareList[i]) == 2))
                    {

                        Player2PointsList[FrameStrikeSpareList[i]] += NumPinsFallen;                                                 // Updates the list of Player's points per Frame
                        PointsSheetList2[2 + (3 * (FrameStrikeSpareList[i]))].text = "" + Player2PointsList[FrameStrikeSpareList[i]]; // Updates the Points Sheet UI

                    }

                    PointsSheetList2[2 + (3 * (FrameStrikeSpareList[i] - 1))].text = "" + aux; // Updates the Points Sheet UI

                }
                else if (Turn == 3)
                {

                    aux += Player3PointsList[FrameStrikeSpareList[i] - 1]; // Adds the points made by the PLayer in the last frame to the ones made in this try
                    Player3PointsList[FrameStrikeSpareList[i] - 1] = aux;  // Updates the list of Player's points per Frame

                    // If it's a strike and it's the second Frame since then
                    if (StrikeSpareList[i] == 2 && ((CurrentFrame - FrameStrikeSpareList[i]) == 2))
                    {

                        Player3PointsList[FrameStrikeSpareList[i]] += NumPinsFallen;                                                 // Updates the list of Player's points per Frame
                        PointsSheetList3[2 + (3 * (FrameStrikeSpareList[i]))].text = "" + Player3PointsList[FrameStrikeSpareList[i]]; // Updates the Points Sheet UI

                    }

                    PointsSheetList3[2 + (3 * (FrameStrikeSpareList[i] - 1))].text = "" + aux; // Updates the Points Sheet UI

                }
                else
                {

                    aux += Player4PointsList[FrameStrikeSpareList[i] - 1]; // Adds the points made by the PLayer in the last frame to the ones made in this try
                    Player4PointsList[FrameStrikeSpareList[i] - 1] = aux;  // Updates the list of Player's points per Frame

                    // If it's a strike and it's the second Frame since then
                    if (StrikeSpareList[i] == 2 && ((CurrentFrame - FrameStrikeSpareList[i]) == 2))
                    {

                        Player4PointsList[FrameStrikeSpareList[i]] += NumPinsFallen;                                                 // Updates the list of Player's points per Frame
                        PointsSheetList4[2 + (3 * (FrameStrikeSpareList[i]))].text = "" + Player4PointsList[FrameStrikeSpareList[i]]; // Updates the Points Sheet UI

                    }

                    PointsSheetList4[2 + (3 * (FrameStrikeSpareList[i] - 1))].text = "" + aux; // Updates the Points Sheet UI

                }

                // If its the second try of the actual Frame or every Pin has fallen
                if ((CurrentFrame - FrameStrikeSpareList[i]) > (StrikeSpareList[i] - 1) && (CurrentTry == 2 || NumPinsFallen == ListPins.Count))
                {

                    // Clears all lists as the bonus points have been set
                    PlayerStrikeSpareList.RemoveAt(i);
                    FrameStrikeSpareList.RemoveAt(i);
                    StrikeSpareList.RemoveAt(i);

                }

            }

        }

    }

    /* Shows/Hide the Points Sheet of the UI that shows the Points per Frame of the Player */
    private void ShowHidePointSheet()
    {

        if(Input.GetKeyDown(KeyCode.Tab))
        {

            // If activated, hides the Controls Window. If it was enable, shows it again when pressing TAB for the second time
            if (ControlsWindow.activeSelf)
                ControlsWindow.SetActive(false);
            else if (!ControlsWindow.activeSelf && _globalVariables.ControlsWindowEnabled)
                ControlsWindow.SetActive(true);

            // If activated, hides the Final Score Panel. If it was activated, shows it again when pressing TAB for the second time
            if (FinalScorePanel.gameObject.activeSelf)
            {

                RenableFinalScorePanel = true;
                FinalScorePanel.gameObject.SetActive(false);

            }
            else if (!FinalScorePanel.gameObject.activeSelf && RenableFinalScorePanel)
            {

                RenableFinalScorePanel = true;
                FinalScorePanel.gameObject.SetActive(true);

            }

            for (int i = 0; i < _globalVariables.NumPlayers; i++)
                PointSheetPanelList[i].SetActive(!PointSheetPanelList[i].activeSelf);

            //TotalPointsPanel.SetActive(!TotalPointsPanel.activeSelf);

        }

    }

}
