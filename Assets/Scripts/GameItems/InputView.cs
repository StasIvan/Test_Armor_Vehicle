using Installers;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace GameItems
{
    public class InputView : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            _signalBus.Fire(new OnDragSignal() { EventData = eventData});
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _signalBus.Fire(new OnEndDragSignal() { EventData = eventData});
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _signalBus.Fire(new OnPointerDownSignal() { EventData = eventData});
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _signalBus.Fire(new OnPointerUpSignal() { EventData = eventData});
        }
    }
}