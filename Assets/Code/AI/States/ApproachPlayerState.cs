using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
	public class ApproachPlayerState : State
	{
		public AttackState attackState;
		public bool canAttack;
		public override State EnterState()
		{
			return this;
		}

		public override State RunCurrentState()
		{
			return this;
		}
	}
}
