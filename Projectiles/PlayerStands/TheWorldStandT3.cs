using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using JoJoStands.Networking;

namespace JoJoStands.Projectiles.PlayerStands
{
    public class TheWorldStandT3 : StandClass
    {
        public override string Texture
        {
            get { return mod.Name + "/Projectiles/PlayerStands/TheWorldStand"; }
        }

        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 10;
        }


        public override int punchDamage => 68;
        public override int altDamage => 47;
        public override int punchTime => 9;
        public override int halfStandHeight => 44;
        public override float fistWhoAmI => 1f;

        public bool abilityPose = false;
        public int timestopPoseTimer = 0;
        public int updateTimer = 0;

        public override void AI()
        {
            SelectFrame();
            updateTimer++;
            if (shootCount > 0)
            {
                shootCount--;
            }
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            if (modPlayer.StandOut)
            {
                projectile.timeLeft = 2;
            }
            if (updateTimer >= 90)      //an automatic netUpdate so that if something goes wrong it'll at least fix in about a second
            {
                updateTimer = 0;
                projectile.netUpdate = true;
            }
            if (JoJoStands.SpecialHotKey.JustPressed && !player.HasBuff(mod.BuffType("AbilityCooldown")) && !player.HasBuff(mod.BuffType("TheWorldBuff")) && projectile.owner == Main.myPlayer)
            {
                timestopPoseTimer = 60;
                Timestop(5);
            }

            if (!modPlayer.StandAutoMode)
            {
                if (Main.mouseLeft && projectile.owner == Main.myPlayer)
                {
                    Punch();
                }
                else
                {
                    if (player.whoAmI == Main.myPlayer)
                        attackFrames = false;
                }
                if (!attackFrames)
                {
                    if (!secondaryAbilityFrames)
                    {
                        StayBehind();
                        projectile.direction = projectile.spriteDirection = player.direction;
                    }
                    else
                    {
                        GoInFront();
                        if (Main.MouseWorld.X > projectile.position.X)
                        {
                            projectile.spriteDirection = 1;
                            projectile.direction = 1;
                        }
                        if (Main.MouseWorld.X < projectile.position.X)
                        {
                            projectile.spriteDirection = -1;
                            projectile.direction = -1;
                        }
                        secondaryAbilityFrames = false;
                    }
                }
                if (Main.mouseRight && player.HasItem(mod.ItemType("Knife")) && projectile.owner == Main.myPlayer)
                {
                    Main.mouseLeft = false;
                    secondaryAbilityFrames = true;
                    normalFrames = false;
                    attackFrames = false;
                    if (shootCount <= 0 && projectile.frame == 9)
                    {
                        shootCount += 28;
                        float rotationk = MathHelper.ToRadians(15);
                        float numberKnives = 3;
                        Vector2 shootVel = Main.MouseWorld - projectile.Center;
                        if (shootVel == Vector2.Zero)
                        {
                            shootVel = new Vector2(0f, 1f);
                        }
                        shootVel.Normalize();
                        shootVel *= 100f;
                        for (int i = 0; i < numberKnives; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(shootVel.X, shootVel.Y).RotatedBy(MathHelper.Lerp(-rotationk, rotationk, i / (numberKnives - 1))) * .2f;
                            int proj = Projectile.NewProjectile(projectile.position.X + 5f, projectile.position.Y - 3f, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("Knife"), (int)(altDamage * modPlayer.standDamageBoosts), 2f, player.whoAmI);
                            Main.projectile[proj].netUpdate = true;
                            projectile.netUpdate = true;
                        }
                        player.ConsumeItem(mod.ItemType("Knife"));
                        player.ConsumeItem(mod.ItemType("Knife"));
                        player.ConsumeItem(mod.ItemType("Knife"));
                    }
                }
                if (timestopPoseTimer > 0)
                {
                    timestopPoseTimer--;
                    normalFrames = false;
                    attackFrames = false;
                    secondaryAbilityFrames = false;
                    abilityPose = true;
                    Main.mouseLeft = false;
                    Main.mouseRight = false;
                    if (timestopPoseTimer <= 1)
                    {
                        abilityPose = false;
                    }
                }
            }
            if (modPlayer.StandAutoMode)
            {
                PunchAndShootAI(mod.ProjectileType("Knife"));
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(abilityPose);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            abilityPose = reader.ReadBoolean();
        }

        public virtual void SelectFrame()
        {
            Player player = Main.player[projectile.owner];
            projectile.frameCounter++;
            if (attackFrames)
            {
                normalFrames = false;
                secondaryAbilityFrames = false;
                if (projectile.frameCounter >= punchTime - player.GetModPlayer<MyPlayer>().standSpeedBoosts)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame <= 1)
                {
                    projectile.frame = 2;
                }
                if (projectile.frame >= 6)
                {
                    projectile.frame = 2;
                }
            }
            if (normalFrames)
            {
                attackFrames = false;
                secondaryAbilityFrames = false;
                if (projectile.frameCounter >= 30)
                {
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 2)
                {
                    projectile.frame = 0;
                }
            }
            if (secondaryAbilityFrames)
            {
                if (projectile.frameCounter >= 28)
                {
                    if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().StandAutoMode && projectile.frame == 9)
                    {
                        secondaryAbility = false;
                    }
                    projectile.frame += 1;
                    projectile.frameCounter = 0;
                }
                if (projectile.frame >= 10)
                {
                    projectile.frame = 8;
                }
                if (projectile.frame <= 7)
                {
                    projectile.frame = 8;
                }
            }
            if (abilityPose)
            {
                projectile.frame = 6;
            }
            if (Main.player[projectile.owner].GetModPlayer<MyPlayer>().poseMode)
            {
                normalFrames = false;
                attackFrames = false;
                secondaryAbilityFrames = false;
                projectile.frame = 7;
            }
        }
    }
}