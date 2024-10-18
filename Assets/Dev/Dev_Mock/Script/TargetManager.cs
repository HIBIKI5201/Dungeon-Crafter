using UnityEngine;

public class Target : MonoBehaviour
{
    private void OnTriggerStay(Collider collision)
    {
        Debug.Log(collision.gameObject.name);

        if (collision.gameObject.TryGetComponent<EnemyManager>(out EnemyManager manager))
        {
            manager.AddDamage(100);
        }
    }
}
