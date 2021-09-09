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
            if (AudioManager.Instance != null) AudioManager.Instance.BackgroundAudioFunc(0);

            GameTittlePositions[0].DOMove(GameTittlePositions[2].position, 1f);

            Sequence _sequence = DOTween.Sequence();
            _sequence.Append(PlayButton.DOScale(new Vector3(1.1f, 1.1f, 1.1f),.85f));
            _sequence.Append(PlayButton.DOScale(Vector3.one, .15f));

            LeftSideButtonPositions[0].DOMove(LeftSideButtonPositions[2].position, 1f);

            RightSideButtonPositions[0].DOMove(RightSideButtonPositions[2].position, 1f);

        }


        private void Start()
        {
            PlayButton.GetComponent<Button>().onClick.AddListener(()=> 
            {
                StartCanvasClosingAnimationFunc();
            });
        }

        private void StartCanvasClosingAnimationFunc()
        {
            GameTittlePositions[0].DOMove(GameTittlePositions[1].position, 1f);
            LeftSideButtonPositions[0].DOMove(LeftSideButtonPositions[1].position, 1f);
            RightSideButtonPositions[0].DOMove(RightSideButtonPositions[1].position, 1f);

            PlayButton.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 1f).OnComplete(()=> 
            {
                UI_Manager.Instance.GameplayCanvas.SetActive(true);
                GameManager.Instance.Refresh();
                UI_Manager.Instance.StartCanvas.SetActive(false);

                if (AudioManager.Instance != null) AudioManager.Instance.StopBackgroundMusic();
            });
        }

    }
}