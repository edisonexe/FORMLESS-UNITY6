using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{

    [SerializeField] private Image _healthBar;
    public float bossHealth;

    public void UpdateBossHealthtBar()
    {
        _healthBar.fillAmount = bossHealth / 100;
    }
}
