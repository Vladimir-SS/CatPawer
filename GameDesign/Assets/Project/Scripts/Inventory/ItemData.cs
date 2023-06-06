using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    [field: SerializeField] public string Name { get; protected set; }
    [field: SerializeField] public string Description { get; protected set; }
    [field: SerializeField] public Sprite Image { get; protected set; }

}
