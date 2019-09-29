/*
 *    日期:
 *    作者:
 *    标题:
 *    功能:
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace RunTimeFrame
{
    public class TypeTest : MonoBehaviour
    {
        public float time;
        public Vector3 pos1 = new Vector3(0, 0, 190);
        public Vector3 pos2 = new Vector3(0, 0, 1000);
        public Vector3 pos3 = new Vector3(0, 0, 1300);
        public float time1=0.5f;
        public float time2 = 1f;
        public float time3 = 0.5f;
        public Ease ease1;
        public Ease ease2;
        public Ease ease3;
        private void Start()
        {

        }
        public void Call1()
        {
            Sequence quence = DOTween.Sequence();
            quence.Append(transform.DOLocalRotate(pos1, time1, RotateMode.LocalAxisAdd).SetEase(ease1));
            quence.Append(transform.DOLocalRotate(pos2, time2, RotateMode.LocalAxisAdd).SetEase(ease2));
            quence.Append(transform.DOLocalRotate(pos3, time3, RotateMode.LocalAxisAdd).SetEase(ease3));
            quence.Play();
            //  Tween tw=  transform.DOLocalRotate(new Vector3(0, 0, -2000), time, RotateMode.FastBeyond360);
            // tw.SetEase(Ease.InOutQuart);

        }
        public void Call2()
        {
            Sequence quence = DOTween.Sequence();
            quence.Append(transform.DOLocalRotate(pos1, time1, RotateMode.LocalAxisAdd).SetEase(ease1));
            quence.Play();

        }
        public void Call3()
        {
            Sequence quence = DOTween.Sequence();
            quence.Append(transform.DOLocalRotate(pos2, time2, RotateMode.LocalAxisAdd).SetEase(ease2));
            quence.Play();
        }
        public void Call4()
        {
            Sequence quence = DOTween.Sequence();
            quence.Append(transform.DOLocalRotate(pos3, time3, RotateMode.LocalAxisAdd).SetEase(ease3));
            quence.Play();

        }
    }
}