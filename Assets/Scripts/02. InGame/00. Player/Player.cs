using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Player : ObjectBase
    {
        // Start is called before the first frame update

        public float Speed = 10.0f; 
        public Vector3 PlayerDirection;
        
        public override void Start()
        {
            base.Start();
            PlayerDirection = Vector3.forward;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                PlayerDirection = Vector3.forward;
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                PlayerDirection = Vector3.back;
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                PlayerDirection = Vector3.left;
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                PlayerDirection = Vector3.right;
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            }
        }

        private void FixedUpdate()
        {
            transform.position += PlayerDirection * Time.deltaTime * Speed;
        }
    }

}

