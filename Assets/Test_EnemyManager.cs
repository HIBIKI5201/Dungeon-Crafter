using UnityEngine;

public class Test_EnemyManager : MonoBehaviour
{
    private float maxHealth = 300;
    private float currentHealth;

    EnemyHealthBarManager healthBarManager;
    [SerializeField]
    Vector3 healthBarOffset = new Vector3(0, 1, 0);
    public EnemyData_B _enemyData_B ;

    private void Start()
    {
        maxHealth = Random.Range(2, 10) * 100;

        currentHealth = maxHealth;
    }

    private void Update()
    {
        healthBarManager?.FollowTarget(transform.position + healthBarOffset);
    }

    public void Init(EnemyHealthBarManager healthBar)
    {
        healthBarManager = healthBar;
    }

    /// <summary>
    /// キャラクターにダメージを与える
    /// </summary>
    /// <param name="damage">正数でダメージ、負数で回復</param>
    public void AddDamage(float damage)
    {
        currentHealth -= damage;
        healthBarManager?.BarFillUpdate(currentHealth / maxHealth);

        if (currentHealth <= 0)
        {

        }
    }

}
