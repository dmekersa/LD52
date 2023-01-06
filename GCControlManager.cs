using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gamecodeur
{
    class ControlMethod
    {
        public string Name;
        public List<Keys> ListKeys;

        // Pad
        public bool[][] GamePadButton;

        // Mouse
        public bool MouseButton1;
        public bool MouseButton2;
        public bool MouseButton3;

        // States
        public bool isDown;
        public bool isUp;
        public bool isPressed;

        public ControlMethod()
        {
            ListKeys = new List<Keys>();
            GamePadButton = new bool[2][];
            GamePadButton[0] = new bool[(int)GCControlManager.GamePadButton._MAX];
            GamePadButton[1] = new bool[(int)GCControlManager.GamePadButton._MAX];
        }
    }

    public class GCControlManager
    {
        KeyboardState OldKB;
        MouseState OldMS;
        GamePadState[] OldGPS;
        List<ControlMethod> ListMethods;

        public enum MouseButton
        {
            Button1,
            Button2,
            Button3
        }

        public enum GamePadButton
        {
            A,
            B,
            X,
            Y,
            Left,
            Up,
            Right,
            Down,
            _MAX
        }

        public GCControlManager()
        {
            Reset();
        }

        public void Reset()
        {
            ListMethods = new List<ControlMethod>();
            OldGPS = new GamePadState[2];
        }

        private ControlMethod GetControlMethod(string pMethodName)
        {
            bool bFoundControl = false;
            foreach (var method in ListMethods)
            {
                if (method.Name == pMethodName)
                {
                    bFoundControl = true;
                    return method;
                }
            }
            if (!bFoundControl)
            {
                ControlMethod cm = new ControlMethod();
                cm.Name = pMethodName;
                ListMethods.Add(cm);
                return cm;
            }

            return null;
        }

        public void SetMethodPad(string pMethodName, int pPadNumber, GamePadButton pButton)
        {
            ControlMethod method = GetControlMethod(pMethodName);
            method.GamePadButton[pPadNumber][(int)pButton] = true;
        }

        public void SetMethodMouse(string pMethodName, MouseButton pButton)
        {
            ControlMethod method = GetControlMethod(pMethodName);
            switch (pButton)
            {
                case MouseButton.Button1:
                    method.MouseButton1 = true;
                    break;
                case MouseButton.Button2:
                    method.MouseButton2 = true;
                    break;
                case MouseButton.Button3:
                    method.MouseButton3 = true;
                    break;
                default:
                    break;
            }
        }

        public void SetMethodKey(string pMethodName, Keys key)
        {
            ControlMethod method = GetControlMethod(pMethodName);

            bool bFoundKey = false;
            for (int i = 0; i < method.ListKeys.Count; i++)
            { 
                Keys k = method.ListKeys[i];
                if (k == key)
                {
                    method.ListKeys[i] = key;
                    bFoundKey = true;
                }
            }
            if (!bFoundKey)
            {
                method.ListKeys.Add(key);
            }
        }

        public bool Pressed(string pMethodName)
        {
            foreach (ControlMethod cm in ListMethods)
            {
                if (cm.Name == pMethodName)
                {
                    bool isPressed = cm.isPressed;
                    cm.isPressed = false;
                    return isPressed;
                }
            }
            return false;
        }

        public bool Down(string pMethodName)
        {
            foreach (ControlMethod cm in ListMethods)
            {
                if (cm.Name == pMethodName)
                {
                    return cm.isDown;
                }
            }
            return false;
        }
        public void Update()
        {
            KeyboardState NewKB = Keyboard.GetState();
            MouseState NewMS = Mouse.GetState();
            GamePadState[] NewGPS = new GamePadState[2];
            NewGPS[0] = GamePad.GetState(PlayerIndex.One);
            NewGPS[1] = GamePad.GetState(PlayerIndex.Two);

            ButtonState OldMouseButtonDown = ButtonState.Released;
            ButtonState NewMouseButtonDown = ButtonState.Released;

            bool OldGamePadButtonDown = false;
            bool NewGamePadButtonDown = false;

            foreach (ControlMethod cm in ListMethods)
            {
                cm.isUp = false;
                cm.isDown = false;
                cm.isPressed = false;

                // Pad
                for (int i = 0; i < (int)GamePadButton._MAX; i++)
                {
                    if (cm.GamePadButton[0][i])
                    {
                        if (i == (int)GamePadButton.A)
                        {
                            OldGamePadButtonDown = OldGPS[0].IsButtonDown(Buttons.A);
                            NewGamePadButtonDown = NewGPS[0].IsButtonDown(Buttons.A);
                        }
                        else if (i == (int)GamePadButton.B)
                        {
                            OldGamePadButtonDown = OldGPS[0].IsButtonDown(Buttons.B);
                            NewGamePadButtonDown = NewGPS[0].IsButtonDown(Buttons.B);
                        }
                        if (i == (int)GamePadButton.X)
                        {
                            OldGamePadButtonDown = OldGPS[0].IsButtonDown(Buttons.X);
                            NewGamePadButtonDown = NewGPS[0].IsButtonDown(Buttons.X);
                        }
                        else if (i == (int)GamePadButton.Y)
                        {
                            OldGamePadButtonDown = OldGPS[0].IsButtonDown(Buttons.Y);
                            NewGamePadButtonDown = NewGPS[0].IsButtonDown(Buttons.Y);
                        }
                        else if (i == (int)GamePadButton.Right)
                        {
                            OldGamePadButtonDown = OldGPS[0].DPad.Right == ButtonState.Pressed;
                            NewGamePadButtonDown = NewGPS[0].DPad.Right == ButtonState.Pressed;
                        }
                        else if (i == (int)GamePadButton.Down)
                        {
                            OldGamePadButtonDown = OldGPS[0].DPad.Down == ButtonState.Pressed;
                            NewGamePadButtonDown = NewGPS[0].DPad.Down == ButtonState.Pressed;
                        }
                        else if (i == (int)GamePadButton.Left)
                        {
                            OldGamePadButtonDown = OldGPS[0].DPad.Left == ButtonState.Pressed;
                            NewGamePadButtonDown = NewGPS[0].DPad.Left == ButtonState.Pressed;
                        }
                        else if (i == (int)GamePadButton.Up)
                        {
                            OldGamePadButtonDown = OldGPS[0].DPad.Up == ButtonState.Pressed;
                            NewGamePadButtonDown = NewGPS[0].DPad.Up == ButtonState.Pressed;
                        }
                    }
                }
                if (NewGamePadButtonDown)
                {
                    cm.isDown = true;
                    if (OldGamePadButtonDown == false)
                    {
                        cm.isPressed = true;
                    }
                }
                else
                {
                    if (OldGamePadButtonDown == true)
                    {
                        cm.isUp = true;
                    }
                    else
                    {
                        cm.isUp = false;
                    }
                }

                // Mouse
                bool MouseToTest = false;
                if (cm.MouseButton1)
                {
                    MouseToTest = true;
                    NewMouseButtonDown = NewMS.LeftButton;
                    OldMouseButtonDown = OldMS.LeftButton;
                }
                if (cm.MouseButton2)
                {
                    MouseToTest = true;
                    NewMouseButtonDown = NewMS.RightButton;
                    OldMouseButtonDown = OldMS.RightButton;
                }
                if (cm.MouseButton3)
                {
                    MouseToTest = true;
                    NewMouseButtonDown = NewMS.MiddleButton;
                    OldMouseButtonDown = OldMS.MiddleButton;
                }

                if (MouseToTest)
                {
                    if (NewMouseButtonDown == ButtonState.Pressed)
                    {
                        cm.isDown = true;
                        if (OldMouseButtonDown == ButtonState.Released)
                        {
                            cm.isPressed = true;
                        }
                    }
                    else
                    {
                        if (OldMouseButtonDown == ButtonState.Pressed)
                        {
                            cm.isUp = true;
                        }
                        else
                        {
                            cm.isUp = false;
                        }
                    }
                }

                // Keys
                for (int i = 0; i < cm.ListKeys.Count; i++)
                {
                    Keys k = cm.ListKeys[i];
                    if (NewKB.IsKeyDown(k))
                    {
                        cm.isDown = true;
                        if (!OldKB.IsKeyDown(k))
                        {
                            cm.isPressed = true;
                        }
                    }
                    else
                    {
                        if (cm.isDown)
                        {
                            cm.isUp = true;
                        }
                        else
                        {
                            cm.isUp = false;
                        }
                    }
                }
            }

            OldGPS[0] = NewGPS[0];
            OldGPS[1] = NewGPS[1];
            OldMS = NewMS;
            OldKB = NewKB;
        }
    }
}
