using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
	public class AttackState : State
	{
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
