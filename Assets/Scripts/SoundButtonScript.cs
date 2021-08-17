using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GS.TilesMatch
{
    public class SoundButtonScript : MonoBehaviour
    {
        [Header("Color")]
        [SerializeField] private Color muteCOlor;
        [SerializeField] private Color unMuteColor;

        [Header("Transform")]
        [SerializeField] private Transform circle;

        [Header("Image")]
        [SerializeField] private Image ButtonUIImg;
    }
}