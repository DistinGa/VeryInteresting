using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour, IContinuousTask
{
    public int DiceResult;

    public bool isDone
    {
        get
        {
            return DiceResult > 0;
        }
    }

    public void RollDice()
    {
        DiceResult = Random.Range(1, 7);
    }
}
