﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace JoJoStands.UI
{
    internal class BulletCounter : UIState
    {
        public DragableUIPanel bulletCountUI;
        public static bool Visible;
        public static Texture2D bulletCounterTexture;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            bulletCountUI = new DragableUIPanel();
            bulletCountUI.Left.Set(800f, 0f);
            bulletCountUI.Top.Set(510f, 0f);
            bulletCountUI.Width.Set(100f, 0f);
            bulletCountUI.Height.Set(100f, 0f);
            bulletCountUI.BackgroundColor = new Color(0, 0, 0, 0);
            bulletCountUI.BorderColor = new Color(0, 0, 0, 0);

            Append(bulletCountUI);
            base.OnInitialize();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Player player = Main.player[Main.myPlayer];
            MyPlayer mPlayer = player.GetModPlayer<MyPlayer>();
            int frame = mPlayer.revolverBulletsShot;
            int frameHeight = bulletCounterTexture.Height / 7;
            spriteBatch.Draw(bulletCounterTexture, bulletCountUI.GetClippingRectangle(spriteBatch), new Rectangle(0, frameHeight * frame, bulletCounterTexture.Width, frameHeight), new Color(255f, 255f, 255f, 255f));
        }
    }
}