using System;
using System.Collections.Generic;
using Formless.Core.Managers;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Tilemaps;

namespace Formless.Room
{
    public class Door : MonoBehaviour
    {
        private static List<(Door, Door)> _doorPairs = new List<(Door, Door)>();

        public static event Action<GameObject, GameObject> OnDoorReplaced;
        //public static event Action<GameObject, Direction> OnKeyRequired;

        [SerializeField] private GameObject wallPrefab;
        private BoxCollider2D _boxCollider2D;
        private BoxCollider2D _interactionCollider;
        public DoorType _doorType;
        [SerializeField] public Direction direction;
        private bool _isProcessed = false;
        private bool _isReplaced = false;
        private bool _isOpened = false;
        private bool _isBossDoorSet = false;
        private Door _linkedDoor;

        public DoorType DoorType => _doorType;
        public Door LinkedDoor => _linkedDoor;

        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _interactionCollider = transform.Find("InteractionTrigger")?.GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            DungeonGenerator.OnDungeonGenerationCompleted += HandleDungeonGenerationCompleted;
            //doorType = DoorType.Regular;
            //if (!_isOpened)
            //{
            //    Invoke("DisableDoorBoxCollider", 2f);
            //}
            //Invoke("CheckIfDoorTouchesLastRoom", 1f);
        }

        private void OnDestroy()
        {
            DungeonGenerator.OnDungeonGenerationCompleted -= HandleDungeonGenerationCompleted;
        }

        private void HandleDungeonGenerationCompleted()
        {
            CheckIfDoorTouchesLastRoom();
        }

        public void OpenDoor(string lockName)
        {
            _isOpened = true;
            _boxCollider2D.enabled = false;
            _interactionCollider.enabled = false;
            DestroyLockAndActivateMover(lockName, _doorType);

            if (_linkedDoor != null)
            {
                _linkedDoor.SyncOpenDoor(lockName);
            }
        }

        private void SyncOpenDoor(string lockName)
        {
            _isOpened = true;
            _boxCollider2D.enabled = false;
            _interactionCollider.enabled = false;
            DestroyLockAndActivateMover(lockName, _doorType);
        }

        public void SetAsBossDoor()
        {
            if (_isBossDoorSet) return;
            _isBossDoorSet = true;
            _doorType = DoorType.Boss;
            if (_linkedDoor != null) _linkedDoor._doorType = DoorType.Boss;
            ReplaceDoor(DoorType.Boss);
            if (_linkedDoor != null) _linkedDoor.ReplaceDoor(DoorType.Boss);
        }

        public void SetKeyRequired()
        {
            _doorType = DoorType.KeyRequired;
            if (_linkedDoor != null) _linkedDoor._doorType = DoorType.KeyRequired;
            ReplaceDoor(DoorType.KeyRequired);
            if (_linkedDoor != null) _linkedDoor.ReplaceDoor(DoorType.KeyRequired);
        }

        public void CheckIfDoorTouchesLastRoom()
        {
            BoxCollider2D _lastRoomCollider = DungeonGenerator.Instance.LastRoom.GetComponent<BoxCollider2D>(); // �������� ��������� ��������� �������


            if (_lastRoomCollider != null && _boxCollider2D != null && !_isBossDoorSet)
            {
                // �������� ����������� � ��������� ��������
                if (_boxCollider2D.IsTouching(_lastRoomCollider))
                {
                    //Debug.Log("����� ������������ � ��������� ��������.");
                    SetAsBossDoor();
                    DisableDoorBoxCollider();
                }
                else
                {
                    //Debug.Log("����� �� ������������ � ��������� ��������.");
                }
            }
            //else
            //{
            //    //Debug.LogWarning("���������� �� �������!");
            //}
        }


        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_isProcessed || _isOpened) return;

            if (collision.CompareTag("Wall"))
            {
                ReplaceDoorWithWall();
                _isProcessed = true;
            }
            else if (collision.CompareTag("Door"))
            {
                _isProcessed = true;
                Door otherDoor = collision.GetComponent<Door>();
                if (otherDoor != null && otherDoor != this)
                {
                    LinkDoors(otherDoor);
                }
            }
        }

        private void LinkDoors(Door otherDoor)
        {
            if (_linkedDoor == null && otherDoor._linkedDoor == null)
            {
                _linkedDoor = otherDoor;
                otherDoor._linkedDoor = this;
                _doorPairs.Add((this, otherDoor));

                //Debug.Log($"������� �����: {this.gameObject.name} <-> {otherDoor.gameObject.name}");
                SyncDoorType();
            }
        }

        private void SyncDoorType()
        {
            if (_linkedDoor == null) return;

            DoorType previousDoorType = _doorType;
            DoorType prevLinkedDoorType = _linkedDoor._doorType;

            // ���� ������� ����� ��� �������, �� ���������������� � ��� � KeyRequired ��� Boss
            if (this._doorType == DoorType.Opened || _linkedDoor._doorType == DoorType.Opened)
            {
                _doorType = DoorType.Opened;
                _linkedDoor._doorType = DoorType.Opened;
            }

            if (this._doorType == DoorType.KeyRequired)
            {
                _linkedDoor._doorType = DoorType.KeyRequired;
            }
            else if (this._doorType == DoorType.Boss)
            {
                _linkedDoor._doorType = DoorType.Boss;
            }

            //Debug.Log($"����������� �����: {direction}");


            if (_doorType != previousDoorType)
            {
                ReplaceDoor(_doorType);
            }

            if (_linkedDoor._doorType != prevLinkedDoorType)
            {
                _linkedDoor.ReplaceDoor(_linkedDoor._doorType);
            }

            //ReplaceDoorIfNecessary(previousDoorType, doorType);

            //Debug.Log($"��� ����� ���������������: {this.gameObject.name} ({this.doorType}) <-> {_linkedDoor.gameObject.name} ({_linkedDoor.doorType})");
        }


        private void ReplaceDoorWithWall()
        {
            Debug.Log("������� ����� �� ������, ������ �� �����");
            Transform room = gameObject.transform.parent;
            if (room.CompareTag("RoomMain")) return;

            if (!_isReplaced && wallPrefab != null)
            {
                DisableDoorBoxCollider();
                Instantiate(wallPrefab, transform.position, Quaternion.identity, transform.parent);
                _isReplaced = true;
                Destroy(gameObject);
            }
        }

        public void TryUnlockDoor()
        {
            if (GameplayManager.Instance.HasBossKey && _doorType == DoorType.Boss)
            {
                OpenDoor("BossLock");
                Player.Player.Instance.UseBossKey();
                DestroyLockAndActivateMover(LockConstants.BOSS_LOCK, _doorType);
                Debug.Log("����� ����� �������");
            }
            else if (GameplayManager.Instance.HasKey() && _doorType == DoorType.KeyRequired)
            {
                OpenDoor("Lock");
                Player.Player.Instance.UseKey();
                DestroyLockAndActivateMover(LockConstants.LOCK, _doorType);
                Debug.Log("������� ����� �������");
            }
            else
            {
                Debug.Log("� ������ ��� ������");
            }
        }

       private void DestroyLockAndActivateMover(string lockName, DoorType doorType)
        {
            Transform lockObject = transform.Find(lockName) ?? transform.Find(lockName + "(Clone)");
            if (lockObject != null)
            {
                // �� ������ ���������� ���� ������ ��� ����������
                GameObject lockDestroyEffect = PrefabManager.Instance.LockDestroyEffect;

                if (lockDestroyEffect != null)
                {
                    // ������������� ���� ������� ���������� � ����������� �� ���� �����
                    SetLockDestroyEffectColor(lockDestroyEffect, doorType);

                    // ������� ������ ���������� � ������� �����
                    Instantiate(lockDestroyEffect, lockObject.position, Quaternion.identity);
                }

                // ���������� ������ �����
                Destroy(lockObject.gameObject);
            }

            // ������ ������ � �������� ���������� � ���������� ���
            Transform moverObject = transform.Find("Mover");
            if (moverObject != null)
            {
                moverObject.gameObject.SetActive(true);
            }

            // ��������� ��������� Tilemap
            TilemapCollider2D collider = GetComponent<TilemapCollider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }
        }

        private void SetLockDestroyEffectColor(GameObject destroyEffect, DoorType doorType)
        {
            // �������� ��������� ParticleSystem
            ParticleSystem particleSystem = destroyEffect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                // �������� �������� ������ ������� ������
                var main = particleSystem.main;
        
                // ������������� ���� � ����������� �� ���� �����
                switch (doorType)
                {
                    case DoorType.Regular:
                        main.startColor = Color.green; // ������� ��� �������� �����
                        break;

                    case DoorType.KeyRequired:
                        main.startColor = Color.yellow; // ������ ��� ����� � ������
                        break;

                    case DoorType.Boss:
                        main.startColor = Color.red; // ������� ��� ����� �����
                        break;
                }
            }
        }

        private void DisableDoorBoxCollider()
        {
            if (_boxCollider2D != null)
            {
                _boxCollider2D.enabled = false;
            }
        }

        public void ReplaceDoorIfNecessary(DoorType oldDoorType, DoorType newDoorType)
        {
            if (oldDoorType != newDoorType)
            {
                ReplaceDoor(newDoorType);
            }
        }

        private void ReplaceDoor(DoorType newDoorType)
        {
            GameObject newDoorPrefab;
            if (!_isReplaced)
            {
                DisableDoorBoxCollider();

                newDoorPrefab = PrefabManager.Instance.GetDoorPrefab("SandStyle", newDoorType, direction);

                if (newDoorPrefab != null)
                {
                    GameObject oldDoorObject = this.gameObject;
                    GameObject newDoorObject = Instantiate(newDoorPrefab, transform.position, Quaternion.identity, transform.parent);
                    Door newDoor = newDoorObject.GetComponent<Door>();

                    if (_linkedDoor != null)
                    {
                        newDoor.LinkDoors(_linkedDoor);
                    }

                    OnDoorReplaced?.Invoke(oldDoorObject, newDoorObject);
                }

                _isReplaced = true;
                Destroy(gameObject);
            }
        }

    }
}
