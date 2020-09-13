﻿using System;
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

        private readonly int[] controls;

        public readonly PlayerIndex index;
        private readonly InputMethod inputMethod;
        private readonly ClearingMode clearingMode;

        private List<BufferedInput> buffer = new List<BufferedInput>();
        private GamePadState lastGamePadState;
        private KeyboardState lastKeyboardState;

        public InputManager(PlayerIndex index, 
            ClearingMode clearingMode = ClearingMode.NonHeld)
        {
            this.index = index;
            this.clearingMode = clearingMode;

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
                // Keyboard
                if (inputMethod == InputMethod.Keyboard)
                {
                    if (InputHeld((Keys)bufferedInput.input))
                    {
                        bufferedInput.heldTime += TennisFightingGame.DeltaTime;
                    }
                }
                // XInput
                else if (inputMethod == InputMethod.XInput)
                {
                    if (InputHeld((Buttons)bufferedInput.input))
                    {
                        bufferedInput.heldTime += TennisFightingGame.DeltaTime;
                    }
                }

                // TODO Explain
                if (bufferedInput.bufferedTime > bufferedInput.clearTime && 
                    InputHeld((Keys)bufferedInput.input))
                {
                    bufferedInput.bufferedTime = bufferedInput.clearTime - TennisFightingGame.DeltaTime * 2;
                }
            }

            // Add inputs to buffer and remove thingies
            // Keyboard
            if (inputMethod == InputMethod.Keyboard)
            {
                foreach (Keys key in Keyboard.GetState().GetPressedKeys())
                {
                    if (InputPressed(key) && controls.Contains((int)key))
                    {
                        buffer.Add(new BufferedInput((int)key));

                        // TODO Explain
                        if (clearingMode == ClearingMode.NonHeld)
                        {
                            buffer.RemoveAll(b => b.input != (int)key && !InputHeld((Keys)b.input));
                        }
                        // TODO Explain
                        else if (clearingMode == ClearingMode.NonRepeated)
                        {
                            buffer.RemoveAll(b => b.input != (int)key);
                        }
                    }
                }
            }
            // XInput
            else if (inputMethod == InputMethod.XInput)
            {
                foreach (Buttons button in GetPressedButtons(GamePad.GetState(index)))
                {
                    if (InputPressed(button))
                    {
                        buffer.Add(new BufferedInput((int)button));
                        buffer.RemoveAll(b =>
                            b.input != (int)button &&
                            !InputHeld((Buttons)b.input)); // Dunno if this is a good idea for xinput
                    }
                }
            }

            // Remove buffered inputs after clearTime
            for (int i = buffer.Count - 1; i >= 0; i--) //reverse for remove items while iterating
            {
                BufferedInput bufferedInput = buffer[i];

                // Keyboard
                if (inputMethod == InputMethod.Keyboard)
                {
                    if (bufferedInput.bufferedTime >= bufferedInput.clearTime &&
                        !InputHeld((Keys)bufferedInput.input))
                    {
                        buffer.Remove(bufferedInput);
                    }
                }

                // XInput
                if (inputMethod == InputMethod.XInput)
                {
                    if (bufferedInput.bufferedTime >= bufferedInput.clearTime &&
                        !InputHeld((Buttons)bufferedInput.input))
                    {
                        buffer.Remove(bufferedInput);
                    }
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

        public bool MotionInput(Actions[] actions)
        {
            // Important null check because attacks default to null motionInput
            if (actions == null)
            {
                return true;
            }

            List<BufferedInput> bufferCopy = new List<BufferedInput>(buffer);
            int inputIndex;
            float totalTime = 0;
            float timeSpan = Tap * actions.Length;

            for (int i = 0; i < actions.Length; i++)
            {
                if (inputMethod == InputMethod.Keyboard)
                {
                    inputIndex = bufferCopy.FindIndex(bufferedInput => 
                        (Keys)bufferedInput.input == (Keys)controls[(int)actions[i]]);
                }
                else
                {
                    inputIndex = bufferCopy.FindIndex(bufferedInput => 
                        (Buttons)bufferedInput.input == (Buttons)controls[(int)actions[i]]);
                }

                if (inputIndex != -1)
                {
                    bufferCopy.RemoveRange(0, inputIndex);
                    totalTime += bufferCopy[inputIndex].bufferedTime;
                }
                else
                {
                    return false;
                }

                if (totalTime > timeSpan)
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
                return InputPressed((Keys)controls[(int) action]);
            }

            if (inputMethod == InputMethod.XInput)
            {
                return InputPressed((Buttons)controls[(int) action]);
            }

            return false;
        }
        private bool InputPressed(Keys key) //keyboard key
        {
            return Keyboard.GetState().IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
        }
        private bool InputPressed(Buttons button) //xinput button
        {
            return GamePad.GetState(index).IsButtonDown(button) && lastGamePadState.IsButtonUp(button);
        }

        private bool InputReleased(Actions input)
        {
            if (inputMethod == InputMethod.Keyboard)
            {
                return InputReleased((Keys)controls[(int) input]);
            }

            if (inputMethod == InputMethod.XInput)
            {
                return InputReleased((Buttons)controls[(int) input]);
            }

            return false;
        }
        private bool InputReleased(Keys key)
        {
            return Keyboard.GetState().IsKeyUp(key) && lastKeyboardState.IsKeyDown(key);
        }
        private bool InputReleased(Buttons button)
        {
            return GamePad.GetState(index).IsButtonUp(button) && lastGamePadState.IsButtonDown(button);
        }

        private bool InputHeld(Actions input)
        {
            if (inputMethod == InputMethod.Keyboard)
            {
                return InputHeld((Keys)controls[(int) input]);
            }

            if (inputMethod == InputMethod.XInput)
            {
                return InputHeld((Buttons)controls[(int) input]);
            }

            return false;
        }
        private bool InputHeld(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key) && lastKeyboardState.IsKeyDown(key);
        }
        private bool InputHeld(Buttons button)
        {
            return GamePad.GetState(index).IsButtonDown(button) && lastGamePadState.IsButtonDown(button);
        }

        private bool InputTwiceBuffered(Actions input)
        {
            if (inputMethod == InputMethod.Keyboard)
            {
                return InputTwiceBuffered((Keys)controls[(int) input]);
            }

            if (inputMethod == InputMethod.XInput)
            {
                return InputTwiceBuffered((Buttons)controls[(int) input]);
            }

            return false;
        }
        private bool InputTwiceBuffered(Keys key)
        {
            return buffer.Count(i => i.input == (int) key) > 1;
        }
        private bool InputTwiceBuffered(Buttons button)
        {
            return buffer.Count(i => i.input == (int) button) > 1;
        }

        private float InputHeldFor(Actions input)
        {
            if (inputMethod == InputMethod.Keyboard)
            {
                return InputHeldFor((Keys)controls[(int) input]);
            }

            if (inputMethod == InputMethod.XInput)
            {
                return InputHeldFor((Buttons)controls[(int) input]);
            }

            return -1;
        }
        private float InputHeldFor(Keys key)
        {
            BufferedInput bufferedInput = buffer.Find(k => (Keys)k.input == key);

            if (bufferedInput != null)
            {
                return bufferedInput.heldTime;
            }

            return -1;
        }
        private float InputHeldFor(Buttons button)
        {
            BufferedInput bufferedInput = buffer.Find(b => (Buttons)b.input == button);

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
		public int input;

		public BufferedInput(int input, float clearTime = .2f)
		{
			this.input = input;
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
