using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JoJoStands.Items.Vanities
{
	[AutoloadEquip(EquipType.Body)]
    public class JotaroCoat : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jotaro's Coat");
            Tooltip.SetDefault("Yaaare Yare Daze...");
		}

		public override void SetDefaults()
        {
			item.width = 18;
			item.height = 18;
			item.rare = 6;
			item.vanity = true;
		}

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Silk, 2);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}