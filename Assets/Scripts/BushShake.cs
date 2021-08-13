using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushShake : MonoBehaviour
{
    void Start()
    {
        Shake();
    }

    private void Shake()
    {
        transform.DOShakeRotation(50f, 2f, 3, 60f).OnComplete(()=> { Shake(); });
    }

}
