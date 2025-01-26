
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace Sonic853.Udon.PosterUI
{
    public class TextureSpliter : UdonSharpBehaviour
    {
        public Texture2D content;
        public string datasStr;
        DataList _datas;
        [NonSerialized]
        public PosterDisplayer[] displayer;
        [NonSerialized]
        public VRC_AvatarPedestal avatarPedestal;
        [NonSerialized]
        public VRC_PortalMarker portalMarker;
        void Start()
        {
            var count = gameObject.transform.childCount;
            displayer = new PosterDisplayer[count];
            for (int i = 0; i < count; i++)
            {
                displayer[i] = (PosterDisplayer)gameObject.transform.GetChild(i).GetComponent(typeof(UdonBehaviour));
                displayer[i].avatarPedestal = avatarPedestal;
                displayer[i].portalMarker = portalMarker;
            }
        }
        public bool SpliteTexture() => SpliteTexture(content);
        public bool SpliteTexture(Texture2D _content)
        {
            if (_content == null) return false;
            foreach (var item in displayer)
            {
                if (item == null)
                    continue;
                if (!item.usePosition)
                {
                    if (item.atlasWidth < 1
                    || item.atlasHeight < 1)
                        continue;
                    var atlasWidth = item.atlasWidth;
                    var atlasHeight = item.atlasHeight;
                    if (atlasWidth > 1 || atlasHeight > 1)
                    {
                        // 根据 item 里的图集参数计算 UV
                        var atlasIndex = item.atlasIndex >= atlasWidth * atlasHeight ? 0 : item.atlasIndex;
                        // 图集中的第几张图（从 0 开始，从左上到右下）
                        var uv = new Rect(
                            atlasIndex % atlasWidth / (float)atlasWidth,
                            1 - atlasIndex / atlasWidth / (float)atlasHeight - 1f / atlasHeight,
                            1f / atlasWidth,
                            1f / atlasHeight
                        );
                        var sprite = Sprite.Create(
                            _content,
                            new Rect(
                                uv.x * _content.width,
                                uv.y * _content.height,
                                uv.width * _content.width,
                                uv.height * _content.height
                            ),
                            new Vector2(0.5f, 0.5f),
                            100,
                            0,
                            SpriteMeshType.FullRect,
                            new Vector4(0, 0, 0, 0),
                            false
                        );
                        item.SetSprite(sprite);
                    }
                    else
                    {
                        item.SetTexture(_content);
                    }
                }
                else
                {
                    var positionX = item.positionX;
                    var positionY = item.positionY;
                    var positionWidth = item.positionWidth;
                    var positionHeight = item.positionHeight;
                    if (positionX < 0
                    || positionY < 0
                    || positionWidth < 1
                    || positionHeight < 1
                    || positionX + positionWidth > _content.width
                    || positionY + positionHeight > _content.height)
                        continue;
                    var uv = new Rect(
                        positionX / (float)_content.width,
                        1 - (positionY + positionHeight) / (float)_content.height,
                        positionWidth / (float)_content.width,
                        positionHeight / (float)_content.height
                    );
                    var sprite = Sprite.Create(
                        _content,
                        new Rect(
                            uv.x * _content.width,
                            uv.y * _content.height,
                            uv.width * _content.width,
                            uv.height * _content.height
                        ),
                        new Vector2(0.5f, 0.5f),
                        100,
                        0,
                        SpriteMeshType.FullRect,
                        new Vector4(0, 0, 0, 0),
                        false
                    );
                    item.SetSprite(sprite);
                }
            }
            return true;
        }
        public void UpdateData() => UpdateData(datasStr);
        public void UpdateData(string datas)
        {
            if (!VRCJson.TryDeserializeFromJson(datas, out var datasToken)) { return; }
            if (datasToken.TokenType != TokenType.DataList) { return; }
            _datas = datasToken.DataList;
            var datasCount = _datas.Count;
            for (var i = 0; i < datasCount; i++)
            {
                if (!_datas.TryGetValue(i, out var dataToken)) continue;
                if (dataToken.TokenType != TokenType.DataDictionary) continue;
                var dataDictionary = dataToken.DataDictionary;
                if (displayer.Length <= i) continue;
                displayer[i].SetDataDictionary(dataDictionary);
            }
        }
        public void SendFunction() => SpliteTexture();
        public void SendFunctions() => SpliteTexture();
    }
}
