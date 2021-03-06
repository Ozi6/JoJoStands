using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace JoJoStands.Items
{
	public class StickyFingersFinal : ModItem
	{
        public override string Texture
        {
            get { return mod.Name + "/Items/StickyFingersT1"; }
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sticky Fingers (Final Tier)");
			Tooltip.SetDefault("Punch enemies at a really fast rate and zip them open! Right-click to use an extended punch!\nSpecial: Zip in the direction of your mouse for a distance of 30 tiles!\nUsed in Stand Slot");
		}

        public override void SetDefaults()
        {
            item.damage = 76;
            item.width = 32;
            item.height = 32;
            item.useTime = 12;
            item.useAnimation = 12;
            item.useStyle = 5;
            item.maxStack = 1;
            item.knockBack = 2f;
            item.value = 0;
            item.noUseGraphic = true;
            item.rare = 6;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            MyPlayer mPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
            TooltipLine tooltipAddition = new TooltipLine(mod, "Speed", "Punch Speed: " + (9 - mPlayer.standSpeedBoosts));
            tooltips.Add(tooltipAddition);
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            mult *= (float)player.GetModPlayer<MyPlayer>().standDamageBoosts;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("StickyFingersT3"));
            recipe.AddIngredient(ItemID.Ectoplasm, 4);
            recipe.AddIngredient(mod.ItemType("CaringLifeforce"));
            recipe.AddIngredient(ItemID.LargeSapphire);
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
