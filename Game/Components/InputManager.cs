using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TennisFightingGame
{
	/// <summary>
	/// Handles input for a player.
	/// </summary>
    public class InputManager
    {
        private const float Tap = 0.09f;
        private const float MotionInputDelayPerInput = 0.2f;

        private readonly int[] controls;

        public readonly PlayerIndex index;
        private readonly InputMethod inputMethod;
        private readonly ClearingMode clearingMode;

        public int forwardDirection;
        private List<BufferedInput> buffer = new List<BufferedInput>();
        private GamePadState lastGamePadState;
        private KeyboardState lastKeyboardState;

        public InputManager(PlayerIndex index, ClearingMode clearingMode = ClearingMode.NonHeld,
            int forwardDirection = 1)
        {
            this.index = index;
            this.clearingMode = clearingMode;
            this.forwardDirection = forwardDirection;
            
            // Get controls nad input method from .ini
            controls = TennisFightingGame.ConfigFile.configs[string.Format("P{0} Controls", (int)index + 1)].Values
                .Select(int.Parse)
                .ToArray();
            inputMethod = (InputMethod)TennisFightingGame.ConfigFile.Number(string.Format("P{0} Settings", 
                (int) index + 1), 
                "InputMethod");

            lastKeyboardState = Keyboard.GetState();
            lastGamePadState = GamePad.GetState(index);
        }

		public delegate void DoublePressedEventHandler(Actions action);
		public delegate void HeldEventHandler(Actions action);
		public delegate void PressedEventHandler(Actions action);
		public delegate void ReleasedEventHandler(Actions action);

		public event PressedEventHandler Pressed;
        public event HeldEventHandler Held;
        public event ReleasedEventHandler Released;
        public event DoublePressedEventHandler DoublePressed;

        public void Update()
        {
            // Count buffered time
            foreach (BufferedInput bufferedInput in buffer)
            {
                bufferedInput.bufferedTime += TennisFightingGame.DeltaTime;
            }

            // Count held time
            foreach (BufferedInput bufferedInput in buffer)
            {
                if (InputHeld(bufferedInput.action))
                {
                    bufferedInput.heldTime += TennisFightingGame.DeltaTime;
                }

                // TODO Explain
                if (bufferedInput.bufferedTime > bufferedInput.clearTime && 
                    InputHeld(bufferedInput.action))
                {
                    bufferedInput.bufferedTime = bufferedInput.clearTime - TennisFightingGame.DeltaTime * 2;
                }
            }

            // Add inputs to buffer and remove thingies
            if (inputMethod == InputMethod.Keyboard)
            {
                foreach (Keys key in Keyboard.GetState().GetPressedKeys())
                {
                    Actions action = (Actions)Array.IndexOf(controls, (int)key);

                    if (controls.Contains((int)key) && InputPressed(action))
                    {
                        buffer.Add(new BufferedInput(action));
                    }
                }
            }
            else if (inputMethod == InputMethod.XInput)
            {
                foreach (Buttons button in GetPressedButtons(GamePad.GetState(index)))
                {                
                    Actions action = (Actions)Array.IndexOf(controls, (int)button);

                    if (controls.Contains((int)button) && InputPressed(action))
                    {
                        buffer.Add(new BufferedInput(action));

                        // TODO Explain
                        if (clearingMode == ClearingMode.NonHeld)
                        {
                            buffer.RemoveAll(b => b.action != action && !InputHeld(b.action));
                        }
                        // TODO Explain
                        else if (clearingMode == ClearingMode.NonRepeated)
                        {
                            buffer.RemoveAll(b => b.action != action);
                        }
                    }
                }
            }

            // Remove buffered inputs after clearTime
            for (int i = buffer.Count - 1; i >= 0; i--) //reverse for remove items while iterating
            {
                BufferedInput bufferedInput = buffer[i];
                if (bufferedInput.bufferedTime >= bufferedInput.clearTime &&
                    !InputHeld(bufferedInput.action))
                {
                    buffer.Remove(bufferedInput);
                }
            }

            // Events
            foreach (Actions action in Enum.GetValues(typeof(Actions)))
            {
                if (InputPressed(action))
                {
                    if (Pressed != null)
                    {
                        Pressed.Invoke(action);
                    }
                }

                if (InputHeld(action))
                {
                    if (Held != null)
                    {
                        Held.Invoke(action);
                    }
                }

                if (InputReleased(action))
                {
                    if (Released != null)
                    {
                        Released.Invoke(action);
                    }
                }

                if (InputTwiceBuffered(action))
                {
                    if (DoublePressed != null)
                    {
                        DoublePressed.Invoke(action);
                    }
                }
            }

            // Previous keyboard/gamepads state
            lastKeyboardState = Keyboard.GetState();
            lastGamePadState = GamePad.GetState(index);
        }

        private List<Buttons> GetPressedButtons(GamePadState gamePadState)
        {
            return Enum.GetValues(typeof(Buttons))
                .Cast<Buttons>()
                .Where(button => gamePadState.IsButtonDown(button))
                .ToList();
        }

        public bool MotionInput(Actions[] actions, float chargeTime = 0)
        {
            // Important null check because attacks default to null motionInput
            if (actions == null)
            {
                return true;
            }

            if (buffer.Count < actions.Length)
            {
                return false;
            }

            float totalTime = 0;
            float timeSpan = MotionInputDelayPerInput * actions.Length;

            for (int i = 0; i < actions.Length; i++)
            {
                // TODO Describe direction-aware thingy

                BufferedInput bufferedInput = buffer[buffer.Count() - actions.Length + i];
                Actions directionAwareAction = actions[i];

                if (forwardDirection == -1)
                {
                    if (directionAwareAction == Actions.Right)
                    {
                        directionAwareAction = Actions.Left;
                    }
                    else if (directionAwareAction == Actions.Left)
                    {
                        directionAwareAction = Actions.Right;
                    }
                }

                totalTime += bufferedInput.bufferedTime - bufferedInput.heldTime;
                bool charged = i != 0 || bufferedInput.heldTime >= chargeTime;
                Console.WriteLine(charged);

                if (bufferedInput.action != directionAwareAction ||
                    totalTime > timeSpan ||
                    !charged)
                {
                    return false;
                }
            }

            return true;
        }

        private bool InputPressed(Actions action)
        {
            if (inputMethod == InputMethod.Keyboard)
            {
                Keys key = (Keys)controls[(int)action];
                return Keyboard.GetState().IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
            }
            else if (inputMethod == InputMethod.XInput)
            {
                Buttons button = (Buttons)controls[(int)action];
                return GamePad.GetState(index).IsButtonDown(button) && lastGamePadState.IsButtonUp(button);
            }

            return false;
        }

        private bool InputReleased(Actions action)
        {
            if (inputMethod == InputMethod.Keyboard)
            {
                Keys key = (Keys)controls[(int)action];
                return Keyboard.GetState().IsKeyUp(key) && lastKeyboardState.IsKeyDown(key);
            }
            else if (inputMethod == InputMethod.XInput)
            {
                Buttons button = (Buttons)controls[(int)action];
                return GamePad.GetState(index).IsButtonUp(button) && lastGamePadState.IsButtonDown(button);
            }

            return false;
        }

        private bool InputHeld(Actions action)
        {
            if (inputMethod == InputMethod.Keyboard)
            {
                Keys key = (Keys)controls[(int)action];
                return Keyboard.GetState().IsKeyDown(key) && lastKeyboardState.IsKeyDown(key);
            }
            else if (inputMethod == InputMethod.XInput)
            {
                Buttons button = (Buttons)controls[(int)action];
                return GamePad.GetState(index).IsButtonDown(button) && lastGamePadState.IsButtonDown(button);
            }

            return false;
        }

        private bool InputTwiceBuffered(Actions action)
        {
            return buffer.Count(i => i.action == action) > 1;
        }

        private float InputHeldFor(Actions action)
        {
            BufferedInput bufferedInput = buffer.Find(k => k.action == action);

            if (bufferedInput != null)
            {
                return bufferedInput.heldTime;
            }

            return -1;
        }

        public string ActionBindName(Actions action)
        {
            if (inputMethod == InputMethod.Keyboard)
            {
                return ((Keys)controls[(int) action]).ToString();
            }

            if (inputMethod == InputMethod.XInput)
            {
                return ((Buttons)controls[(int) action]).ToString();
            }

            return "Not set";
        }
	}

	internal class BufferedInput
	{
		public float bufferedTime;
		public float clearTime;
		public float heldTime;
		public Actions action;

		public BufferedInput(Actions action, float clearTime = 0.2f)
		{
			this.action = action;
			this.clearTime = clearTime;
			bufferedTime = 0;
			heldTime = 0;
		}
	}

	/// <summary>
    /// Decides when buffered inputs past their clearTime are disposed of.
	/// </summary>
	public enum ClearingMode
	{
        // All buffered inputs past their clearTime except held ones
        NonHeld, 
        
        // All buffered inputs past their clearTime except ones equivalent to
        // the last input
		NonRepeated
	}

	enum InputMethod
	{
		Keyboard = 0,
		XInput = 1
	}

	public enum Actions
	{
		Up,
		Left,
		Right,
		Down,
		Jump,
		Light,
		Medium,
		Heavy,
        Turn,
		Start
	}
}
