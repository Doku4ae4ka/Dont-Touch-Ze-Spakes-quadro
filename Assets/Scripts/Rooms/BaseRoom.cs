using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRoom : Room
{
    public int Score { get; set; }
    public event Action OnScoreChanged;

    private void OnEnable()
    {
        _birb.OnWallHit += IncreaseScore;

    }

    private void OnDisable()
    {
        _birb.OnWallHit -= IncreaseScore;
    }

    private void IncreaseScore()
    {
        Score++;
        OnScoreChanged?.Invoke();
    }

    private void ChangeSpikesNumber()
    {
    }

}
