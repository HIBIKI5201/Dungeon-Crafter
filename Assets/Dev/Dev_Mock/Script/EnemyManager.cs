using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private float _maxHealth = 300;
    private float _currentHealth;

    EnemyHealthBarManager _healthBarManager;
    [SerializeField]
    Vector3 _healthBarOffset = new Vector3(0, 1, 0);

    private void Start()
    {
        _maxHealth = Random.Range(2, 10) * 100;

        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        _healthBarManager?.FollowTarget(transform.position + _healthBarOffset);
    }

    public void Init(EnemyHealthBarManager healthBar)
    {
        _healthBarManager = healthBar;
    }

    /// <summary>
    /// キャラクターにダメージを与える
    /// </summary>
    /// <param name="damage">正数でダメージ、負数で回復</param>
    public void AddDamage(float damage)
    {
        _currentHealth -= damage;
        _healthBarManager?.BarFillUpdate(_currentHealth / _maxHealth);

        if (_currentHealth <= 0)
        {
            DeathBehaviour();
        }
    }

    private void DeathBehaviour()
    {
        Destroy(gameObject);
        Destroy(_healthBarManager.gameObject);
    }
}
