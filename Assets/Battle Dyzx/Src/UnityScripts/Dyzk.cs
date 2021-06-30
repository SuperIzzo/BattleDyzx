using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleDyzx
{
    public class Dyzk : MonoBehaviour
    {        
        public DyzkState dyzkState { get; set; }
        public Spinning spinning { get; private set; }
        public bool debugRender;

        public Texture dyzkTexture
        { 
            get
            {
                var renderer = GetComponentInChildren<Renderer>();
                if (renderer)
                {
                    return renderer.sharedMaterial.mainTexture;
                }
                return null;
            }

            set
            {
                var renderer = GetComponentInChildren<Renderer>();
                if (renderer)
                {
                    renderer.material.mainTexture = value;
                }
            }
        }

        public void SetHorizontaInput(float h)
        {
            dyzkState.control.x = h;
        }

        public void SetVerticalInput(float v)
        {
            dyzkState.control.y = v;
        }

        private void Start()
        {
            spinning = GetComponentInChildren<Spinning>();
        }

        private void Update()
        {
            var normal = new Vector3(dyzkState.normal.x, dyzkState.normal.z, dyzkState.normal.y);
            var velocity = new Vector3(dyzkState.velocity.x, dyzkState.velocity.z, dyzkState.velocity.y);
            var control = new Vector3(dyzkState.control.x, dyzkState.control.z, dyzkState.control.y);

            transform.position = new Vector3(dyzkState.position.x, dyzkState.position.z, dyzkState.position.y) + normal*0.01f;            
            transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
            spinning.RPM = dyzkState.RPM;            

            var renderer = GetComponentInChildren<Renderer>();
            renderer.material.SetColor("_Color", dyzkState.isInCollision ? Color.red : Color.white);

            float scale = dyzkState.dyzkData.size;
            renderer.transform.localScale = new Vector3(scale, scale, scale);

            if (debugRender)
            {
                // Draw Normal
                Debug.DrawLine(transform.position, transform.position + normal * 100, Color.red);

                // Draw Velocity
                Debug.DrawLine(transform.position, transform.position + velocity * 100, Color.blue);

                // Draw Velocity
                Debug.DrawLine(transform.position, transform.position + control * 100, Color.cyan);
            }

        }        

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }
    }
}