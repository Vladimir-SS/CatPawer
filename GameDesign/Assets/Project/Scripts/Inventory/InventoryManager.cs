using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance;

    public List<GameObject> items = new List<GameObject>();

    [SerializeField] private GameObject player;

    [SerializeField] private GameObject inventoryItem;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform inventoryContent;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this; 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel.activeSelf)
        {
            ClearItems();
            inventoryPanel.SetActive(false);
        }
        else
        {
            inventoryPanel.SetActive(true);
            ShowItems();
        }
    }

    private void ShowItems()
    {
        items = player.GetComponent<InventoryHandler>().GetItems();
        ItemData data;

        foreach (GameObject item in items)
        {
            data = item.GetComponent<ItemData>();

            GameObject newItem = Instantiate(inventoryItem, inventoryContent);

            newItem.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = data.Name;
            newItem.transform.Find("Image").GetComponent<Image>().sprite = data.Image;

        }
    }

    private void ClearItems()
    {
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }
    }
}
