using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GS.TilesMatch
{
    public class GridItem : MonoBehaviour
    {
        [SerializeField] private Button GridItemButton;
        [SerializeField] private Image GridItemBorder;
        [SerializeField] private Image GridItemImg;
        [SerializeField] private Image BorderColorIndicator;

        [SerializeField] private float tilesFlipInterval = .5f;

        public int ItemNo; //{ get; set; }

        private bool isHidden = false;

        private void OnEnable()
        {
            GameManager.OnHideAllTiles += HideTiles;
            GameManager.OnShowAllTiles += ShowTiles;
            GameManager.OnReset += Reset;
        }

        private void OnDisable()
        {
            GameManager.OnHideAllTiles -= HideTiles;
            GameManager.OnShowAllTiles -= ShowTiles;
            GameManager.OnReset -= Reset;
        }

        private void Start()
        {
            GridItemButton.onClick.AddListener(() =>
            {
                if (GameManager.Instance.IsPlay)
                {
                    OnItemClick();
                }
            });

            // HideTiles();
        }


        public void SetGridItem(int itemNo)
        {
            ItemNo = itemNo;
            GridItemImg.sprite = GameManager.Instance.GetItemSprite(itemNo);
        }

        private void OnItemClick()
        {
            if (AudioManager.Instance != null) AudioManager.Instance.AudioChangeFunc(0, 1);
            ShowTiles();
            GameManager.Instance.OnTilesHit(this);

            /*if (!isHidden)
            {
                HideTiles();
            }
            else
            {
                ShowTiles();
            }*/
        }




        public void HideTiles(bool imediateAction = false)
        {
            BorderColorIndicator.fillAmount = 0f;

            if (!isHidden)
            {
                float _duration = imediateAction ? 0f : tilesFlipInterval;
                GridItemButton.interactable = false;
                transform.DORotate(new Vector3(0f, 90f, 0f), _duration).OnComplete(() =>
                {
                    GridItemImg.gameObject.SetActive(false);
                    transform.DORotate(new Vector3(0f, 0f, 0f), _duration).OnComplete(() =>
                    {
                        GridItemButton.interactable = true;
                        isHidden = true;
                        Debug.Log("Tiles Hide");
                        GameManager.Instance.IsAbleToRefresh = true;
                        UI_Manager.Instance.SetRefreshButtonVisibility(true);

                        if (!imediateAction)
                            UI_Manager.Instance.SetHitButtonVisibility(true);
                    });
                });
            }
        }

        public void ShowTiles(bool imediateAction = false)
        {
            if (isHidden)
            {
                isHidden = false;
                GameManager.Instance.IsAbleToRefresh = false;
                UI_Manager.Instance.SetRefreshButtonVisibility(false);
                float _duration = imediateAction ? 0f : tilesFlipInterval;
                GridItemButton.interactable = false;
                transform.DORotate(new Vector3(0f, 90f, 0f), _duration).OnComplete(() =>
                {
                    GridItemImg.gameObject.SetActive(true);
                    transform.DORotate(new Vector3(0f, 0f, 0f), _duration).OnComplete(() =>
                    {
                        GridItemButton.interactable = false;
                        
                    });
                });

                BorderColorIndicator.color = Color.blue;
                BorderColorIndicator.DOFillAmount(1, 1f);
            }
        }

        public void ChangeBorderColorIndicatorColor(Color _color)
        {
            BorderColorIndicator.DOColor(_color, 0.35f);
        }

        public void Reset()
        {
            ItemNo = -1;
            GridItemImg.sprite = null;
            transform.DOScale(0.1f, 0.5f).OnComplete(()=> { this.gameObject.SetActive(false); });
        }
    }
}