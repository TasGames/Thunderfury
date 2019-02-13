 using UnityEngine;
 using UnityEngine.Events;
 using UnityEngine.EventSystems;
 
 public class ButtonWhenHighlighted : MonoBehaviour, IPointerEnterHandler, ISelectHandler
  {
      public void OnPointerEnter(PointerEventData eventData)
      {
          //do your stuff when highlighted
      }

      public void OnSelect(BaseEventData eventData)
      {
          
      }
  }
