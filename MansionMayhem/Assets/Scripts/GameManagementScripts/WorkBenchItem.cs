using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkBenchItem : MonoBehaviour
{
    [SerializeField]
    private int cost;

    [SerializeField]
    private Unlock unlockVar;

    public int Cost
    {
        get { return cost; }
    }

    public Unlock UnlockVar
    {
        get { return unlockVar; }
    }
}
