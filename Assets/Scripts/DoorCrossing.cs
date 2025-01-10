using UnityEngine;

public class DoorCrossing : MonoBehaviour
{
    [SerializeField] private GameObject _leftWallPrefab;  // ������ ����� ��� ����� �����
    [SerializeField] private GameObject _rightWallPrefab; // ������ ����� ��� ����� ������

    [SerializeField] private bool _isLeftDoor; // ���� ��� ����������� ������� �����

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            ReplaceWithWall();
        }
    }

    private void ReplaceWithWall()
    {
        // �������� ������ � ����������� �� ������� �����
        GameObject selectedWallPrefab = _isLeftDoor ? _leftWallPrefab : _rightWallPrefab;

        // ������ ��������� ����� �� ����� �����
        Instantiate(selectedWallPrefab, transform.position, Quaternion.identity);

        // ������� ������� ������ (�����)
        Destroy(gameObject);
    }
}

