using UnityEngine;
using DG.Tweening;

public class UI_Shine : MonoBehaviour
{
    [SerializeField] private Transform shine;
    [SerializeField] private float offset;
    [SerializeField] private float speed;
    [SerializeField] private float minDelay;
    [SerializeField] private float maxDelay;
    [SerializeField] private bool inYDirection = false;

    private void Start()
    {
        Animate();
    }

    private void Animate()
    {
        if (shine != null)
        {
            if (!inYDirection)
            {
                shine.DOLocalMoveX(offset, speed).SetEase(Ease.Linear).SetDelay(Random.Range(minDelay, maxDelay)).OnComplete(() =>
                {
                    shine.DOLocalMoveX(-offset, 0);
                    Animate();
                });
            }
            else
            {
                shine.DOLocalMoveY(offset, speed).SetEase(Ease.Linear).SetDelay(Random.Range(minDelay, maxDelay)).OnComplete(() =>
                {
                    shine.DOLocalMoveY(-offset, 0);
                    Animate();
                });
            }
        }
    }

    public void KillTween()
    {
        shine.DOKill();
    }
}