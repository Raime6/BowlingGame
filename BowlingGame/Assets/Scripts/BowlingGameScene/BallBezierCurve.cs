using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor;
//using UnityEditor.Build.Reporting;
using UnityEngine;

public class BallBezierCurve : MonoBehaviour
{

    [SerializeField] Transform Ball;                // Ball associated with this curve
    [SerializeField] LineRenderer BallLine;         // Ball's Line (Shown when playing)
    private Transform tr;                           // Origin Point's Transform
    [SerializeField] Transform DestinyTr;           // Destiny Point's Transform
    [SerializeField] Transform ControlPointTr;      // Control Point's Transform
    [SerializeField] int numLineSegments = 40;      // Number of points which conform the line
    [SerializeField] Color color;                   // Line's color
    private float segment;                          // Number of Segment inverse
    //[SerializeField] int speedReductionFactor = 70;

    // Start is called before the first frame update
    void Start()
    {

        tr = transform;

    }

    private void OnDrawGizmos()
    {

        if (!DestinyTr)
            return;

        // Must be at least one segment
        if(numLineSegments < 1)
            numLineSegments = 1;

        tr = transform;                           // Sets the transform
        segment = 1.0f / numLineSegments;         // Calculates the segment
        BallLine.positionCount = numLineSegments; // Sets the number of Line's positions

        Vector3 currentSegmentOriginPoint;                                  // Origin point of the current segment
        float x, y, z;                                                      // Needed to build the current segment's destiny point
        float segmentAux;                                                   // Will increase in every for step with an equal value to the segment
        float verticalDistance = DestinyTr.position.y - tr.position.y;      // Vertical distance between the origin and destiny point
        float vertSegment = verticalDistance / numLineSegments;             // Vertical increment when drawing each segment
        int contLinePositions = 0;                                          // Needed for accessing to the positions of the LineRenderer

        currentSegmentOriginPoint = tr.position;
        BallLine.SetPosition(contLinePositions, Vector3.zero);              // Sets the first position of the LineRenderer
        y = tr.position.y;
        Gizmos.color = color;

        // Each segment is drawn in a for
        for(segmentAux = segment; segmentAux <= 1; segmentAux += segment)
        {

            x = (1 - segmentAux) * (1 - segmentAux) * tr.position.x + 2 * (1 - segmentAux) * segmentAux * ControlPointTr.position.x + segmentAux * segmentAux * DestinyTr.position.x;
            y = DestinyTr.position.y;
            z = (1 - segmentAux) * (1 - segmentAux) * tr.position.z + 2 * (1 - segmentAux) * segmentAux * ControlPointTr.position.z + segmentAux * segmentAux * DestinyTr.position.z;
            Gizmos.DrawLine(currentSegmentOriginPoint, new Vector3(x, y, z));

            // LineRenderer
            if(contLinePositions <= 38)
            {

                contLinePositions++;
                if (contLinePositions == 39) // Last position
                    BallLine.SetPosition(contLinePositions, new Vector3(-20, 0, -(Ball.position.z - z)));
                else                         // Rest of positions
                    BallLine.SetPosition(contLinePositions, new Vector3(-(currentSegmentOriginPoint.x - x), 0, -(Ball.position.z - z)));

            }

        }

    }

    public IEnumerator Move(Transform t, float speed)
    {

        // Animation curve for the trajectory with the three axes
        AnimationCurve trajectoryX;
        AnimationCurve trajectoryY;
        AnimationCurve trajectoryZ;

        int numSegments = 12;                                          // Must be at least one segment
        float segment = 1.0f / numSegments;                            // Calculates the segment
        float x, y, z;                                                 // Needed to build the current segment's destiny point
        float segmentAux;                                              // Will increase in every for step with an equal value to the segment
        float verticalDistance = DestinyTr.position.y - tr.position.y; // Vertical distance between the origin and destiny point
        float vertSegment = verticalDistance / numSegments;            // Vertical increment when drawing each segment

        // Key frames for each frame
        Keyframe[] kfx = new Keyframe[1];
        kfx[0] = new Keyframe(0, tr.position.x);
        trajectoryX = new AnimationCurve(kfx);

        Keyframe[] kfy = new Keyframe[1];
        kfy[0] = new Keyframe(0, tr.position.y);
        trajectoryY = new AnimationCurve(kfy);

        Keyframe[] kfz = new Keyframe[1];
        kfz[0] = new Keyframe(0, tr.position.z);
        trajectoryZ = new AnimationCurve(kfz);

        y = tr.position.y; // Initializes Y

        // Sets the keyframes for each curve
        for(segmentAux = segment; segmentAux <= 1; segmentAux += segment)
        {

            x = (1 - segmentAux) * (1 - segmentAux) * tr.position.x + 2 * (1 - segmentAux) * segmentAux * ControlPointTr.position.x + segmentAux * segmentAux * DestinyTr.position.x;
            z = (1 - segmentAux) * (1 - segmentAux) * tr.position.z + 2 * (1 - segmentAux) * segmentAux * ControlPointTr.position.z + segmentAux * segmentAux * DestinyTr.position.z;
            y += verticalDistance;

            trajectoryX.AddKey(new Keyframe(segmentAux, x));
            trajectoryY.AddKey(new Keyframe(segmentAux, y));
            trajectoryZ.AddKey(new Keyframe(segmentAux, z));

        }

        // Draws a last segment if it's necessary, in case the segment's value in the last step of the for was greater than 1 (depends on the segment)
        if(segmentAux > 1)
        {

            segmentAux = 1;

            x = (1 - segmentAux) * (1 - segmentAux) * tr.position.x + 2 * (1 - segmentAux) * segmentAux * ControlPointTr.position.x + segmentAux * segmentAux * DestinyTr.position.x;
            z = (1 - segmentAux) * (1 - segmentAux) * tr.position.z + 2 * (1 - segmentAux) * segmentAux * ControlPointTr.position.z + segmentAux * segmentAux * DestinyTr.position.z;
            y = DestinyTr.position.y;

            trajectoryX.AddKey(new Keyframe(segmentAux, x));
            trajectoryY.AddKey(new Keyframe(segmentAux, y));
            trajectoryZ.AddKey(new Keyframe(segmentAux, z));

        }

        // Curve smoothing
        for(int i = 0; i < trajectoryX.length; i++)
        {

            trajectoryX.SmoothTangents(i, 0);
            trajectoryY.SmoothTangents(i, 0);
            trajectoryZ.SmoothTangents(i, 0);

        }

        float path = 0; // Initializes the path
        Rigidbody rb = t.GetComponent<Rigidbody>(); // Gets the Ball's rigidbody

        rb.AddRelativeForce(new Vector3(-transform.position.x * speed, 0, 0), ForceMode.Impulse);

        // Sets the Ball's position through the path
        while (path <= 1)
        {

            // Calculates the new position evaluating the three curves
            // Moves the rigidbody
            rb.MovePosition(new Vector3(trajectoryX.Evaluate(path), trajectoryY.Evaluate(path), trajectoryZ.Evaluate(path)));

            // Increases the path;
            path += Time.fixedDeltaTime * speed;
            yield return new WaitForFixedUpdate();

        }

    }

}
