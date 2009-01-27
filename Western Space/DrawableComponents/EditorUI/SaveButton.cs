﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;

using WesternSpace.ServiceInterfaces;
using WesternSpace.TilingEngine;
using WesternSpace.Utility;

namespace WesternSpace.DrawableComponents.EditorUI
{
    // Quick hack to allow saving of your changes.
    public class SaveButton : EditorUIComponent
    {
        private TileSelector tileSelector;

        public Tile Tile
        {
            get { return tileSelector.Tile; }
        }

        private ITextureService textureService;

        public SaveButton(Game game, SpriteBatch spriteBatch, RectangleF bounds, TileSelector tileSelector)
            : base(game, spriteBatch, bounds)
        {
            this.tileSelector = tileSelector;
            this.textureService = (ITextureService)Game.Services.GetService(typeof(ITextureService));
        }

        #region MOUSE EVENT HANDLERS

        protected override void OnMouseUnclick(int button)
        {
            if (button == 0) // Only handle left clicks, ignore others.
            {
                // Save TileMap here.
                XDocument doc = new XDocument(tileSelector.TileMap.ToXElement());
                doc.Save("TileMapDump.xml");
            }
            base.OnMouseUnclick(button);
        }

        #endregion

    }
}