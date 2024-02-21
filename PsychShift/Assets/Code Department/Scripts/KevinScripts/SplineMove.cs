using System.Collections;
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
}
