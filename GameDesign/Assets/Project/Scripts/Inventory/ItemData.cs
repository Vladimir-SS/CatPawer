using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private GameObject prefab;

    public string ItemName
    {
        get => itemName;
    }

    public string Description
    {
        get => description;
    }

    public Sprite ItemSprite
    {
        get => icon;
    }

    public GameObject Prefab
    {
        get => prefab;
    }
}
