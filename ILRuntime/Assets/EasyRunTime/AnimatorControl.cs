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
    public class AnimatorControl : MonoBehaviour
    {
        public Animator animator;
        private bool walk;
        private Vector3 startPos;
        private Vector3 endPos;
        private Vector3 nor;
        private Vector2 smootDelta = Vector2.zero;
        private Vector2 v = Vector2.zero;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            startPos = transform.localPosition;
        }
        public void Update()
        {
            startPos = transform.position;
            nor = endPos - startPos;
            float dx = Vector3.Dot(transform.right, nor);
            float dy = Vector3.Dot(transform.forward, nor);
            Vector2 deltoposition = new Vector2(dx, dy);
            float smooth = Mathf.Min(1, Time.deltaTime / 0.15f);
            smootDelta = Vector2.Lerp(smootDelta, smootDelta, smooth);

            v = smootDelta / Time.deltaTime;

            bool speed = v.magnitude > 0.5f;
            animator.SetBool("walk", animator);
            animator.SetFloat("inputV", v.x);
            animator.SetFloat("inputH", v.y);
            //Debug.Log("smooth:" + smooth);
            endPos = transform.position;

        }
        public void Walk()
        {
            walk = true;
            // animator.SetFloat("inputV", 0.2f);
        }
        public void StopWalk()
        {
            walk = false;
            //animator.SetFloat("inputV", 0);
        }
        public void Attack(int animatorIndex)
        {
            animator.SetInteger("Attack",animatorIndex);
        }
    }
}
