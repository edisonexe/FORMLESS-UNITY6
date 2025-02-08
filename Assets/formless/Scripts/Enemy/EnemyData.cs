using Formless.Enemy;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemyData
{
    public string enemyType;
    public Vector3 position;
    public Sprite enemySprite;

    // Данные аниматора
    public string animatorControllerName;  // Имя аниматора, а не сам компонент

    // Данные для коллайдеров
    public Vector2 boxColliderSize;
    public Vector2 boxColliderOffset;
    public float capsuleColliderSizeX;
    public float capsuleColliderSizeY;
    public float capsuleColliderOffsetX;
    public float capsuleColliderOffsetY;
    public int capsuleColliderDirection;  // int для направления капсулы (0 - вертикальное, 1 - горизонтальное)

    // Данные для PolygonCollider2D атак
    public PolygonColliderData basicAttackCollider;
    public PolygonColliderData strongAttackCollider;

    public EnemyData(Enemy enemy)
    {
        enemyType = enemy.name;  
        position = enemy.transform.position;
        enemySprite = enemy.GetComponent<SpriteRenderer>().sprite;

        // Сохраняем имя аниматора
        Animator animator = enemy.GetComponent<Animator>();
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animatorControllerName = animator.runtimeAnimatorController.name;
        }

        // Сохраняем данные о BoxCollider2D
        BoxCollider2D boxCollider = enemy.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            boxColliderSize = boxCollider.size;
            boxColliderOffset = boxCollider.offset;
        }

        // Сохраняем данные о CapsuleCollider2D
        CapsuleCollider2D capsuleCollider = enemy.GetComponent<CapsuleCollider2D>();
        if (capsuleCollider != null)
        {
            capsuleColliderSizeX = capsuleCollider.size.x;
            capsuleColliderSizeY = capsuleCollider.size.y;
            capsuleColliderOffsetX = capsuleCollider.offset.x;
            capsuleColliderOffsetY = capsuleCollider.offset.y;
            capsuleColliderDirection = (int)capsuleCollider.direction;  // Преобразуем в int
        }

        // Сохраняем данные для атак
        basicAttackCollider = GetPolygonColliderData("BasicAttack");
        strongAttackCollider = GetPolygonColliderData("StrongAttack");
    }

    // Метод для получения данных PolygonCollider2D
    private PolygonColliderData GetPolygonColliderData(string attackName)
    {
        PolygonColliderData colliderData = new PolygonColliderData();
        GameObject attackObject = GameObject.Find(attackName);
        if (attackObject != null)
        {
            PolygonCollider2D attackCollider = attackObject.GetComponent<PolygonCollider2D>();
            if (attackCollider != null)
            {
                colliderData.pathCount = attackCollider.pathCount;
                for (int i = 0; i < attackCollider.pathCount; i++)
                {
                    colliderData.paths.Add(attackCollider.GetPath(i));
                }
            }
        }
        return colliderData;
    }
}



// Структура для хранения данных PolygonCollider2D
[System.Serializable]
public class PolygonColliderData
{
    public int pathCount;
    public List<Vector2[]> paths = new List<Vector2[]>();
}
