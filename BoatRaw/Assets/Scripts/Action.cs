using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameSpace;


public class Moveable : MonoBehaviour
{

    readonly float move_speed = 20;

    // change frequently
    int moving_status;  // 0->not moving, 1->moving to middle, 2->moving to dest
    Vector3 dest;
    Vector3 middle;

    void Update()
    {
        if (moving_status == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, middle, move_speed * Time.deltaTime);
            if (transform.position == middle)
            {
                moving_status = 2;
            }
        }
        else if (moving_status == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, dest, move_speed * Time.deltaTime);
            if (transform.position == dest)
            {
                moving_status = 0;
            }
        }
    }
    public void setDestination(Vector3 _dest)
    {
        dest = _dest;
        middle = _dest;
        if (_dest.y == transform.position.y)
        {   // boat moving
            moving_status = 2;
        }
        else if (_dest.y < transform.position.y)
        {   // character from coast to boat
            middle.y = transform.position.y;
        }
        else
        {                               
            // character from boat to coast
            middle.x = transform.position.x;
        }
        moving_status = 1;
    }

    public void reset()
    {
        moving_status = 0;
    }
}


