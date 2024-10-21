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
    /// �L�����N�^�[�Ƀ_���[�W��^����
    /// </summary>
    /// <param name="damage">�����Ń_���[�W�A�����ŉ�</param>
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
