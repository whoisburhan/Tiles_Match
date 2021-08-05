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

        private int inputCounter = 0;
        private GridItem tilesOne, tilesTwo;

        [HideInInspector] public bool IsPlay = true;

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


            GenerateLevel(6);

            StartCoroutine( WaitTime(() =>
            {
                OnHideAllTiles?.Invoke(false);
                Debug.Log("Working 2");
            }));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GenerateLevel(6);
            }
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
                    StartCoroutine(WaitTime(() => {
                        tilesOne.gameObject.SetActive(false);
                        tilesTwo.gameObject.SetActive(false);
                        IsPlay = true;
                    }, 2f));
                    
                }
                else
                {
                    Debug.Log("HideTiles");
                    StartCoroutine(WaitTime(()=> {
                        tilesOne.HideTiles();
                        tilesTwo.HideTiles();
                        StartCoroutine(WaitTime(()=> { IsPlay = true; },1f));2
                    },2f));
                   
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