﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WesternSpace.ServiceInterfaces;
using WesternSpace.Interfaces;

namespace WesternSpace.TilingEngine
{
    public class TileMap : DrawableGameObject, IMapCoordinates
    {
        private SpriteBatch sb;

        private int gridCellHeight;

        private int gridCellWidth;

        private Tile[,] tiles;

        private ICameraService camera;

        #region IMapCoordinates Members

        public float MinimumX
        {
            get { return 0; }
        }

        public float MaximumX
        {
            get { return gridCellWidth * tiles.GetLength(0); }
        }

        public float MinimumY
        {
            get { return 0; }
        }

        public float MaximumY
        {
            get { return gridCellHeight * tiles.GetLength(1); }
        }

        #endregion

        public TileMap(Game game, int cellX, int cellY, int tileWidth, int tileHeight)
            : base(game)
        {
            this.Enabled = false;

            sb = new SpriteBatch(game.GraphicsDevice);

            tiles = new Tile[cellX,cellY];
            gridCellWidth = tileWidth;
            gridCellHeight = tileHeight;
        }

        public void SetTile(Tile tile, int x, int y)
        {
            tiles[x,y] = tile;
        }

        public override void Initialize()
        {
            camera = (ICameraService)this.Game.Services.GetService(typeof(ICameraService));

            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Begin(SpriteBlendMode.None, SpriteSortMode.BackToFront, SaveStateMode.None, camera.CurrentViewMatrix);

            for (int x = 0, y = 0; x < tiles.GetLength(0) && y < tiles.GetLength(1); x++)
            {
                Vector2 position = new Vector2(x * gridCellWidth, y * gridCellHeight);

                tiles[x, y].Draw(sb, position);

                if (x != 0 && x % (tiles.GetLength(0) - 1) == 0)
                {
                    y++;
                    x = -1;
                }
            }

            sb.End();

            base.Draw(gameTime);
        }

        #region IMapCoordinates Members

        public Vector2 CalculateMapCoordinatesFromMouse(Vector2 atPoint)
        {
            float x = (camera.Position.X / gridCellWidth) + atPoint.X / gridCellWidth;
            float y = camera.Position.Y + atPoint.Y / gridCellHeight;

            return new Vector2(x, y);
        }

        #endregion
    }
}
