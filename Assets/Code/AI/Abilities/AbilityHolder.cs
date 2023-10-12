using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class AbilityHolder : MonoBehaviour
    {
        public Ability[] abilities;
        public Ability ability;
        float cooldownTime;
        float activeTime;
        private int random = 0;
        public bool CanUseAbility = false; // <- Here for testing purposes.
        public bool UseAbility = false; // <- Here for testing purposes.
        private bool ActivateAbilityThroughAnim = false;

        private void Start()
        {

            random = UnityEngine.Random.Range(0, abilities.Length);
            ability = abilities[random];
        }

        enum AbilityState
        {
            ready,
            active,
            cooldown
        }
        AbilityState state = AbilityState.ready;
        // Update is called once per frame
        void Update()
        {
            if (CanUseAbility)
            {
                ability = abilities[random];
                switch (state)
                {
                    case AbilityState.ready:
                        if (UseAbility)
                        {
                            if (ability.AbilityWithAnim)
                            {
                                // TODO: PLAY ANIMATION
                                if (ActivateAbilityThroughAnim)
                                {
                                    Debug.Log("Using" + ability);
                                    ability.Activate(gameObject);
                                    state = AbilityState.active;
                                    activeTime = ability.ActiveTime;
                                }
                            }
                            else
                            {
                                Debug.Log("Using" + ability);
                                ability.Activate(gameObject);
                                state = AbilityState.active;
                                activeTime = ability.ActiveTime;
                            }
                           
                        }
                        break;
                    case AbilityState.active:
                        if (activeTime > 0)
                        {
                            activeTime -= Time.deltaTime;
                        }
                        else
                        {
                            ActivateAbilityThroughAnim = false;
                            UseAbility = false; // <- Here for testing purposes.
                            ability.Deactivate();
                            state = AbilityState.cooldown;
                            cooldownTime = ability.coolDown;
                        }
                        break;
                    case AbilityState.cooldown:
                        if (cooldownTime > 0)
                        {
                            cooldownTime -= Time.deltaTime;

                        }
                        else
                        {
                            state = AbilityState.ready;
                        }
                        break;
                }


            }
        }
        public void ActivateAbility()
        {
            ActivateAbilityThroughAnim = true;
        }
    }
}
