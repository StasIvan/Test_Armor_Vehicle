using System;
using Base.BaseWindow;
using UnityEngine;
using UnityEngine.UI;

namespace Windows.WinGameWindow
{
    public class WinGameView : ViewBase<WinGameModel>
    {
        [SerializeField] private GameObject _windowRoot;
        [SerializeField] private Button _closeButton;

        public event Action OnCloseClicked;

        protected override void OnModelChanged(WinGameModel model)
        {
            _windowRoot.SetActive(model.IsVisible);
        }

        private void Awake()
        {
            _closeButton.onClick.AddListener(() => OnCloseClicked?.Invoke());
        }

        protected override void OnDestroy()
        {
            _closeButton.onClick.RemoveAllListeners();
            base.OnDestroy();
        }
    }
}