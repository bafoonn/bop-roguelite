using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pasta
{
    public class CleavingWeaponAnimations : MonoBehaviour
    {
        [SerializeField] private Transform _weaponPivot;
        [SerializeField] private Transform _attackHandler;

        private SpriteRenderer _weaponRenderer;
        [SerializeField] private Vector2 _idleOffset;
        [SerializeField] private float _idleRotation;
        private float _flippedRotation;
        private Vector2 _aimDir;
        private Vector2 _targetOffset;
        private Vector2 _targetDir;
        private Vector2 _currentDir;
        private Vector2 _currentOffset;
        private bool _swinging = false;
        private bool _flip;
        public bool AnimateSwing = false;
        public float SwingTime = 0.1f, MoveSpeed = 25f, RotateSpeed = 10f, AdditionalAngle = 20f;

        private void Awake()
        {
            _weaponRenderer = _weaponPivot.GetComponentInChildren<SpriteRenderer>();
            _flippedRotation = 180 - _idleRotation;
        }

        private void OnValidate()
        {
            //    if (_weaponPivot != null)
            //    {
            //        _weaponPivot.position = transform.position + (Vector3)_idleOffset;
            //        _weaponPivot.rotation = Quaternion.Euler(0, 0, _idleRotation);
            //    }
        }

        private void Update()
        {
            _weaponRenderer.flipY = _flip;
            if (_swinging) return;

            _flip = Vector2.Dot(Vector2.right, _aimDir) < 0;

            _targetOffset = _idleOffset;
            _targetDir = _aimDir;
            if (_flip) _targetOffset.x = -_targetOffset.x;

            _currentOffset = Vector2.Lerp(_currentOffset, _targetOffset, MoveSpeed * Time.deltaTime);
            _currentDir = Vector2.Lerp(_currentDir, _targetDir, RotateSpeed * Time.deltaTime);

            SetPosition(_currentOffset, _currentDir);
        }

        public void SetAim(Vector2 aim)
        {
            _aimDir = aim.normalized;
        }

        private void SetPosition(Vector2 offset, Vector2 direction)
        {
            _weaponPivot.localPosition = (Vector3)offset;
            _weaponPivot.right = direction.normalized;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + (Vector3)_idleOffset, 0.1f);
        }

        public void Swing(float windup, float winddown, float attackTime, bool clockWise, float angle)
        {
            _flip = !clockWise;

            StopAllCoroutines();
            StartCoroutine(SwingRoutine(windup, winddown, attackTime, !_flip, angle));
        }

        public void StopSwing()
        {
            StopAllCoroutines();
            _swinging = false;
        }

        private IEnumerator SwingRoutine(float windup, float winddown, float attackTime, bool clockWise, float angle)
        {
            _swinging = true;
            angle += AdditionalAngle;
            float timer = 0;
            float distance = 0.5f;
            float min = angle / 2;
            min *= clockWise ? 1 : -1;
            float max = -min;
            float buffer = 0.1f;
            float swingTime = Mathf.Min(SwingTime, attackTime);

            Vector2 offset = UnitCircle.Lerp(min, min * 1.5f, 0, distance, _attackHandler.eulerAngles.z);
            Vector2 current = offset;
            Vector2 target;
            while (timer < windup)
            {
                float t = timer / windup;
                offset = UnitCircle.Lerp(min, min * 1.5f, t * t, distance, _attackHandler.eulerAngles.z);
                target = offset;
                current = Vector2.Lerp(current, target, Time.deltaTime * MoveSpeed);
                SetPosition(current, current);
                timer += Time.deltaTime;
                yield return null;
            }

            if (!AnimateSwing)
            {

                offset = UnitCircle.Lerp(min, max, 1, distance, _attackHandler.eulerAngles.z);
                SetPosition(offset, offset);
                current = offset;
                yield return new WaitForSeconds(attackTime);
            }
            else
            {
                timer = 0;
                while (timer < swingTime)
                {
                    float t = timer / swingTime;
                    offset = UnitCircle.Lerp(min, max, t, distance, _attackHandler.eulerAngles.z);
                    target = offset;
                    current = Vector2.Lerp(current, target, Time.deltaTime * MoveSpeed);
                    SetPosition(current, current);
                    timer += Time.deltaTime;
                    yield return null;
                }
            }

            yield return new WaitForSeconds(attackTime - swingTime + winddown);
            //timer = 0;
            //while (timer < winddown + attackTime - swingTime)
            //{
            //    offset = UnitCircle.Lerp(min, max, 1, distance, _attackHandler.transform.eulerAngles.z);
            //    target = offset;
            //    current = Vector2.Lerp(current, target, Time.deltaTime * 3);
            //    SetPosition(current, current);
            //    timer += Time.deltaTime;
            //    yield return null;
            //}
            yield return new WaitForSeconds(buffer);
            _currentOffset = current;
            _currentDir = current;
            _swinging = false;
        }
    }
}
