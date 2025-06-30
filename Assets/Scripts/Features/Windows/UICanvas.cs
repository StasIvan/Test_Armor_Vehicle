using UnityEngine;

namespace Features.Windows
{
    public class UICanvas : MonoBehaviour
    {
        [SerializeField] private Transform _windowsTransform;

        public Transform GetWindowsTransform()
        {
            return _windowsTransform;
        }
    }
}