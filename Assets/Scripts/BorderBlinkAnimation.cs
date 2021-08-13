using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GS.TilesMatch
{
    public class BorderBlinkAnimation : MonoBehaviour
    {
        [SerializeField] Image borderIMg;
        public void ChangeBorderColorIndicatorColor(Color _color)
        {
            Color _lastColor = borderIMg.color;

            borderIMg.DOColor(_color, 0.35f).OnComplete(()=> 
            {
                borderIMg.DOColor(_lastColor, 0.35f);
            });
        }
    }
}