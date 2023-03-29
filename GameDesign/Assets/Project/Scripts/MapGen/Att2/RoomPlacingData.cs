using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlacingData : MonoBehaviour
{
    private RoomPivot _pivot;
    private int _rotation;

    public RoomPlacingData(DoorMarker doorMarker, int rotation)
    {
        this.Rotation = rotation;
        this.Pivot = new RoomPivot(doorMarker);
    }

    public class RoomPivot
    {
        //when placing in scene: get the correct door with the id, move door to pivot position, instantiate prefab then rotate
        private int _id;
        private Vector3 _position;
        
        public RoomPivot(DoorMarker doorMarker)
        {
            this._id = doorMarker.GetInstanceID();

            this._position = new Vector3(doorMarker.transform.position.x, doorMarker.transform.position.y, doorMarker.transform.position.z);
        }

        public int ID
        {
            get => _id;
        }

        public Vector3 Position
        {
            get => _position;
        }
    }

    // to make things mre confusing for all parties involved
    public RoomPivot Pivot
    {
        get => _pivot;
        set
        {
            _pivot = value;
        }
    }

    public int Rotation
    {
        get => _rotation;
        set
        {
            if ((value % 90 == 0) && (value < 360))
            {
                _rotation = value;
            }
        }
    }
}
