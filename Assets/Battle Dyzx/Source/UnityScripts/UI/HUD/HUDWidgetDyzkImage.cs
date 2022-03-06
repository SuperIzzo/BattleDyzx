using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleDyzx
{
    public class HUDWidgetDyzkImage : HUDWidgetBase
    {
        [SerializeField]
        private Image _image;

        private Quaternion _originalRotation;

        private float _rotation = 0.0f;

        private void Start()
        {
            _originalRotation = _image.rectTransform.rotation;
        }

        private void Update()
        {
            if (!dyzk)
            {
                return;
            }

            if (!_image.sprite)
            {
                Texture2D texture = dyzk.dyzkTexture as Texture2D;
                if (texture)
                {
                    _image.sprite = Sprite.Create(
                        texture,
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f));
                }
            }

            _rotation = Time.deltaTime * dyzk.dyzkState.angularVelocity * -3.0f;
            Quaternion zRotation = Quaternion.Euler(0, 0, _rotation);
            _image.rectTransform.rotation *= zRotation;
        }
    }
}