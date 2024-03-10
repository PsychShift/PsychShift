/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineMove : MonoBehaviour
{
    public SplineContainer spline;
    public float speed = 1f;
    public Transform playerDistance;
    float maxSpeed;
    float distancePercentage = 0f;
    float splineLength;
    // Start is called before the first frame update
    void Start()
    {
        splineLength = spline.CalculateLength();
        maxSpeed = speed;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Make a function editing speed as the player transform gets closer 
        //speed = Mathf.Lerp(baseSpeed, maxSpeed, 1f - Mathf.Clamp01(distanceToPlayer / minDistance));

        distancePercentage += speed * Time.deltaTime / splineLength;

        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;

        if(distancePercentage > 1f)
        {
            distancePercentage = 0f;
        }

        Vector3 nextPosition = spline.EvaluatePosition(distancePercentage + 0.05f);
        Vector3 direction = nextPosition - currentPosition;
        transform.rotation = Quaternion.LookRotation(direction, transform.up);

    }
} */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineMove : MonoBehaviour
{
    public SplineContainer spline;
    public Transform playerDistance;
    public float minDistance = 5f;
    public float maxDistance = 100f;
    public float maxSpeed = 5f;
    public float minSpeed = 1f;

    private float currentSpeed;
    private float splineLength;
    private float distancePercentage = 0f;

    void Start()
    {
        splineLength = spline.CalculateLength();
        currentSpeed = maxSpeed;
    }

    void Update()
    {
        // Calculate distance between player and object following the spline
        float distanceToPlayer = Vector3.Distance(transform.position, playerDistance.position);
        float normalizedValue = (distanceToPlayer - minDistance) / (maxDistance - minDistance);


        // Calculate speed based on player distance
        currentSpeed = Mathf.Lerp(maxSpeed, minSpeed,  normalizedValue);//Player, min, max / max distance

        // Calculate distance to move along the spline
        float distanceToMove = currentSpeed * Time.deltaTime / splineLength;

        // Update the distance percentage along the spline
        distancePercentage = (distancePercentage + distanceToMove) % 1f;

        // Evaluate position on the spline and move object
        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;

        // Rotate object along the spline
        Vector3 nextPosition = spline.EvaluatePosition((distancePercentage + 0.05f) % 1f);
        Vector3 direction = nextPosition - currentPosition;
        transform.rotation = Quaternion.LookRotation(direction, transform.up);
    }
}
