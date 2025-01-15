using UnityEngine;

public class GridDrawer : MonoBehaviour
{
    public Vector2 centerPoint = new Vector2(0, 0); // Координаты начальной точки (центр сетки)
    public int width = 10;      // Количество ячеек в каждую сторону по ширине
    public int height = 10;     // Количество ячеек в каждую сторону по высоте
    public float cellWidth = 1f; // Ширина одной ячейки
    public float cellHeight = 1f; // Высота одной ячейки
    public Color lineColor = Color.green; // Цвет линий

    void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        // Рисуем вертикальные линии
        for (int x = -width; x <= width; x++)
        {
            float xPos = centerPoint.x + x * cellWidth;
            Vector3 start = new Vector3(xPos, centerPoint.y + height * cellHeight, 0);
            Vector3 end = new Vector3(xPos, centerPoint.y - height * cellHeight, 0);
            Gizmos.DrawLine(start, end);
        }

        // Рисуем горизонтальные линии
        for (int y = -height; y <= height; y++)
        {
            float yPos = centerPoint.y + y * cellHeight;
            Vector3 start = new Vector3(centerPoint.x - width * cellWidth, yPos, 0);
            Vector3 end = new Vector3(centerPoint.x + width * cellWidth, yPos, 0);
            Gizmos.DrawLine(start, end);
        }
    }
}
