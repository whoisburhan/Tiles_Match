using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GS.TilesMatch
{
    public class UI_Manager : MonoBehaviour
    {
        public static UI_Manager Instance { get; private set; }

        [Header("All Text File")]
        [SerializeField] private Text uniqueTilesLeftText;
        [SerializeField] private Text wrongAttemptText;
        [SerializeField] private Text countDownTimerText;

        [Header("Tiles Border")]
        [SerializeField] private Image uniqueTilesLeftBorder;
        [SerializeField] private Image wrongAttemptBorder;

        [Header("GameObjects")]
        [SerializeField] private GameObject smallTilesOne, smallTilesTwo;

        [Header("Transform")]
        [SerializeField] private CanvasGroup PausePanelCanvas;
        [SerializeField] private CanvasGroup GameCompletePanelCanvas;

        [Header("Buttons")]
        [SerializeField] private Button homeButton, shopButton, hintButton, retryButton;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            GameManager.OnReset += Reset;
        }

        private void OnDisable()
        {
            GameManager.OnReset -= Reset;
        }

        private void Start()
        {
            hintButton.onClick.AddListener(() => { Hint(); });
            retryButton.onClick.AddListener(() => 
            {
                 GameManager.Instance.Refresh();
            });
        }

        #region Button Func

        public void GameCompleteMenu()
        {
            GameCompletePanelCanvas.DOFade(1f, 0.3f);

            Sequence _sequence = DOTween.Sequence();
            _sequence.Append(GameCompletePanelCanvas.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f));
            _sequence.Append(GameCompletePanelCanvas.transform.DOScale(Vector3.one, 0.05f));
            _sequence.OnComplete(() => { GameCompletePanelCanvas.blocksRaycasts = true; });
        }

        public void NextLevel(Action OnNextLevel = null)
        {
            GameCompletePanelCanvas.DOFade(0f, 0.3f);

            GameCompletePanelCanvas.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.3f).OnComplete(() => { 
                GameCompletePanelCanvas.blocksRaycasts = false;
            });
        }
        public void Pause()
        {
            PausePanelCanvas.DOFade(1f, 0.3f);

            Sequence _sequence = DOTween.Sequence();
            _sequence.Append(PausePanelCanvas.transform.DOScale(new Vector3(1.1f,1.1f,1.1f),0.25f));
            _sequence.Append(PausePanelCanvas.transform.DOScale(Vector3.one, 0.05f));
            _sequence.OnComplete(() => { PausePanelCanvas.blocksRaycasts = true; });
        }

        public void Resume()
        {
            PausePanelCanvas.DOFade(0f, 0.3f);

            PausePanelCanvas.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.3f).OnComplete(() => { PausePanelCanvas.blocksRaycasts = false; });
        }

        public void Hint()
        {
            GameManager.Instance.ShowHint(5);
        }

        #endregion


        public void SetHitButtonVisibility(bool _isVisible)
        {
            if(hintButton != null)
            {
                Debug.Log("OK");
                hintButton.interactable = _isVisible;
            }
        }

        public void SetRefreshButtonVisibility(bool _isVisible)
        {
            if (retryButton != null)
            {
                Debug.Log("OK");
                retryButton.interactable = _isVisible;
            }
        }
        public void CountDownTimerAnimation(int _countDownTime)
        {
            countDownTimerText.gameObject.SetActive(true);
            if (_countDownTime > 0)
            {
                countDownTimerText.text = _countDownTime.ToString();
                countDownTimerText.transform.DOScale(Vector3.one, 1f).OnComplete(() =>
                {
                    countDownTimerText.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    _countDownTime--;
                    CountDownTimerAnimation(_countDownTime);

                });
            }
            else
            {
                countDownTimerText.gameObject.SetActive(false);
            }
        }

        public void SetTilesLeft(string _tilesText, Transform _fromOne, Transform _fromTwo)
        {
            Sequence _sequence = DOTween.Sequence();

            smallTilesOne.transform.position = _fromOne.position;
            smallTilesTwo.transform.position = _fromTwo.position;
            smallTilesOne.SetActive(true);
            smallTilesTwo.SetActive(true);

            _sequence.Append(smallTilesOne.transform.DOMove(uniqueTilesLeftBorder.transform.position, .5f).OnComplete(() => {
                smallTilesOne.gameObject.SetActive(false);
                uniqueTilesLeftText.text = _tilesText;
                UI_Manager.Instance.SetHitButtonVisibility(true);
                UI_Manager.Instance.SetRefreshButtonVisibility(true);

            }));
            smallTilesTwo.transform.DOMove(uniqueTilesLeftBorder.transform.position, .5f).OnComplete(() => {
                smallTilesTwo.gameObject.SetActive(false);
                
            });

            
        }

        public void SetTilesLeft(string _tilesText)
        {
            uniqueTilesLeftText.text = _tilesText;
        }

        public void SetWrongAttempt(string _tilesText)
        {

            Color _lastColor = wrongAttemptText.color;
            Color _color = Color.red;

            wrongAttemptText.text = _tilesText;
            
            Sequence _sequence = DOTween.Sequence();
            _sequence.Append(wrongAttemptText.DOColor(_color, 0.5f));
            _sequence.Append(wrongAttemptText.DOColor(_color, 2f));
            _sequence.Append(wrongAttemptText.DOColor(_lastColor, 0.5f));
            ChangeBorderColorIndicatorColor(_color, wrongAttemptBorder);

        }

        
        private void ChangeBorderColorIndicatorColor(Color _color, Image _img)
        {
            Color _lastColor = _img.color;

            Sequence _sequence = DOTween.Sequence();
            _sequence.Append(_img.DOColor(_color, 0.5f));
            _sequence.Append(_img.DOColor(_color, 2f));
            _sequence.Append(_img.DOColor(_lastColor, 0.5f));

        }

        private void Reset()
        {
            uniqueTilesLeftText.text = wrongAttemptText.text = "0";
        }

    }
}