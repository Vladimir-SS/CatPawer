using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ItemEquip : MonoBehaviour
{
    [SerializeField] private List<GameObject> AllItems;

    Random random = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject.transform.parent.gameObject;
            player.GetComponent<InventoryHandler>().AddItem(AllItems[random.Next(AllItems.Count)]);

            Destroy(transform.gameObject);
        }
    }
}
