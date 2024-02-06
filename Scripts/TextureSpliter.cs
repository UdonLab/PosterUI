
using UdonSharp;
using UnityEngine;

namespace Sonic853.Udon.PosterUI
{
    public class TextureSpliter : UdonSharpBehaviour
    {
        public Texture2D content;
        public ImageDisplayer[] displayer;
        public bool SpliteTexture()
        {
            if (content == null) return false;
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
                            content,
                            new Rect(
                                uv.x * content.width,
                                uv.y * content.height,
                                uv.width * content.width,
                                uv.height * content.height
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
                        item.SetTexture(content);
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
                    || positionX + positionWidth > content.width
                    || positionY + positionHeight > content.height)
                        continue;
                    var uv = new Rect(
                        positionX / (float)content.width,
                        1 - (positionY + positionHeight) / (float)content.height,
                        positionWidth / (float)content.width,
                        positionHeight / (float)content.height
                    );
                    var sprite = Sprite.Create(
                        content,
                        new Rect(
                            uv.x * content.width,
                            uv.y * content.height,
                            uv.width * content.width,
                            uv.height * content.height
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
        public void SendFunction() => SpliteTexture();
        public void SendFunctions() => SpliteTexture();
    }
}
