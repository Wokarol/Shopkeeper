using DG.Tweening;
using Shopkeeper.World;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

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
        [SerializeField] private Vector2 curveOffsetMinMaxMoney = new Vector2(50, 100);
        [SerializeField] private Vector2 curveOffsetMinMaxHappiness = new Vector2(50, 100);
        [SerializeField] private float goodFlyingInterval = 0.03f;
        [SerializeField] private float flyDuration = 7f;
        [SerializeField] private int ScaleOvershoot = 4;
        [SerializeField] private float moneyPerIcon = 25;
        [SerializeField] private float happinessPerIcon = 25;

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
                seq.Append(AnimateFlyingGoods(Mathf.CeilToInt(money / moneyPerIcon), Mathf.CeilToInt(happiness / happinessPerIcon)));

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

        private Tween AnimateFlyingGoods(int countMoney, int countHappiness)
        {
            Sequence seq = DOTween.Sequence();
            for (int i = 0; i < countMoney; i++)
            {
                Image moneyIconCopy = Instantiate(moneyImage, moneyImage.transform.parent);
                var mrt = moneyIconCopy.transform as RectTransform;

                float delay = i * goodFlyingInterval;
                seq.Insert(delay, DOFly(mrt, moneyIconCopy.canvas.transform, MoneyAndHappinessBar.MoneyIcon.position, curveOffsetMinMaxMoney));
                seq.InsertCallback(delay + flyDuration, () =>
                {
                    MoneyAndHappinessBar.MoneyIcon.DOKill(true);
                    MoneyAndHappinessBar.MoneyIcon.DOPunchPosition(Random.insideUnitCircle.normalized * 10, 0.10f);
                });
            }
            for (int i = 0; i < countHappiness; i++)
            {
                Image happinessImageCopy = Instantiate(happinessImage, happinessImage.transform.parent);
                var hrt = happinessImageCopy.transform as RectTransform;

                float delay = i * goodFlyingInterval;
                seq.Insert(delay, DOFly(hrt, happinessImageCopy.canvas.transform, MoneyAndHappinessBar.HappinessIcon.position, curveOffsetMinMaxHappiness));
                seq.InsertCallback(delay + flyDuration, () =>
                {
                    MoneyAndHappinessBar.HappinessIcon.DOKill(true);
                    MoneyAndHappinessBar.HappinessIcon.DOPunchPosition(Random.insideUnitCircle.normalized * 10, 0.1f);
                });
            }
            return seq;
        }

        private Tween DOFly(RectTransform rt, Transform parent, Vector3 targetPos, Vector2 curveOffsetMinMax)
        {
            rt.SetParent(parent);

            Vector3[] path = new Vector3[3];
            path[0] = rt.position;
            path[2] = targetPos;

            float offset = Random.Range(curveOffsetMinMax.x, curveOffsetMinMax.y);

            Vector3 tangent = Quaternion.Euler(0, 0, 90) * (path[2] - path[0]).normalized;
            Vector3 middle = Vector3.Lerp(path[0], path[2], 0.5f);

            path[1] = tangent * offset + middle;

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
