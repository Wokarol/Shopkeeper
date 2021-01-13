using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopkeeper
{
    public class CraftingSelectorPanel : MonoBehaviour
    {
        [SerializeField] private float panelPeekMargin = 40;
        private bool open;

        private RectTransform rectTransform;
        private Sequence openCloseSequence;

        private void Awake()
        {
            rectTransform = transform as RectTransform;
        }

        private void Start()
        {
            rectTransform.anchorMin = new Vector2(0, -1);
            rectTransform.anchorMax = new Vector2(1, 0);

            rectTransform.anchoredPosition = new Vector2(0, panelPeekMargin);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Toggle();
            }
        }

        private void Toggle()
        {
            open = !open;

            openCloseSequence.Kill();

            if (open)
            {
                float duration = 0.8f;
                openCloseSequence = DOTween.Sequence();

                openCloseSequence.Append(rectTransform.DOAnchorMax(Vector2.one, duration));
                openCloseSequence.Join(rectTransform.DOAnchorMin(Vector2.zero, duration));
                openCloseSequence.Join(rectTransform.DOAnchorPos(Vector2.zero, duration));

                openCloseSequence.SetEase(Ease.OutBounce);
            }
            else
            {
                float duration = 0.8f;
                openCloseSequence = DOTween.Sequence();

                openCloseSequence.Append(rectTransform.DOAnchorMax(new Vector2(1, 0), duration));
                openCloseSequence.Join(rectTransform.DOAnchorMin(new Vector2(0, -1), duration));
                openCloseSequence.Join(rectTransform.DOAnchorPos(new Vector2(0, panelPeekMargin), duration));

                openCloseSequence.SetEase(Ease.InOutCubic);
            }
        }
    }
}
