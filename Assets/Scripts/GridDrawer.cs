using UnityEngine;

public class GridDrawer : MonoBehaviour
{
    public Vector2 centerPoint = new Vector2(0, 0); // ���������� ��������� ����� (����� �����)
    public int width = 10;      // ���������� ����� � ������ ������� �� ������
    public int height = 10;     // ���������� ����� � ������ ������� �� ������
    public float cellWidth = 1f; // ������ ����� ������
    public float cellHeight = 1f; // ������ ����� ������
    public Color lineColor = Color.green; // ���� �����

    void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        // ������ ������������ �����
        for (int x = -width; x <= width; x++)
        {
            float xPos = centerPoint.x + x * cellWidth;
            Vector3 start = new Vector3(xPos, centerPoint.y + height * cellHeight, 0);
            Vector3 end = new Vector3(xPos, centerPoint.y - height * cellHeight, 0);
            Gizmos.DrawLine(start, end);
        }

        // ������ �������������� �����
        for (int y = -height; y <= height; y++)
        {
            float yPos = centerPoint.y + y * cellHeight;
            Vector3 start = new Vector3(centerPoint.x - width * cellWidth, yPos, 0);
            Vector3 end = new Vector3(centerPoint.x + width * cellWidth, yPos, 0);
            Gizmos.DrawLine(start, end);
        }
    }
}
