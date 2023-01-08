using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour, IContinuousTask
{
    public bool isDone { get; set; } //true - передвижение фишки окончено
    public string Name;
    public int Turns = 0, Bonuses = 0, Penalties = 0, CurrentGamePos = 0;
    public bool PassTurn = false;

    public void ChangePosition(Vector3 newPosition)
    {
        isDone = false;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(newPosition, 1f));
        seq.Join(transform.DOMoveY(0.01f, 0.5f).SetRelative());
        seq.Insert(0.5f, transform.DOMoveY(newPosition.y, 0.5f));
        seq.AppendCallback(() => isDone = true);
        seq.Play();
    }
}
