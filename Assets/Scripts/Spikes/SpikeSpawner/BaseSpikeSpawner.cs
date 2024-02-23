using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class BaseSpikeSpawner : SpikeSpawner, ICanMoveEntities
{
    [SerializeField] private List<Transform> _spikesList;
    [SerializeField] private Birb _birb;
    private Dictionary<Transform, bool> _spikesMap;
    private bool isRightWall = true;
    private bool isForward;

    public int SpikesNumber { get; set; }

    private void OnEnable()
    {
        _birb.OnLeftWallHit += ChangeWall;
        _birb.OnRightWallHit += ChangeWall;
        _birb.OnBirdDeath += OnDeath;
    }

    private void OnDeath()
    {
        enabled = false;
    }

    private void OnDisable()
    {
        _birb.OnLeftWallHit -= ChangeWall;
        _birb.OnRightWallHit -= ChangeWall;
    }

    private void Start()
    {
        _spikesMap = new Dictionary<Transform, bool>();
        SpikesNumber = 5;

        foreach (Transform spike in _spikesList)
            _spikesMap.Add(spike, false);
    }

    public void Move(Transform entity)
    {
        if(isRightWall && isForward)
        {
            entity.DOLocalMoveX(-0.19f, 0.1f);
        }
        else if (isRightWall && !isForward)
        {
            entity.DOLocalMoveX(-0.25f, 0.1f);
        }
        else if (!isRightWall && isForward)
        {
            entity.DOLocalMoveX(0.19f, 0.1f);
        }
        else
        {
            entity.DOLocalMoveX(0.25f, 0.1f);
        }
    }

    public void SpawnWall(int spikesNumber)
    {

        for (int i = 0; i < spikesNumber; i++)
        {
            int rndIndex = GetRandomRandomIndex(isRightWall);
            Transform crntSpike = _spikesList[rndIndex];
            if (!_spikesMap[crntSpike])
            {
                _spikesMap[crntSpike] = true;
                isForward = true;
                Move(crntSpike);
            }
            else
            {
                i--;
            }
        }
    }

    public void DespawnWall()
    {
        for (int i = 0; i < GetLastIndex(isRightWall); i++)
        {
            Transform crntSpike = _spikesList[i];
            if (_spikesMap[crntSpike])
            {
                _spikesMap[crntSpike] = false;
                isForward = false;
                Move(crntSpike);
            }
        }
    }

    private void ChangeWall(bool isRight)
    {
        SetCurrentWall(isRight);
        DespawnWall();
        SpawnWall(SpikesNumber);
    }

    private int GetRandomRandomIndex(bool isRight)
    {
        return isRight ? UnityEngine.Random.Range(0, 9) : UnityEngine.Random.Range(10, 19);
    }

    private int GetLastIndex(bool isRight)
    {
        return isRight ? 19 : 9;
    }

    private void SetCurrentWall(bool isRight) => isRightWall = isRight;
}
