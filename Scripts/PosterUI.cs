﻿
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace Sonic853.Udon.PosterUI
{
    public class PosterUI : UdonSharpBehaviour
    {
        string programName = "PosterUI";
        public bool initAtStart = false;
        // public UdonDebuger debuger;
        /// <summary>
        /// 滚动视图
        /// </summary>
        public ScrollRect scrollRect;
        /// <summary>
        /// 显示区域
        /// </summary>
        public RectTransform viewport;
        /// <summary>
        /// 容器
        /// </summary>
        public RectTransform content;
        /// <summary>
        /// TextureSpliter
        /// </summary>
        public TextureSpliter textureSpliter;
        /// <summary>
        /// Avatar Pedestal
        /// </summary>
        public VRC_AvatarPedestal avatarPedestal;
        /// <summary>
        /// Portal Marker
        /// </summary>
        public VRC_PortalMarker portalMarker;
        /// <summary>
        /// 海报数量
        /// </summary>
        [NonSerialized]
        public int count = 0;
        /// <summary>
        /// 海报
        /// </summary>
        [NonSerialized]
        Image[] childrens;
        /// <summary>
        /// 当前页
        /// </summary>
        public int index = 0;
        /// <summary>
        /// 目标位置
        /// </summary>
        public float target = 0;
        /// <summary>
        /// 是否需要移动
        /// </summary>
        [NonSerialized] public bool touchMove = false;
        /// <summary>
        /// 移动时间
        /// </summary>
        public float moveTime = 0.7f;
        /// <summary>
        /// 上次移动时间
        /// </summary>
        [NonSerialized] public float lastMoveTime = 0;
        /// <summary>
        /// 是否自动移动
        /// </summary>
        public bool autoMove = false;
        /// <summary>
        /// 自动移动时间
        /// </summary>
        public float autoMoveTime = 10f;
        /// <summary>
        /// 当鼠标按下
        /// </summary>
        bool pointerDown = false;
        /// <summary>
        /// 当鼠标抬起
        /// </summary>
        bool pointerUp = false;
        // /// <summary>
        // /// 是否是垂直滚动
        // /// </summary>
        // bool isVertical = false;
        void Start()
        {
            if (!initAtStart) return;
            Init();
        }
        public void Init()
        {
            // if (debuger == null) debuger = UdonDebuger.Instance();
            count = content.childCount;
            // 获得 content 的所有子物体
            childrens = new Image[count];
            for (int i = 0; i < count; i++)
            {
                childrens[i] = (Image)content.GetChild(i).GetComponent(typeof(Image));
                childrens[i].color = i == index ? Color.white : Color.clear;
            }
            if (textureSpliter != null)
            {
                if (avatarPedestal != null) textureSpliter.avatarPedestal = avatarPedestal;
                if (portalMarker != null) textureSpliter.portalMarker = portalMarker;
            }
        }
        void Update()
        {
            if (count <= 1) return;
            if (touchMove)
            {
                touchMove = false;
                // 获得 scrollRect 的水平滚动值
                var x = scrollRect.horizontalNormalizedPosition;
                // 平滑移动到目标位置
                x = Mathf.Lerp(x, target, (Time.time - lastMoveTime) / moveTime);
                Log($"x: {x}");
                scrollRect.horizontalNormalizedPosition = x;
                if (Mathf.Abs(x - target) > 0.001f)
                {
                    touchMove = true;
                }
                else
                {
                    scrollRect.horizontalNormalizedPosition = target;
                    // 目标页颜色为白色，其他页颜色为透明
                    for (int i = 0; i < count; i++)
                    {
                        childrens[i].color = i == index ? Color.white : Color.clear;
                    }
                }
            }
            else if (autoMove && !pointerDown)
            {
                if (Time.time - lastMoveTime > autoMoveTime)
                {
                    Next();
                }
            }
        }
        public void Previous()
        {
            var after = index;
            index = index - 1 < 0 ? count - 1 : index - 1;
            target = (float)index / (count - 1);
            if (Math.Abs(after - index) > 1)
            {
                foreach (var child in childrens)
                {
                    child.color = Color.white;
                }
            }
            else
            {
                childrens[after].color = Color.white;
                childrens[index].color = Color.white;
            }
            lastMoveTime = Time.time;
            touchMove = true;
        }
        public void Next()
        {
            var after = index;
            index = index + 1 >= count ? 0 : index + 1;
            target = (float)index / (count - 1);
            if (Math.Abs(after - index) > 1)
            {
                foreach (var child in childrens)
                {
                    child.color = Color.white;
                }
            }
            else
            {
                childrens[after].color = Color.white;
                childrens[index].color = Color.white;
            }
            lastMoveTime = Time.time;
            touchMove = true;
        }
        #region Pointer
        public void OnPointerDown()
        {
            if (pointerDown) return;
            Log("OnPointerDown");
            pointerDown = true;
            pointerUp = false;
            foreach (var child in childrens)
            {
                child.color = Color.white;
            }
        }
        public void OnPointerUp()
        {
            if (pointerUp) return;
            Log("OnPointerUp");
            pointerUp = true;
            pointerDown = false;
            touchMove = true;
            lastMoveTime = Time.time;
            // 获得 scrollRect 的水平滚动值
            var x = scrollRect.horizontalNormalizedPosition;
            // 滚动值为 0 时为第一页，滚动值为 1 时为最后一页
            Log($"x: {x}");
            // 计算当前位置与哪一页最近
            index = Mathf.RoundToInt(x * (count - 1));
            target = (float)index / (count - 1);
            Log($"index: {index}");
        }
        #endregion
        #region Log
        public void Log(string text) => Log(text, programName);
        public void Log(string text, string _programName = "")
        {
            // if (debuger == null)
            // {
            // Debug.Log($"{_programName}: {text}");
            //     return;
            // }
            // debuger.Log(text, _programName);
        }
        public void LogWarning(string text) => LogWarning(text, programName);
        public void LogWarning(string text, string _programName = "")
        {
            // if (debuger == null)
            // {
            Debug.LogWarning($"{_programName}: {text}");
            //     return;
            // }
            // debuger.LogWarning(text, _programName);
        }
        public void LogError(string text) => LogError(text, programName);
        public void LogError(string text, string _programName = "")
        {
            // if (debuger == null)
            // {
            Debug.LogError($"{_programName}: {text}");
            //     return;
            // }
            // debuger.LogError(text, _programName);
        }
        #endregion
    }
}
