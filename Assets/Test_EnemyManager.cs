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
    /// �L�����N�^�[�Ƀ_���[�W��^����
    /// </summary>
    /// <param name="damage">�����Ń_���[�W�A�����ŉ�</param>
    public void AddDamage(float damage)
    {
        currentHealth -= damage;
        healthBarManager?.BarFillUpdate(currentHealth / maxHealth);

        if (currentHealth <= 0)
        {

        }
    }

}
