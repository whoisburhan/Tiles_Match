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

        [SerializeField] private float tilesFlipInterval = 1f;

        public int ItemNo; //{ get; set; }

        private bool isHidden = false;

        private void OnEnable()
        {
            GameManager.OnHideAllTiles += HideTiles;
            GameManager.OnShowAllTiles += ShowTiles;
        }

        private void OnDisable()
        {
            GameManager.OnHideAllTiles -= HideTiles;
            GameManager.OnShowAllTiles -= ShowTiles;
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
                    });
                });
            }
        }

        public void ShowTiles(bool imediateAction = false)
        {
            if (isHidden)
            {
                float _duration = imediateAction ? 0f : tilesFlipInterval;
                GridItemButton.interactable = false;
                transform.DORotate(new Vector3(0f, 90f, 0f), _duration).OnComplete(() =>
                {
                    GridItemImg.gameObject.SetActive(true);
                    transform.DORotate(new Vector3(0f, 0f, 0f), _duration).OnComplete(() =>
                    {
                        GridItemButton.interactable = false;
                        isHidden = false;
                    });
                });
            }
        }

        public void Reset()
        {
            ItemNo = -1;
            GridItemImg.sprite = null;
        }
    }
}