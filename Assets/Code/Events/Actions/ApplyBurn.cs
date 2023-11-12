using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class ApplyBurn : EventAction
    {
        private Stat _damage = null;

        protected override void Init()
        {
            base.Init();
            _damage = StatManager.Current.GetStat(StatType.Damage);
        }

        protected override void Trigger(EventContext context)
        {
            base.Trigger(context);
            if (context is HitContext hitContext)
            {
                if (hitContext.Target is ICharacter character)
                {
                    character.Status.ApplyStatus(new BurnStatus(_damage.Value, 1f));
                }
            }
        }
    }
}
