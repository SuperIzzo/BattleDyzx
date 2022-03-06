using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleDyzx
{
    public class DyzkUIImage : MonoBehaviour
    {
        [SerializeField]
        private int _playerId = -1;

        [SerializeField]
        private Image _image;

        public DyzkController dyzkController
        {
            get => BattleManager.instance?.GetDyzkController(_playerId);
            set { _playerId = value ? value.playerId : -1; }
        }

        public Dyzk dyzk { get => dyzkController?.dyzk; }

        private Quaternion _originalRotation;
        float _rotation = 0.0f;

        // Start is called before the first frame update
        void Start()
        {
            _originalRotation = _image.rectTransform.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            if (dyzk)
            {
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

                _rotation = Time.deltaTime * dyzk.dyzkState.angularVelocity * -1.0f;
                Quaternion zRotation = Quaternion.Euler(0, 0, _rotation);
                _image.rectTransform.rotation *= zRotation;
            }
        }
    }
}