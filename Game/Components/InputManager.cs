using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TennisFightingGame
{
	/// <summary>
	/// TODO
	/// </summary>

    public class InputManager
    {
        private const float ClearTime = 0.1f;

        public readonly PlayerIndex index;
        private readonly InputMethod inputMethod;
        private readonly int[] controls;

        public int forwardDirection;
        private List<BufferedInput> buffer = new List<BufferedInput>();
        private float timeToClear;
        private GamePadState lastGamePadState;
        private KeyboardState lastKeyboardState;

        public InputManager(PlayerIndex index, int forwardDirection = 1)
        {
            this.index = index;
            this.forwardDirection = forwardDirection;
            
            // Get controls and input method from .ini
            controls = TennisFightingGame.ConfigFile.configs[string.Format("P{0} Controls", (int)index + 1)].Values
                .Select(int.Parse)
                .ToArray();
            inputMethod = (InputMethod)TennisFightingGame.ConfigFile.Number(
            	string.Format("P{0} Settings", (int) index + 1), "InputMethod");

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
            // Time and heldTime
            bool held = false;

            foreach (BufferedInput input in buffer)
            {
            	if (InputHeld(input.action))
            	{
            		input.heldTime += TennisFightingGame.DeltaTime;
            		held = true;
            	}
            }

            if (!held)
            {
            	timeToClear -= TennisFightingGame.DeltaTime;
            }


            // Add inputs to buffer
            if (inputMethod == InputMethod.Keyboard)
            {
                foreach (Keys key in Keyboard.GetState().GetPressedKeys())
                {
                    Actions action = (Actions)Array.IndexOf(controls, (int)key);

                    if (controls.Contains((int)key) && InputPressed(action))
                    {
                        buffer.Add(new BufferedInput(action));
                        timeToClear = ClearTime;
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
                        timeToClear = ClearTime;
                    }
                }
            }

			// Clear
            if (timeToClear < 0)
            {
                buffer.Clear();
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

                if (InputDoublePressed(action))
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

            for (int i = 0; i < actions.Length; i++)
            {
                BufferedInput input = buffer[buffer.Count() - actions.Length + i];
                Actions action = actions[i];

				// Correct forward and backward diraction
                if (forwardDirection == -1)
                {
                    if (action == Actions.Right)
                    {
                        action = Actions.Left;
                    }
                    else if (action == Actions.Left)
                    {
                        action = Actions.Right;
                    }
                }

                bool charged = i != 0 || input.heldTime >= chargeTime;

                // Add up heldTime of inputs except for the first one
                if (i != 0)
                {
                	totalTime += input.heldTime;
                }

                if (input.action != action ||
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

        private bool InputDoublePressed(Actions action)
        {
        	// HACK This looks like it shouldn't work since we're not
        	// actually checking wether action is the last or the one
        	// before inputs in the buffer
            if (InputHeld(action) && buffer.Count() > 1 &&
            	buffer[buffer.Count() - 1].action == buffer[buffer.Count() - 2].action)
            {
                return true;
            }

            return false;
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
		public float heldTime;
		public Actions action;

		public BufferedInput(Actions action)
		{
			this.action = action;
			heldTime = 0;
		}
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
