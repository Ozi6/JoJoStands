using System;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace JoJoStands.Buffs.AccessoryBuff
{
    public class CenturyBoyBuff : ModBuff
    {
        public int limitTimer = 36000;       //like 10 minutes
        public int breathSave = 0;

        public override void SetDefaults()
        {
			DisplayName.SetDefault("20th Century Boy");
            Description.SetDefault("You are being protected!");
            Main.buffNoTimeDisplay[Type] = true;
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            MyPlayer Mplayer = player.GetModPlayer<MyPlayer>();
            player.controlUseItem = false;
            player.moveSpeed = 0f;
            player.lifeRegen = 0;
            player.immune = true;
            player.manaRegen = 0;
            player.controlUseItem = false;
            player.dash = 0;
            player.controlQuickHeal = false;
            player.controlQuickMana = false;
            player.controlRight = false;
            player.controlUseTile = false;
            player.maxRunSpeed = 0f;
            player.noFallDmg = true;
            limitTimer--;
            if (player.wet && player.ZoneSnow)
            {
                limitTimer -= 6;
                if (breathSave == 0)
                {
                    breathSave = player.breath;
                }
            }
            if (player.wet)
            {
                limitTimer -= 3;
                if (breathSave == 0)
                {
                    breathSave = player.breath;
                }
            }
            if (breathSave != 0)
            {
                player.breath = breathSave;
            }
            if (limitTimer <= 0)
            {
                if (player.wet || (player.wet && player.ZoneSnow))
                {
                    player.KillMe(PlayerDeathReason.ByCustomReason("The water kept it's constant rythm and " + player.name + " has stopped waiting. And stopped thinking."), player.statLife - 1, 1);
                }
                else
                {
                    player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " has stopped thinking."), player.statLife - 1, 1);
                }
            }
            if (Main.mouseRight && Mplayer.StandSlot.Item.type == mod.ItemType("CenturyBoyT2") && player.HasItem(ItemID.Dynamite) && !player.HasBuff(mod.BuffType("AbilityCooldown")))
            {
                player.ConsumeItem(ItemID.Dynamite);
                var explosion = Projectile.NewProjectile(player.position, Vector2.Zero, ProjectileID.GrenadeIII, 49, 8f, Main.myPlayer);
                Main.projectile[explosion].timeLeft = 2;
                Main.projectile[explosion].netUpdate = true;
                Main.PlaySound(SoundID.Item62);
                player.AddBuff(mod.BuffType("AbilityCooldown"), 720);
            }
        }
    }
}