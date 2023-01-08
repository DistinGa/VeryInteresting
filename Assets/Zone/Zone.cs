using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public ZoneTypes ZoneType;

    int ChipsOnBoard = 0;

    public Vector3 GetPosition()
    {
        return transform.GetChild(ChipsOnBoard++).position;
    }

    public void FreePosition()
    {
        ChipsOnBoard--;
    }
}

public enum ZoneTypes
{
    Regular,
    Bonus,
    Penalty
}