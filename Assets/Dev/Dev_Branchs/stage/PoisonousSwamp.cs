using DCFrameWork.Enemy;
using DCFrameWork.MainSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DCFrameWork
{
    public class PoisonousSwamp : Swamp_B
    {
        [SerializeField] float _damage;
        [SerializeField] float _rate;
        List<IEnemy> _enemies = new List<IEnemy>();
        private void Start()
        {
            StartCoroutine(AddDamage());
        }
        protected override void AddCondition(IEnemy enemy)
        {
            _enemies.Add(enemy);
        }
        protected override void RemoveCondition(IEnemy enemy)
        {
            _enemies.Remove(enemy);
        }
        IEnumerator AddDamage()
        {
            while (true)
            {
                foreach (IEnemy enemy in _enemies)
                {
                    enemy.HitDamage(_damage);
                }
                yield return FrameWork.PausableWaitForSecond(_rate);
            }         
        }
    }
}
