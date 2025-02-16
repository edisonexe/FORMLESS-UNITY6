using System;
using System.Collections.Generic;
using UnityEngine;

namespace Formless.Room
{
    [CreateAssetMenu(fileName = "NewDoorStyle", menuName = "Scriptable Objects/DoorStyle")]
    public class DoorStyle : ScriptableObject
    {
        [SerializeField] private string _name;

        // Префабы для каждого типа двери и направления
        [Header("Boss Doors")]
        [SerializeField] private GameObject _bossTopDoor;
        [SerializeField] private GameObject _bossBottomDoor;
        [SerializeField] private GameObject _bossLeftDoor;
        [SerializeField] private GameObject _bossRightDoor;

        [Header("Regular Doors")]
        [SerializeField] private GameObject _regularTopDoor;
        [SerializeField] private GameObject _regularBottomDoor;
        [SerializeField] private GameObject _regularLeftDoor;
        [SerializeField] private GameObject _regularRightDoor;

        [Header("Key Required Doors")]
        [SerializeField] private GameObject _keyRequiredTopDoor;
        [SerializeField] private GameObject _keyRequiredBottomDoor;
        [SerializeField] private GameObject _keyRequiredLeftDoor;
        [SerializeField] private GameObject _keyRequiredRightDoor;

        [Header("Opened Doors")]
        [SerializeField] private GameObject _openedTopDoor;
        [SerializeField] private GameObject _openedBottomDoor;
        [SerializeField] private GameObject _openedLeftDoor;
        [SerializeField] private GameObject _openedRightDoor;

        private Dictionary<(DoorType, Direction), GameObject> _doors;

        public string Name => _name;

        public void Initialize()
        {
            _doors = new Dictionary<(DoorType, Direction), GameObject>();

            // Заполнение словаря с помощью явных полей
            // Boss Doors
            _doors[(DoorType.Boss, Direction.Top)] = _bossTopDoor;
            _doors[(DoorType.Boss, Direction.Bottom)] = _bossBottomDoor;
            _doors[(DoorType.Boss, Direction.Left)] = _bossLeftDoor;
            _doors[(DoorType.Boss, Direction.Right)] = _bossRightDoor;

            // Regular Doors
            _doors[(DoorType.Regular, Direction.Top)] = _regularTopDoor;
            _doors[(DoorType.Regular, Direction.Bottom)] = _regularBottomDoor;
            _doors[(DoorType.Regular, Direction.Left)] = _regularLeftDoor;
            _doors[(DoorType.Regular, Direction.Right)] = _regularRightDoor;

            // Key Required Doors
            _doors[(DoorType.KeyRequired, Direction.Top)] = _keyRequiredTopDoor;
            _doors[(DoorType.KeyRequired, Direction.Bottom)] = _keyRequiredBottomDoor;
            _doors[(DoorType.KeyRequired, Direction.Left)] = _keyRequiredLeftDoor;
            _doors[(DoorType.KeyRequired, Direction.Right)] = _keyRequiredRightDoor;

            // Opened Doors
            _doors[(DoorType.Opened, Direction.Top)] = _openedTopDoor;
            _doors[(DoorType.Opened, Direction.Bottom)] = _openedBottomDoor;
            _doors[(DoorType.Opened, Direction.Left)] = _openedLeftDoor;
            _doors[(DoorType.Opened, Direction.Right)] = _openedRightDoor;

            // Логирование
            Debug.Log($"DoorStyle '{_name}' initialized with {_doors.Count} doors.");
        }

        public GameObject GetDoor(DoorType type, Direction direction)
        {
            Debug.Log($"Ищем дверь: Тип = {type}, Направление = {direction}");

            if (_doors.TryGetValue((type, direction), out var door))
            {
                Debug.Log($"Найдена дверь: {door?.name ?? "null"}");
                return door;
            }

            Debug.LogWarning($"Дверь не найдена для: Тип = {type}, Направление = {direction}");
            return null;
        }
    }
}
