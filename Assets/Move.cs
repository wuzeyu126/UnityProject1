using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float duration = 0.2f;
    float elapsedTime;
    Vector3 destination;
    Vector3 origin;

    void Start()
    {
        destination = transform.position;
        origin = destination;
    }

    void Update()
    {
        if (transform.position == destination)
        {
            return;
        }

        elapsedTime += Time.deltaTime;
        float timeRatio = elapsedTime / duration;
        float easing = timeRatio;
        Vector3 currentPosition = Vector3.Lerp(origin, destination, easing);
        transform.position = currentPosition;
    }



    public void MoveTo(Vector3 destination)
    {
        elapsedTime = 0;
        origin = this.destination;
        transform.position = origin;
        this.destination = destination;
    }
}