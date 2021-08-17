using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GS.TilesMatch
{
    [RequireComponent(typeof(Button))]
    public class SoundButtonScript : MonoBehaviour
    {
        [Header("Color")]
        [SerializeField] private Color muteCOlor;
        [SerializeField] private Color unMuteColor;

        [Header("RectTransform")]
        [SerializeField] private RectTransform circle;

        [Header("Image")]
        [SerializeField] private Image ButtonUIImg;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void Start()
        {
            button.onClick.AddListener(() => 
            {
                if(circle.position.x > 0f)
                {

                }
            });
        }

        private void Animate()
        {

        }
    }
}