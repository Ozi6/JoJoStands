using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace JoJoStands.Items
{
    public class TheWorldT2 : ModItem
    {
        public override string Texture
        {
            get { return mod.Name + "/Items/TheWorldT1"; }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The World (Tier 2)");
            Tooltip.SetDefault("Punch enemies at a really fast rate! \nSpecial: Stop time for 2 seconds!\nUsed in Stand Slot");
        }

        public override void SetDefaults()
        {
            item.damage = 42;
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
            TooltipLine tooltipAddition = new TooltipLine(mod, "Speed", "Punch Speed: " + (10 - mPlayer.standSpeedBoosts));
            tooltips.Add(tooltipAddition);
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            mult *= (float)player.GetModPlayer<MyPlayer>().standDamageBoosts;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("TheWorldT1"));
            recipe.AddIngredient(ItemID.HellstoneBar, 25);
            recipe.AddIngredient(ItemID.GoldWatch);
            recipe.AddIngredient(mod.ItemType("WillToFight"));
            recipe.AddIngredient(mod.ItemType("WillToControl"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("TheWorldT1"));
            recipe.AddIngredient(ItemID.HellstoneBar, 25);
            recipe.AddIngredient(ItemID.PlatinumWatch);
            recipe.AddIngredient(mod.ItemType("WillToFight"));
            recipe.AddIngredient(mod.ItemType("WillToControl"));
            recipe.AddTile(mod.TileType("RemixTableTile"));
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
