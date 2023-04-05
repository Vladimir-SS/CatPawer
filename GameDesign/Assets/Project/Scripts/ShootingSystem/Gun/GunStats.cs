using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStats : MonoBehaviour
{
    [field: SerializeField] public float FireRateBase { get; private set; } = 0f;
    [field: SerializeField] public float ReloadSpeedBase { get; private set; } = 0f;

    [field: SerializeField] public uint MagazineCapacityBase { get; private set; } = 0;
}
