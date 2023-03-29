using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public RoomParameters starterRoom;
    public List<RoomParameters> placeablePrefabs;
    private List<RoomPlacingData> placingData;
    
    void Start()
    {
        RoomPlacingData r;
       // roomPlaceData = BuildLayout(starterRoom, placeablePrefabs);
    }

    private void GetLayout()
    {
        // ??
    }
}
