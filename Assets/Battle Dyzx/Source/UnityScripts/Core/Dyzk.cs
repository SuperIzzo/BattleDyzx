using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleDyzx
{
    public class Dyzk : MonoBehaviour
    {        
        public DyzkState dyzkState { get; set; }
        public Spinning spinning { get; private set; }
        public bool debugRenderMovement;
        public bool debugRenderForces;

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
            float scale = dyzkState.dyzkData.size;
            renderer.transform.localScale = new Vector3(scale, scale, scale);

            if (debugRenderMovement)
            {
                // Draw Normal
                Debug.DrawLine(transform.position, transform.position + normal * 100, Color.red);

                // Draw Velocity
                Debug.DrawLine(transform.position, transform.position + velocity * 100, Color.blue);

                // Draw Velocity
                Debug.DrawLine(transform.position, transform.position + control * 100, Color.cyan);
            }

            if (debugRenderForces)
            {
                renderer.material.SetColor("_Color", dyzkState.collisionData.numDyzkCollisions > 0 ? Color.red : Color.white);

                var preservedForce = new Vector3(dyzkState.collisionData.preservedForce.x, 0, dyzkState.collisionData.preservedForce.y);
                var knockbackForce = new Vector3(dyzkState.collisionData.knockbackForce.x, 0, dyzkState.collisionData.knockbackForce.y);
                var tangentForce = new Vector3(dyzkState.collisionData.tangentForce.x, 0, dyzkState.collisionData.tangentForce.y);
                var finalForce = new Vector3(dyzkState.collisionData.finalForce.x, 0, dyzkState.collisionData.finalForce.y);

                Debug.DrawLine(transform.position, transform.position + preservedForce, Color.blue);
                Debug.DrawLine(transform.position, transform.position + knockbackForce, Color.red);
                Debug.DrawLine(transform.position, transform.position + tangentForce, Color.cyan);
                Debug.DrawLine(transform.position, transform.position + finalForce, Color.white);
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