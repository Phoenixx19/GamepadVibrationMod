using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.MiscEntities.WorldItems;
using JumpKing.MiscEntities.WorldItems.Inventory;
using JumpKing.Player;
using JumpKing_GamepadVibration.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace JumpKing_GamepadVibration.Behaviour
{
    public class Vibration : IBodyCompBehaviour
    {
        private const PlayerIndex PLAYER_ONE = PlayerIndex.One;
        private bool isLanded = false;
        private bool setKnocked = false;

        public bool ExecuteBehaviour(BehaviourContext behaviourContext)
        {
            // if its not enabled, you can skip this
            if (!Preference.Preferences.IsEnabled)
                return true;

            BodyComp bodyComp = behaviourContext.BodyComp;

            // landed on ground vibration, if toggled
            if (Preference.Preferences.IsLanded)
            {
                LandedVibration(bodyComp);
            }

            // knocked vibration, if toggled
            if (Preference.Preferences.IsKnocked)
            {
                KnockedVibration(behaviourContext);
            }

            return true;
        }

        private void LandedVibration(BodyComp bodyComp)
        {
            float power = 0f;
            if (bodyComp.IsOnGround)
            {
                if (isLanded)
                {
                    power = InventoryManager.HasItemEnabled(Items.GiantBoots) && Preference.Preferences.GiantBootsPower ? 1.0f : 0.3f;
                    isLanded = false;
                }
            }
            else
            {
                isLanded = true;
            }

            TrySetVibration(power, power);
        }

        private void KnockedVibration(BehaviourContext behaviourContext)
        {
            if (behaviourContext.ContainsKey("PlayBumpSFX"))
            {
                setKnocked = true;
                TrySetVibration(0f, 0f);
            }
            else
            {
                if (setKnocked)
                {
                    TrySetVibration(0.3f, 0.3f);
                    setKnocked = false;

                }
                else
                {
                    TrySetVibration(0f, 0f);
                }
            }
        }

        private float left;
        private float right;

        private void TrySetVibration(float leftMotor, float rightMotor)
        {
            // dont call the vibration method too many times for no reason
            if (leftMotor == this.left && rightMotor == this.right)
                return;
#if DEBUG
            Debug.WriteLine($"Setting vibration to LEFT: {leftMotor} & RIGHT: {rightMotor}");
#endif
            GamePad.SetVibration(PLAYER_ONE, leftMotor, rightMotor);

            left = leftMotor;
            right = rightMotor;
        }
    }
}
