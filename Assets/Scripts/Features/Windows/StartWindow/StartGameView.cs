using System;
using Features.Windows.BaseWindow;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Windows.StartWindow
{
    public class StartGameView : ViewBase<StartGameModel>
    {
        [SerializeField] private GameObject _windowRoot;
        [SerializeField] private Button _closeButton;

        public event Action OnCloseClicked;

        protected override void OnModelChanged(StartGameModel model)
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