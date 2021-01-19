using DG.Tweening;
using Shopkeeper.World;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shopkeeper
{
    [SelectionBase]
    public class OrderCardUI : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI description = null;
        [SerializeField] private ItemLister itemLister = null;
        [Space]
        [SerializeField] private Button confirmButton = null;
        [SerializeField] private Button cancelButton = null;
        [Space]
        [SerializeField] private Image resultPanel = null;
        [SerializeField] private TextMeshProUGUI moneyLabel = null;
        [SerializeField] private Image moneyImage = null;
        [SerializeField] private TextMeshProUGUI happinessLabel = null;
        [SerializeField] private Image happinessImage = null;
        [Space]
        [SerializeField] private Color goodColor = Color.green;
        [SerializeField] private Color badColor = Color.red;
        [Header("Animations")]
        [SerializeField] private int curveOffset = 50;
        [SerializeField] private float flyDuration = 7f;
        [SerializeField] private int ScaleOvershoot = 4;

        private Order order;
        private PlayerState playerState;

        public event Action OnOrderFinished;

        private void Start()
        {
            playerState = WorldContext.PlayerState;

            resultPanel.gameObject.SetActive(false);
            confirmButton.onClick.AddListener(FinisheOrder);
            cancelButton.onClick.AddListener(() => ShowResult(false, 0, -25));
        }

        private void FinisheOrder()
        {
            if (order.IsFullfilled(itemLister))
            {
                ShowResult(true, 100, 20);
            }
            else
            {
                ShowResult(false, 0, -50);
            }
        }

        private void ShowResult(bool success, int money, int happiness)
        {
            playerState.Money += money;
            playerState.ClientHappiness += happiness;

            resultPanel.gameObject.SetActive(true);
            resultPanel.color = success ? goodColor : badColor;
            moneyLabel.text = money.ToString();
            happinessLabel.text = happiness.ToString();

            Sequence seq = DOTween.Sequence();
            seq.Append(resultPanel.GetComponent<CanvasGroup>()
                .DOFade(0, 0.2f)
                .From()
                .SetEase(Ease.InOutCubic));

            if (success)
            {
                var mrt = moneyImage.transform as RectTransform;
                var hrt = happinessImage.transform as RectTransform;

                seq.Append(DOFly(mrt, moneyImage.canvas.transform, MoneyAndHappinessBar.MoneyIcon.position));
                seq.Join(DOFly(hrt, happinessImage.canvas.transform, MoneyAndHappinessBar.HappinessIcon.position));

            }
            else
            {
                seq.Append(transform.DOShakePosition(0.5f, 5f));
            }

            seq.AppendCallback(() =>
            {
                OnOrderFinished?.Invoke();
            });
        }

        private Tween DOFly(RectTransform rt, Transform parent, Vector3 targetPos)
        {
            rt.SetParent(moneyImage.canvas.transform);

            Vector3[] path = new Vector3[3];
            path[0] = rt.position;
            path[2] = targetPos;

            path[1] = (Quaternion.Euler(0, 0, 90) * (path[2] - path[0]).normalized * curveOffset) + Vector3.Lerp(path[0], path[2], 0.5f);

            Sequence seq = DOTween.Sequence();
            seq.Append(rt.DOPath(path, flyDuration, pathType: PathType.CatmullRom).SetEase(Ease.InSine));
            seq.Join(rt.DOScale(0, flyDuration * 1f).SetEase(Ease.InBack, ScaleOvershoot));

            return seq;
        }

        public void Init(Order order)
        {
            description.text = order.Description;
            order.FillLister(itemLister);
            this.order = order;

            itemLister.FindDropTargetImplementation = FindDropTarget;
        }

        public bool FindDropTarget(Item item, out int i, out int innerItemIndex, out Action onDroppped)
        {
            return order.FindMatchingItem(itemLister, item, out i, out innerItemIndex, out onDroppped);
        }
    }
}
