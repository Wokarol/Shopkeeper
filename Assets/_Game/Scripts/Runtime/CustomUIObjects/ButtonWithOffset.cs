using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shopkeeper
{
    public class ButtonWithOffset : Button
    {
        [SerializeField] private Vector2 offset = Vector2.zero;
        private SelectionState oldState;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (transition != Transition.SpriteSwap)
            {
                transition = Transition.SpriteSwap;
            }
        } 
#endif

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            if (oldState == state)
                return;

            if (state == SelectionState.Pressed)
            {
                foreach (RectTransform child in transform)
                {
                    child.anchoredPosition += offset;
                }
            }

            if (oldState == SelectionState.Pressed)
            {
                foreach (RectTransform child in transform)
                {
                    child.anchoredPosition -= offset;
                }
            }

            oldState = state;
        }
    } 
}
