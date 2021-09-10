using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GS.TilesMatch
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public static event Action<bool> OnHideAllTiles;
        public static event Action<bool> OnShowAllTiles;
        public static event Action OnReset;

        private const string Current_Level_No_Key = "CURRENT_LEVEL_NO_KEY";

        public int CurrentLevelNo 
        { 
            get
            {
                UI_Manager.Instance.SetLevelNoInUI(PlayerPrefs.GetInt(Current_Level_No_Key, 1));
                return PlayerPrefs.GetInt(Current_Level_No_Key, 1); 
            } 
            set 
            {
                PlayerPrefs.SetInt(Current_Level_No_Key, value);
                UI_Manager.Instance.SetLevelNoInUI(value);
            }
        }

        [Header("Items Sprite")]
        [SerializeField] private List<Sprite> itemSprites = new List<Sprite>();

        [Header("Grid Item Parents")]
        [SerializeField] private List<GameObject> gridItemParents;

        [Header("Grid Items Row")]
        [SerializeField] private List<GridItem> gridItemRow0;
        [SerializeField] private List<GridItem> gridItemRow1;
        [SerializeField] private List<GridItem> gridItemRow2;
        [SerializeField] private List<GridItem> gridItemRow3;
        [SerializeField] private List<GridItem> gridItemRow4;
        [SerializeField] private List<GridItem> gridItemRow5;


        private List<List<GridItem>> gridItemRows = new List<List<GridItem>>();

        public int InputCounter { get { return inputCounter; } }

        private int inputCounter = 0;
        private GridItem tilesOne, tilesTwo;

        private int failedAttempt, uniqueTiles, countDownTimer;

        int uniqueTilesVariation;
        public int UniqueTilesLeft { get { return uniqueTiles; } }
        public int FailedAttempt { get { return failedAttempt; } }
        public int UniqueTilesVariation { get { return uniqueTilesVariation; } }

        [HideInInspector] public bool IsPlay = true;
        [HideInInspector] public bool IsAbleToRefresh = false;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }

            Instance = this;
        }

        private void Start()
        {
            gridItemRows.Add(gridItemRow0);
            gridItemRows.Add(gridItemRow1);
            gridItemRows.Add(gridItemRow2);
            gridItemRows.Add(gridItemRow3);
            gridItemRows.Add(gridItemRow4);
            gridItemRows.Add(gridItemRow5);

            if (AudioManager.Instance != null) AudioManager.Instance.BackgroundAudioFunc(0);

            //Refresh();

            /*StartCoroutine(WaitTime(() =>
           {
               OnHideAllTiles?.Invoke(false);
               Debug.Log("Working 2");
           }));*/

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && IsAbleToRefresh)
            {
                // GenerateLevel(6);
                Refresh();
            }
        }

        private int TilesAmountDecider()
        {
            int _currentLevel = CurrentLevelNo;
            if (_currentLevel <= 3) return 2;
            else if (_currentLevel <= 7) return 3;
            else if (_currentLevel <= 15) return Random.Range(2, 5);
            else if (_currentLevel <= 25) return Random.Range(2, 6);
            else return Random.Range(2, 7);
        }
        public void NewGame()
        {
            StopAllCoroutines();

            UI_Manager.Instance.SetHitButtonVisibility(false);

            tilesOne = tilesTwo = null;
            inputCounter = 0;

            IsPlay = false;
            uniqueTilesVariation = TilesAmountDecider();//2;//Random.Range(2, 7);
            int _showTilesTime = 15;

            uniqueTiles = uniqueTilesVariation * 2;
            failedAttempt = 0;

            UI_Manager.Instance.SetTilesLeft(uniqueTiles.ToString());

            // Activate & Deactivate Rows According to level
            for (int i = 0; i < gridItemRows.Count; i++)
            {
                for (int j = 0; j < gridItemRow0.Count; j++)
                {
                    if (i < uniqueTilesVariation)
                    {
                        gridItemRows[i][j].gameObject.SetActive(true);
                        gridItemRows[i][j].transform.DOScale(1f, 0.5f);
                    }
                    else
                    {
                        gridItemRows[i][j].gameObject.SetActive(false);
                    }
                }
            }

            // Generate Tiles
            GenerateLevel(uniqueTilesVariation);
            OnHideAllTiles?.Invoke(true);

            StartCoroutine(WaitTime(() =>
            {
            OnShowAllTiles?.Invoke(false);
            StartCoroutine(WaitTime(() => { OnHideAllTiles?.Invoke(false); }, 1.6f + _showTilesTime));
            StartCoroutine(WaitTime(()=>{ UI_Manager.Instance.CountDownTimerAnimation(_showTilesTime); }, 1f));
                
            }, 0.5f));

            IsPlay = true;
        }

        public void Refresh()
        {
            OnReset?.Invoke();
            StartCoroutine(WaitTime(() => {
                NewGame();
                IsAbleToRefresh = false;
                UI_Manager.Instance.SetRefreshButtonVisibility(false);
            },0.6f));
        }

        public void ShowHint(int _showTilesTime)
        {
            UI_Manager.Instance.SetHitButtonVisibility(false);
            OnShowAllTiles?.Invoke(false);
            StartCoroutine(WaitTime(() => { OnHideAllTiles?.Invoke(false); }, 1.6f + _showTilesTime));
            StartCoroutine(WaitTime(() => { UI_Manager.Instance.CountDownTimerAnimation(_showTilesTime); }, 1f));
        }

        public Sprite GetItemSprite(int index)
        {
            if (index >= 0 && index < itemSprites.Count)
            {
                return itemSprites[index];
            }

            else
            {
                Debug.LogError("Get Item Sprite Out Of Range!! Check Index No ...");
                return null;
            }
        }

        #region Level Generation
        private void GenerateLevel(int gridRow)
        {
            int _gridItemVariation = gridRow * 2;
            UI_Manager.Instance.SetTilesLeft(_gridItemVariation.ToString());

            List<int> _selectedGridItemVariationList = new List<int>();

            for (int i = 0; i < _gridItemVariation; i++)
            {
                int _gridItemNo = UnityEngine.Random.Range(0, itemSprites.Count);

                while (_selectedGridItemVariationList.Contains(_gridItemNo))
                {
                    _gridItemNo = UnityEngine.Random.Range(0, itemSprites.Count);
                }

                _selectedGridItemVariationList.Add(_gridItemNo);
            }

            List<int> _oneTimeUsedItemNoList = new List<int>();
            List<int> _twoTimeUsedItemNoList = new List<int>();

            for (int i = 0; i < gridRow; i++)
            {
                for (int j = 0; j < gridItemRows[i].Count; j++)
                {
                    int _gridItemNo = _selectedGridItemVariationList[Random.Range(0, _selectedGridItemVariationList.Count)];

                    while (_twoTimeUsedItemNoList.Contains(_gridItemNo))
                    {
                        _gridItemNo = _selectedGridItemVariationList[Random.Range(0, _selectedGridItemVariationList.Count)];
                    }

                    gridItemRows[i][j].SetGridItem(_gridItemNo);

                    if (_oneTimeUsedItemNoList.Contains(_gridItemNo))
                    {
                        _twoTimeUsedItemNoList.Add(_gridItemNo);
                        _oneTimeUsedItemNoList.Remove(_gridItemNo);
                    }
                    else
                    {
                        _oneTimeUsedItemNoList.Add(_gridItemNo);
                    }
                }
            }

        }

        #endregion

        public void OnTilesHit(GridItem hittedTiles)
        {
            inputCounter++;
            if (inputCounter == 1)
            {
                tilesOne = hittedTiles;
                Debug.Log("1");
                UI_Manager.Instance.SetHitButtonVisibility(false);
            }
            else if (inputCounter >= 2)
            {
                IsPlay = false;
                tilesTwo = hittedTiles;
                Debug.Log("2");


                if (tilesOne.ItemNo == tilesTwo.ItemNo)
                {
                    // Add some Score
                    Debug.Log("Tiles Matched!!");

                    StartCoroutine(WaitTime(() =>
                    {
                        tilesOne.ChangeBorderColorIndicatorColor(Color.green);
                        tilesTwo.ChangeBorderColorIndicatorColor(Color.green);
                    }, 1f));



                    StartCoroutine(WaitTime(() =>
                    {
                        if (AudioManager.Instance != null) AudioManager.Instance.AudioChangeFunc(0, 3);
                        tilesOne.transform.DOScale(0.1f, 0.5f).OnComplete(() =>
                        {
                            tilesOne.gameObject.SetActive(false);
                            uniqueTiles--;
                            UI_Manager.Instance.SetTilesLeft(uniqueTiles.ToString(), tilesOne.transform, tilesTwo.transform);

                            IsPlay = IsAbleToRefresh = true;
                        });
                        tilesTwo.transform.DOScale(0.1f, 0.5f).OnComplete(() => { tilesTwo.gameObject.SetActive(false); });

                    }, 2f));

                }
                else
                {
                    Debug.Log("HideTiles");

                    failedAttempt++;

                    StartCoroutine(WaitTime(() =>
                    {
                        tilesOne.ChangeBorderColorIndicatorColor(Color.red);
                        tilesTwo.ChangeBorderColorIndicatorColor(Color.red);
                        UI_Manager.Instance.SetWrongAttempt(failedAttempt.ToString());

                        if (AudioManager.Instance != null) AudioManager.Instance.AudioChangeFunc(0, 2);

                    }, 1f));

                    StartCoroutine(WaitTime(() =>
                    {
                        tilesOne.HideTiles();
                        tilesTwo.HideTiles();
                        StartCoroutine(WaitTime(() => { IsPlay = true; }, 2f));
                    }, 3f));

                }

                inputCounter = 0;
            }

        }


        private void Reset()
        {
            OnHideAllTiles?.Invoke(true);
            inputCounter = 0;
            tilesOne = tilesTwo = null;
        }

        private IEnumerator WaitTime(Action taskToComplete, float duration = 1f)
        {
            yield return new WaitForSeconds(duration);
            Debug.Log("Working");
            taskToComplete();
        }

        
    }
}