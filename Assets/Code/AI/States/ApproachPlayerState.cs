using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
	public class ApproachPlayerState : State
	{
		public AttackState attackState;
		public bool canAttack;
		private Transform player;
		private Transform parent;
		public NewEnemy enemy;
		public bool isClose = false;
		Vector3 moveDirection;
		private Rigidbody2D rbd2d;
		public override State EnterState()
		{
			player = GameObject.FindGameObjectWithTag("Player").transform;
			parent = transform.parent.transform.parent;
			enemy = parent.GetComponent<NewEnemy>();
			rbd2d = parent.GetComponent<Rigidbody2D>();
			return this;
		}

		public override State RunCurrentState()
		{
			if ((enemy.target.transform.position - enemy.transform.position).magnitude < 2.0f)
			{
				isClose = true;
				//Close to player
				// STOP MOVING
			}
			else
			{
				isClose = false;
				Vector3 targetDirection = (player.transform.position - enemy.transform.position).normalized;
				Debug.DrawLine(enemy.transform.position, enemy.transform.position + targetDirection);
				moveDirection = (player.transform.position - enemy.transform.position).normalized;
				moveDirection = Vector3.Lerp(moveDirection, targetDirection, Time.deltaTime * enemy.turnSpeed).normalized;
				//transform.Traslate(moveDirection * Time.deltaTime * speed);
				enemy.transform.position += (moveDirection * Time.deltaTime * enemy.speed);
			}

			if (isClose)
			{
				return attackState;
			}
			else
			{
				return this;
			}
		}
	}
}
