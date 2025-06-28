using UnityEngine;

namespace Windows
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