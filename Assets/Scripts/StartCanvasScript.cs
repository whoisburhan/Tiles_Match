using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GS.TilesMatch
{
    public class StartCanvasScript : MonoBehaviour
    {
        [SerializeField] private Transform[] GameTittlePositions;       // 0 - Main Transform Object; 1 - index for start position ; 2 - for end position
        [SerializeField] private Transform PlayButton;
        [SerializeField] private Transform[] LeftSideButtonPositions;
        [SerializeField] private Transform[] RightSideButtonPositions;

        private bool isAnimationRunning = false;

        private void OnEnable()
        {
            GameTittlePositions[0].DOMove(GameTittlePositions[2].position, 1f);

            Sequence _sequence = DOTween.Sequence();
            _sequence.Append(PlayButton.DOScale(new Vector3(1.1f, 1.1f, 1.1f),.85f));
            _sequence.Append(PlayButton.DOScale(Vector3.one, .15f));
        }

        public void StartCanvasClosingAnimationFunc()
        {

        }
    }
}