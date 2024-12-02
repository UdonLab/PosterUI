
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace Sonic853.Udon.PosterUI
{
    public class ImageDisplayer : UdonSharpBehaviour
    {
        public MeshRenderer meshRenderer;
        public Material material;
        public string textureName = "_MainTex";
        public Image uiImage;
        public RawImage rawImage;
        /// <summary>
        /// 图集横排数量
        /// </summary>
        public int atlasWidth = 1;
        /// <summary>
        /// 图集纵排数量
        /// </summary>
        public int atlasHeight = 1;
        /// <summary>
        /// 图集中的第几张图（从 0 开始，从左到右，从上到下）
        /// </summary>
        public int atlasIndex = 0;
        /// <summary>
        /// 根据位置获取图片
        /// </summary>
        public bool usePosition = false;
        /// <summary>
        /// 图片位置 X
        /// </summary>
        public int positionX = 0;
        /// <summary>
        /// 图片位置 Y
        /// </summary>
        public int positionY = 0;
        /// <summary>
        /// 图片位置宽度
        /// </summary>
        public int positionWidth = 2048;
        /// <summary>
        /// 图片位置高度
        /// </summary>
        public int positionHeight = 2048;
        public Texture2D texture;
        public bool SetTexture() => SetTexture(texture);
        public bool SetTexture(Texture2D _texture)
        {
            if (meshRenderer != null && meshRenderer.material != null)
            {
                if (string.IsNullOrEmpty(textureName))
                    meshRenderer.material.mainTexture = _texture;
                else
                    meshRenderer.material.SetTexture(textureName, _texture);
            }
            if (material != null)
            {
                if (string.IsNullOrEmpty(textureName))
                    material.mainTexture = _texture;
                else
                    material.SetTexture(textureName, _texture);
            }
            if (uiImage != null)
            {
                uiImage.sprite = Sprite.Create(
                    _texture,
                    new Rect(0, 0, _texture.width, _texture.height),
                    new Vector2(0.5f, 0.5f),
                    100,
                    0,
                    SpriteMeshType.FullRect,
                    new Vector4(0, 0, 0, 0),
                    false
                );
            }
            if (rawImage != null)
            {
                rawImage.texture = _texture;
            }
            return true;
        }
        public Image image;
        public bool SetImage() => SetImage(image);
        public bool SetImage(Image _image)
        {
            if (material != null)
            {
                if (string.IsNullOrEmpty(textureName))
                    material.mainTexture = _image.sprite.texture;
                else
                    material.SetTexture(textureName, _image.sprite.texture);
            }
            if (uiImage != null)
            {
                uiImage.sprite = _image.sprite;
            }
            if (rawImage != null)
            {
                rawImage.texture = _image.sprite.texture;
            }
            return true;
        }
        public Sprite sprite;
        public bool SetSprite() => SetSprite(sprite);
        public bool SetSprite(Sprite _sprite)
        {
            if (material != null)
            {
                if (string.IsNullOrEmpty(textureName))
                    material.mainTexture = _sprite.texture;
                else
                    material.SetTexture(textureName, _sprite.texture);
            }
            if (uiImage != null)
            {
                uiImage.sprite = _sprite;
            }
            if (rawImage != null)
            {
                rawImage.texture = _sprite.texture;
            }
            return true;
        }
    }
}
