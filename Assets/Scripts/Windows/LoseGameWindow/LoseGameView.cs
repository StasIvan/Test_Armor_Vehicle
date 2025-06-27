using System;
using Base.BaseWindow;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Windows.LoseGameWindow
{
    public class LoseGameView : ViewBase<LoseGameModel>
    {
        [SerializeField] private GameObject _windowRoot;
        [SerializeField] private Button _closeButton;

        public event Action OnCloseClicked;

        protected override void OnModelChanged(LoseGameModel model)
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