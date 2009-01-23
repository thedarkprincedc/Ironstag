﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using WesternSpace.ServiceInterfaces;

namespace WesternSpace.Input
{
    // Our version of MouseState. Much more useful.
    public class BetterMouse : GameObject
    {
        // Where we get our mouse data from:
        IInputManagerService inputManager;

        private MouseState previousState;

        // button 0 - Left Click
        // button 1 - Middle Click
        // button 2 - Right Click

        private bool[] buttonsClicked;

        public bool[] ButtonsClicked
        {
            get { return buttonsClicked; }
        }

        private bool[] buttonsHeld;

        public bool[] ButtonsHeld
        {
            get { return buttonsHeld; }
        }

        // This refers to buttons that were *just* released.
        private bool[] buttonsUnclicked;

        public bool[] ButtonsUnclicked
        {
            get { return buttonsUnclicked; }
        }

        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        private Vector2 motion;

        public Vector2 Motion
        {
            get { return motion; }
        }

        private int scrollAmount;

        public int ScrollAmount
        {
            get { return scrollAmount; }
        }


        public BetterMouse(Game game)
            : base(game)
        {
            buttonsClicked = new bool[3];
            buttonsHeld = new bool[3];
            buttonsUnclicked = new bool[3];
            position = new Vector2();
            motion = new Vector2();
        }

        public override void Initialize()
        {
            inputManager = (IInputManagerService)Game.Services.GetService(typeof(IInputManagerService));
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            MouseState currentState = inputManager.MouseState;

            ButtonState[] previousButtons = new ButtonState[3];
            ButtonState[] currentButtons = new ButtonState[3];

            previousButtons[0] = previousState.LeftButton;
            previousButtons[1] = previousState.MiddleButton;
            previousButtons[2] = previousState.RightButton;

            currentButtons[0] = previousState.LeftButton;
            currentButtons[1] = previousState.MiddleButton;
            currentButtons[2] = previousState.RightButton;

            for(int i = 0; i < 3; ++i)
            {
                buttonsUnclicked[i] = false;
                if ( currentButtons[i] == ButtonState.Pressed)
                {
                    buttonsClicked[i] = !buttonsHeld[i];
                    buttonsHeld[i] = true;
                }
                else
                {
                    if (buttonsHeld[i])
                    {
                        buttonsUnclicked[i] = true;
                    }
                    buttonsHeld[i] = buttonsClicked[i] = false;
                }
            }

            scrollAmount = currentState.ScrollWheelValue - previousState.ScrollWheelValue;
            motion = new Vector2(currentState.X, currentState.Y) - position;
            position = new Vector2(currentState.X, currentState.Y);
            previousState = currentState;
            base.Update(gameTime);
        }
    }
}