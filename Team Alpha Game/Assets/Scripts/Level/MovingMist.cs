using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMist : MonoBehaviour
{
    public List<GameObject> waypoints;
    public List<float> mistSpeed;
    public List<Vector3> mistscale;
    public float tolerance = 0.1f;

    private int currentWP = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (waypoints.Count != 0 && mistSpeed.Count == waypoints.Count)
        {
            this.transform.position = waypoints[currentWP].transform.position;
            NextWaypoint();
        }
        else
        {
            Debug.LogError("Waypoints improperly initialized");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWP < waypoints.Count)
        {
            MoveMist();
        }
    }

    void MoveMist()
    {
        Debug.Log(currentWP);
        Debug.Log(CurrentPosDiff().magnitude);
        if(CurrentPosDiff().magnitude <= tolerance)
        {
            NextWaypoint();
        }
        else
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position,
                                                          waypoints[currentWP].transform.position,
                                                          mistSpeed[currentWP-1]*Time.deltaTime);
        }
    }

    Vector2 CurrentPosDiff()
    {
        float xVal = waypoints[currentWP].transform.position.x - this.transform.position.x;
        float yVal = waypoints[currentWP].transform.position.y - this.transform.position.y;
        return new Vector2(xVal, yVal);
    }

    void NextWaypoint()
    {
        if(currentWP < waypoints.Count)
        {
            currentWP = currentWP + 1;
        }
    }
}
