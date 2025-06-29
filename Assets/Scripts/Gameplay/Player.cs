using System;
using System.Collections.Generic;
using GamePlaySetup;
using UnityEngine;

namespace Gameplay
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigidbody2D;
        [SerializeField] private PlayerConfig playerConfig;
        [SerializeField] private Animator animator;

        private bool canMove;
        private float distance;
        public float Distance => distance;

        public void Move(Vector2 direction)
        {
            if (direction == Vector2.zero || !canMove)
            {
                return;
            }

            Vector2 targetPos = rigidbody2D.position + direction * playerConfig.Speed * Time.fixedDeltaTime;
            rigidbody2D.MovePosition(targetPos);
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);

            distance += direction.magnitude;
        }

        public void Reset()
        {
            distance = 0;
        }

        public void SetMoveState(bool state)
        {
            canMove = state;
        }
    }
}