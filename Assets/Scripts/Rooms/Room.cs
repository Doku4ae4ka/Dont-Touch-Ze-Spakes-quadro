using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Room : MonoBehaviour
{
    [SerializeField] protected SpikeSpawner _spikeSpawner;
    [SerializeField] protected Birb _birb;

}
