using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneController : MonoBehaviour
{
    [SerializeField] GameController _gameController; // Game Controller to use the game's methods
    public GameObject BowlingPicker;                 // Bowling Picker to control its animations
    public GameObject SweeperVagon;                  // Sweeper Vagon to control its animations
    public GameObject BowlingBallPrefab;             // Ball's prefab
    public Rigidbody BowlingBallRB;                  // Bowling Ball's RigidBody
    public Vector3 BallInitialPosition;              // Ball's Initial position when creating it
    public Quaternion BowlingBallOriginalRotation;   // Original orientation of the ball
    [SerializeField] GameObject Dummy;               // Father of the Pin, is the one whose transform is modified. Necessary to be able to move vertically the Pins correctly
    [SerializeField] GameObject BowlingPin;          // Bowling Pin Prefab
    [SerializeField] List<Material> PinMaterials;    // Pin's different materials, modified in the script so there is more variety
    private List<float> PinPositionX;                // Coord X of the Pin
    private float PinPositionY;                      // Coord Y of the Pin
    private List<float> PinPositionZ;                // Coord Z of the Pin
    public bool StartGame;                           // If true, it's the beggining of the game so the flow of animation it's different only this time
    public bool SweepEnded;                          // If true, sweep has ended so the animation flow changes
    public bool AnimationOn;                         // If true, an animation is playing
    public int CurrentTry;                           // Actual try of the frame, each Frame consists on two Tries, except the 10th Frame in which Tries are up to three (value between 1 and 3)
    public List<GameObject> ListPins;                // List of Pins that hasn't fallen in the current try
    public int NumPinsFallen;                        // Conts the num of fallen Pins to select what animation is going to play
    

    // Builder
    private void Awake()
    {

        BallInitialPosition = new Vector3(9.558f, 0.2100252f, this.transform.position.z);
        PinPositionX = new List<float> { -9.011f, -9.161f, -9.161f, -9.311f, -9.311f, -9.311f, -9.461f, -9.461f, -9.461f, -9.461f };
        PinPositionY = 0.1415551f;
        PinPositionZ = new List<float> { this.transform.position.z, this.transform.position.z + 0.15f, this.transform.position.z - 0.15f, this.transform.position.z + 0.3f, this.transform.position.z, this.transform.position.z - 0.3f, this.transform.position.z + 0.45f, this.transform.position.z + 0.15f, this.transform.position.z - 0.15f, this.transform.position.z - 0.45f };
        StartGame = true;
        SweepEnded = false;
        AnimationOn = false; // There's no animation playing
        CurrentTry = 1;
        ListPins = new List<GameObject> { };
        NumPinsFallen = 0;

    }

    // Start is called before the first frame update
    void Start()
    {

        CreateBall(BallInitialPosition);
        CreatePins();
        SweeperVagon.GetComponent<SweepControllerLight>().SweepEnd();

    }

    private void Update()
    {

        StartAnimations();

    }

    /* Creates the ball */
    public void CreateBall(Vector3 position)
    {

        // Creates the ball
        GameObject clon = Instantiate(BowlingBallPrefab);
        BowlingBallRB = clon.GetComponent<Rigidbody>();
        BowlingBallOriginalRotation = BowlingBallRB.transform.rotation;        // Original orientation of the ball
        clon.GetComponent<BallControllerLight>()._laneController = this;
        clon.GetComponent<BallControllerLight>().PinsHorizontalPosition = PinPositionZ;
        clon.transform.position = position;                                    // Places the ball

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
                clonPin.GetComponent<PinController>()._laneController = this;

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
            SweeperVagon.GetComponent<SweepControllerLight>().MoveDownStart();

    }

    /* Resets shooting unless the 10th Frame has ended */
    public IEnumerator ResetShoot()
    {
        
        // Reset Ball's variables to enable shooting again
        BowlingBallRB.useGravity = false;
        BowlingBallRB.transform.position = BallInitialPosition;
        BowlingBallRB.transform.rotation = BowlingBallOriginalRotation;

        // Stop Ball's movement
        BowlingBallRB.isKinematic = true;
        BowlingBallRB.isKinematic = false;

        // Resets ball's Bezier Central Point
        BowlingBallRB.transform.GetChild(1).transform.position = new Vector3(BowlingBallRB.transform.GetChild(1).transform.position.x, BowlingBallRB.transform.GetChild(1).transform.position.y, BowlingBallRB.transform.position.z);

        // Reset the pins
        DestroyPins();
        CreatePins();

        // Waits a random time so the ball of the Lanes are not shoot at the same time
        float randWait = Random.Range(0, 20);
        yield return new WaitForSeconds(randWait);

        // Shoots the ball with a random difficulty to create variety of shoots
        int randShoot = Random.Range(1, 4);
        StartCoroutine(BowlingBallRB.GetComponent<BallControllerLight>().CpuShoot(randShoot));

    }

}
