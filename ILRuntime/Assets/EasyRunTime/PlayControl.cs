/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Like
{
    public class PlayControl : MonoBehaviour
    {
        public AnimatorControl animatorControl;
        private CharacterController colliderPlayer;
        private bool isMoveing;
        private Vector3 nor;
        private Vector3 endVector;
        private bool walk = false;
        public void Awake()
        {
            colliderPlayer = gameObject.GetComponent<CharacterController>();
            // animationControl = GetComponent<AnimationControl>();
        }
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                nor = transform.TransformDirection(Vector3.forward);
                walk = true;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                nor = transform.TransformDirection(Vector3.back);
                walk = true;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                nor = transform.TransformDirection(Vector3.left);
                walk = true;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                nor = transform.TransformDirection(Vector3.right);
                walk = true;
            }
            if (walk)
            {
                colliderPlayer.SimpleMove(nor * 2);
                animatorControl.Walk();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                animatorControl.Attack(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                animatorControl.Attack(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                animatorControl.Attack(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                animatorControl.Attack(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                animatorControl.Attack(5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                animatorControl.Attack(6);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                animatorControl.Attack(7);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                animatorControl.Attack(8);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                animatorControl.Attack(9);
            }
            //if (!colliderPlayer.isGrounded)
            //{
            //    Debug.Log("===UpdatePlayerAttribute===");
            //    colliderPlayer.Move(Vector3.up * Time.deltaTime * -10);
            //}

            //if (isMoveing)
            //{
            //    MoveOrientation(nor);
            //    MoveStop(endVector);
            //}
        }
        void MoveOrientation(Vector3 vector)
        {
            Quaternion tranTemp;
            tranTemp = Quaternion.LookRotation(vector);
            float yTemp = tranTemp.eulerAngles.y;
            Quaternion currentRotation = Quaternion.Euler(0, yTemp, 0);
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, currentRotation, Time.deltaTime * 300);

            colliderPlayer.Move(vector * Time.deltaTime * 3);

        }
        void MoveStop(Vector3 vector)
        {
            Vector3 myPing = this.transform.position;
            if (Vector3.Distance(myPing, vector) <= 0.3f)
            {
                isMoveing = false;
                // animationControl.PlayAnimPeaceWait();
            }
        }
    }
}
