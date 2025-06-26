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
            _signalBus.Fire(new OnDrag() { EventData = eventData});
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _signalBus.Fire(new OnEndDrag() { EventData = eventData});
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _signalBus.Fire(new OnPointerDown() { EventData = eventData});
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _signalBus.Fire(new OnPointerUp() { EventData = eventData});
        }
    }
}