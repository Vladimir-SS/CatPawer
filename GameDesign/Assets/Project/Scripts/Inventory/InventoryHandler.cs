using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    //TODO: perhaps more guns and press 1,2,3 to switch between them ? like in our description

    [SerializeField] private GameObject GunPlace;
    public GameObject Gun { get; private set; } = null;
    [SerializeField] private GameObject StartWithThisGun;
        
    [SerializeField] private GameObject ItemsPlace;
    [SerializeField] private List<GameObject> StartingItems;
    [SerializeField] private List<GameObject> gunPrefabs;

    private EventSubmission eventSubmission;

    public void ChangeGun(GameObject gun)
    {
        if (Gun != null)
        {
            Destroy(Gun);
        }

        if (gun != null)
        {
            Gun = Instantiate(gun, GunPlace.transform);
            var HoldingAnchor = Gun.transform.Find("HoldingAnchor");

            if (HoldingAnchor != null)
            {
                Vector3 delta = GunPlace.transform.position - HoldingAnchor.position;
                Gun.transform.position += delta;
            }
        }

        GunSwitchEventArgs gunSwitchEventArgs = new(Gun);
        eventSubmission.TriggerGunSwitchEvent(this, gunSwitchEventArgs);
    }

    public void AddItem(GameObject itemPrefab)
    {
        Instantiate(itemPrefab, ItemsPlace.transform);


        //TODO: event sub??
    }

    public List<GameObject> GetItems()
    {
        List<GameObject> rv = new();

        foreach (Transform child in ItemsPlace.transform)
        {
            rv.Add(child.gameObject);
        }

        return rv;
    }

    private void Start()
    {
        foreach (GameObject item in StartingItems)
        {
            AddItem(item);
        }

        eventSubmission = GetComponent<EventSubmission>();
        ChangeGun(StartWithThisGun);
        if (MainManager.Instance != null)
        {
            switch (MainManager.Instance.weapon)
            {
                case "Glock":
                    ChangeGun(gunPrefabs[0]);
                    break;
                case "Minigun":
                    ChangeGun(gunPrefabs[1]);
                    break;
                case "Bazooka":
                    ChangeGun(gunPrefabs[2]);
                    break;
                case "50Cal":
                    ChangeGun(gunPrefabs[3]);
                    break;
                default:
                    break;
            }
        }
    }

}
