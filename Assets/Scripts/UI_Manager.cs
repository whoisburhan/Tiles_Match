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

        [Header("Coming Soon Canvas")]
        [SerializeField] private CanvasGroup comingSoonCanvasGrp;
        [SerializeField] private Transform[] comingSoonPanelPositions;


        [Header("CANVAS")]
        public GameObject StartCanvas;
        public GameObject GameplayCanvas;

        [Header("All Text File")]
        [SerializeField] private Text uniqueTilesLeftText;
        [SerializeField] private Text wrongAttemptText;
        [SerializeField] private Text countDownTimerText;
        [SerializeField] private Text levelNoText;

        [Header("Tiles Border")]
        [SerializeField] private Image uniqueTilesLeftBorder;
        [SerializeField] private Image wrongAttemptBorder;

        [Header("GameObjects")]
        [SerializeField] private GameObject smallTilesOne, smallTilesTwo;

        [Header("Transform")]
        [SerializeField] private CanvasGroup PausePanelCanvas;
        [SerializeField] private CanvasGroup GameCompletePanelCanvas;

        [Header("Buttons")]
        [SerializeField] private Button homeButton, shopButton, hintButton, retryButton,nextLevelButton,backToMainMenuButton, rateButton, eventButton, settingsButton , pauseMenuShopButton, aboutUsButton;

        [Header("Star Image")]
        [SerializeField] private Sprite[] starSprites;
        [SerializeField] private Image[] starImages;

        [Header("Particle Effect")]
        [SerializeField] private GameObject LevelCompleteParticleEffectCanvas;

        private bool isCountDownTimerActivated = true;

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
            homeButton.onClick.AddListener(() => { Pause(); });

            shopButton.onClick.AddListener(() => { ComingSoonFunc(); });

            hintButton.onClick.AddListener(() => { Hint(); });
            retryButton.onClick.AddListener(() => 
            {
                 GameManager.Instance.Refresh();
            });

            nextLevelButton.onClick.AddListener(()=> { NextLevel(); });

            backToMainMenuButton.onClick.AddListener(() => { ReturnToMainMenu(); });

            settingsButton.onClick.AddListener(() => { ComingSoonFunc(); });
            eventButton.onClick.AddListener(() => { ComingSoonFunc(); });

            pauseMenuShopButton.onClick.AddListener(() => { ComingSoonFunc(); });
            aboutUsButton.onClick.AddListener(() => { ComingSoonFunc(); });
        }

        private void CheckForLevelComplete()
        {
            if(GameManager.Instance.UniqueTilesLeft <= 0)
            {
                GameCompleteMenu();
            }
        }

        public void GameCompleteMenu()
        {
            GameCompletePanelCanvas.DOFade(1f, 0.3f);

            Sequence _sequence = DOTween.Sequence();
            _sequence.Append(GameCompletePanelCanvas.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f));
            _sequence.Append(GameCompletePanelCanvas.transform.DOScale(Vector3.one, 0.05f));
            _sequence.OnComplete(() => { 
                GameCompletePanelCanvas.blocksRaycasts = true;
                StarCountAndShow();
            });
        }

        private void StarCountAndShow()
        {
            float _percentage = 100f - ((float)(GameManager.Instance.FailedAttempt<=0 ? 1 :GameManager.Instance.FailedAttempt) / (float)(GameManager.Instance.UniqueTilesVariation * 2f)) * 100f;
            Debug.Log(_percentage);
            Debug.Log("AA : GameManager.Instance.FailedAttempt  " + (float)GameManager.Instance.FailedAttempt + "(float)(GameManager.Instance.UniqueTilesVariation * 2f)" + (float)(GameManager.Instance.UniqueTilesVariation * 2f));

            int _starGot = _percentage < 50f ? 1 : _percentage < 70f ? 2 : 3;
            Debug.Log("AA : __starGot : " + _starGot);

            switch (_starGot)
            {
                case 1:
                    StartCoroutine(Delay(() => { StarShow(0); }, 0.25f));
                    StartCoroutine(Delay(()=> { 
                        if (AudioManager.Instance != null) AudioManager.Instance.AudioChangeFunc(0, 4);
                        LevelCompleteParticleEffectCanvas.SetActive(true);
                    },0.5f));
                    break;
                case 2:
                    StartCoroutine(Delay(() => { StarShow(0); }, 0.25f));
                    StartCoroutine(Delay(() => { StarShow(1); }, 0.5f));
                    StartCoroutine(Delay(() => { 
                        if (AudioManager.Instance != null) AudioManager.Instance.AudioChangeFunc(0, 4);
                        LevelCompleteParticleEffectCanvas.SetActive(true);
                    }, 0.75f));
                    break;
                case 3:
                    StartCoroutine(Delay(() => { StarShow(0); }, 0.25f));
                    StartCoroutine(Delay(() => { StarShow(1); }, 0.5f));
                    StartCoroutine(Delay(() => { StarShow(2); }, 0.75f));
                    StartCoroutine(Delay(() => { 
                        if (AudioManager.Instance != null) AudioManager.Instance.AudioChangeFunc(0, 4);
                        LevelCompleteParticleEffectCanvas.SetActive(true);
                    }, 1.0f));
                    break;
            }
        }

        private void StarShow(int x)
        {
            if(x >= 0 && x < starImages.Length)
            {
                starImages[x].sprite = starSprites[1];
                if(AudioManager.Instance != null)   AudioManager.Instance.AudioChangeFunc(0, 0);
            }
        }

        #region Button Func

        public void SetLevelNoInUI(int currentLevelNo)
        {
            levelNoText.text = "Level - " + currentLevelNo.ToString();
        }

        public void NextLevel(Action OnNextLevel = null)
        {
            LevelCompleteParticleEffectCanvas.SetActive(false);
            GameCompletePanelCanvas.DOFade(0f, 0.3f);

            GameCompletePanelCanvas.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.3f).OnComplete(() => { 
                GameCompletePanelCanvas.blocksRaycasts = false;
                
                foreach(Image _img in starImages)
                {
                    _img.sprite = starSprites[0];
                }

                GameManager.Instance.CurrentLevelNo++;
                GameManager.Instance.NewGame();
                Reset();
                OnNextLevel?.Invoke();
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


        public void ReturnToMainMenu()
        {
            ResetCountDownTimerAnimation();
            PausePanelCanvas.DOFade(0f, 0.3f);

            PausePanelCanvas.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.3f).OnComplete(() => 
            {
                PausePanelCanvas.blocksRaycasts = false;
                GameplayCanvas.SetActive(false);
                StartCanvas.SetActive(true);
            });
        }


        public void ComingSoonFunc()
        {
            comingSoonCanvasGrp.blocksRaycasts = true;
            Sequence _sequence = DOTween.Sequence();
            _sequence.Append(comingSoonCanvasGrp.DOFade(1, 0.5f));
            _sequence.Append(comingSoonPanelPositions[0].DOMove(comingSoonPanelPositions[2].position, 1f));
            _sequence.AppendInterval(1f);
            _sequence.Append(comingSoonPanelPositions[0].DOMove(comingSoonPanelPositions[1].position, 1f));
            _sequence.Append(comingSoonCanvasGrp.DOFade(0, 0.5f)).OnComplete(()=> 
            {
                comingSoonCanvasGrp.blocksRaycasts = false;
            });
            

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
            if (isCountDownTimerActivated)
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
            else
            {
                isCountDownTimerActivated = true;
            }
        }

        public void ResetCountDownTimerAnimation()
        {
            isCountDownTimerActivated = false;
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
                CheckForLevelComplete();

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

        private IEnumerator Delay(Action taskToComplete, float duration = 1f)
        {
            yield return new WaitForSeconds(duration);
            Debug.Log("Working");
            taskToComplete();
        }

        private void Reset()
        {
             wrongAttemptText.text = "0";
        }

    }
}