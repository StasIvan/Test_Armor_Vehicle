using Core.Interfaces;
using UnityEngine;

namespace Features.GameItems.EnemyItem
{
    public class EnemySeekerView : MonoBehaviour
    {
        [SerializeField] private EnemyView _item;
        
        private void OnTriggerEnter(Collider other)
        {
            var dmg = other.GetComponent<IDamageable>();
            if (dmg == null) return;
            
            _item.SetTarget(other.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            _item.SetTarget(null);
        }
    }
}