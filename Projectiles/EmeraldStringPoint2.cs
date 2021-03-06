using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace JoJoStands.Projectiles
{
    public class EmeraldStringPoint2 : ModProjectile
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/EmeraldStringPoint"; }
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.timeLeft = 600;
            projectile.friendly = true;     //Either a string or an attack that comes from all sides of the screen to the middle
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }

        public bool search = false;
        public int linkWhoAmI = 0;
        public Vector2 collisionLine = Vector2.Zero;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (!search)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].type == mod.ProjectileType("EmeraldStringPoint"))
                    {
                        if (Main.projectile[i].ai[0] == 0f)
                        {
                            linkWhoAmI = Main.projectile[i].whoAmI;
                            Main.projectile[i].ai[0] = 1f;      //meaning, linked
                        }
                    }
                }
                search = true;
            }
            if (search && linkWhoAmI != 0)
            {
                if (projectile.ai[1] == 0f)
                {
                    projectile.timeLeft = 1200;
                    projectile.ai[1] = 1f;
                }
                collisionLine = projectile.position + (Main.projectile[linkWhoAmI].Center - projectile.Center);
                for (int k = 0; k < Main.maxNPCs; k++)
                {
                    NPC npc = Main.npc[k];
                    if (Collision.CheckAABBvLineCollision(npc.Center, new Vector2(npc.width, npc.height), projectile.Center, Main.projectile[linkWhoAmI].Center))
                    {
                        projectile.Kill();
                        Main.projectile[linkWhoAmI].Kill();
                        float numberProjectiles = 6;
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            int proj = Projectile.NewProjectile(player.position.X + (Main.screenWidth / 2) * npc.direction, npc.position.Y + Main.rand.NextFloat(-10f, 11f), (10f * -npc.direction) - Main.rand.NextFloat(0f, 3f), 0f, mod.ProjectileType("Emerald"), 32 + (int)projectile.ai[0], 3f, Main.myPlayer);
                            Main.projectile[proj].netUpdate = true;
                            Main.projectile[proj].tileCollide = false;
                        }
                    }
                }
            }
            if (search && linkWhoAmI == 0)
            {
                projectile.Kill();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity = Vector2.Zero;
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Main.projectile[linkWhoAmI].Kill();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (linkWhoAmI != 0)
            {
                Vector2 linkCenter = Main.projectile[linkWhoAmI].Center;
                Vector2 center = projectile.Center;
                float rotation = (linkCenter - center).ToRotation();
                Texture2D texture = mod.GetTexture("Projectiles/EmeraldString");
                for (float k = 0; k <= 1; k += 1 / (Vector2.Distance(center, linkCenter) / texture.Width))     //basically, getting the amount of space between the 2 points, dividing it by the textures width, then making it a fraction, so saying you 'each takes 1/x space, make x of them to fill it up to 1'
                {
                    Vector2 pos = Vector2.Lerp(center, linkCenter, k) - Main.screenPosition;       //getting the distance and making points by 'k', then bringing it into view
                    spriteBatch.Draw(texture, pos, new Rectangle(0, 0, texture.Width, texture.Height), lightColor, rotation, new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), projectile.scale, SpriteEffects.None, 0f);
                }
            }
            return true;
        }
    }
}