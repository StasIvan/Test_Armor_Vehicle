using UnityEngine;

namespace Features.GameItems
{
    public class GameItemHealth : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private float _currentHealth;
        private float _maxHealth;
        private Vector3 _originalScale;
        
        private void Awake()
        {
            _originalScale = _spriteRenderer.transform.localScale;
        }

        public void SetHealth(float amount)
        {
            _currentHealth = amount;
            float healthPercentage = _currentHealth / _maxHealth;
            _spriteRenderer.transform.localScale =
                new Vector3(_originalScale.x * healthPercentage, _originalScale.y, _originalScale.z);
        }

        public void Initialize(float health)
        {
            _spriteRenderer.transform.localScale = _originalScale;
            _maxHealth = health;
            _currentHealth = _maxHealth;
        }
    }
}