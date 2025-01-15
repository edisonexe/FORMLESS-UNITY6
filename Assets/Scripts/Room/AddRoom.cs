using UnityEngine;
using System.Collections.Generic;

public class AddRoom : MonoBehaviour
{
    private RoomVariants _roomVariants;

    private void Start()
    {
        _roomVariants = GameObject.FindGameObjectWithTag("RoomVariants").GetComponent<RoomVariants>();
        _roomVariants.rooms.Add(this.gameObject);
    }
}
