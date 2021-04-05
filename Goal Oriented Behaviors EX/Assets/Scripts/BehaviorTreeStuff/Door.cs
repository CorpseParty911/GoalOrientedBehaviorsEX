using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isClosed = false;
    public bool isLocked = false;

    Vector3 closedRotation = new Vector3(0, 0, 0);
    Vector3 openRotation = new Vector3(0, -135, 0);

    void Start()
    {
        if (isClosed)
        {
            transform.eulerAngles = closedRotation;
        }
        else
        {
            transform.eulerAngles = openRotation;
        }
    }

    public bool Open()
    {
        if (isClosed && !isLocked)
        {
            isClosed = false;
            transform.eulerAngles = openRotation;
            return true;
        }
        return false;
    }

    public bool Close()
    {
        if (!isClosed)
        {
            transform.eulerAngles = closedRotation;
            isClosed = true;
        }
        return true;
    }

    public void ToggleDoorOpen()
    {
        if (!isClosed)
        {
            isClosed = true;
            transform.eulerAngles = closedRotation;
        }
        else
        {
            isClosed = false;
            transform.eulerAngles = openRotation;
        }
    }

    public void ToggleDoorLocked()
    {
        if (!isLocked)
        {
            isLocked = true;
        }
        else
        {
            isLocked = false;
        }
    }
}