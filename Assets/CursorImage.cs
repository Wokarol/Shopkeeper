using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class CursorImage : MonoBehaviour
{
//    [SerializeField] AnimationCurve rotationCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] float duration = 0.2f;

    [SerializeField] Vector2 offset;

	private void Start()
	{
        Cursor.visible = false;
	}

	// Update is called once per frame
	void Update()
    {
        transform.position = Input.mousePosition + (Vector3)offset;

        if (Input.GetMouseButtonDown(0))
            transform.DOBlendableLocalRotateBy(new Vector3(0, 0, 30), duration).SetEase(Ease.OutCubic);
        if (Input.GetMouseButtonUp(0))
            transform.DOBlendableLocalRotateBy(new Vector3(0, 0, -30), duration).SetEase(Ease.InCubic);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
