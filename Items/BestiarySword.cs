using Terraria.ID;
using Terraria.ModLoader;

namespace BestiarySword.Items
{
    public class BestiarySword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefaults("Bestiary Sword");
                Tooltip.SetDefaults("A sword made from the knowledge of the bestiary.");
        }
        public override void SetDefaults()
        {
            item.melee = true;
            item.damage = 20;
            item.useStyle = 3;
            item.useAnimation = 18;
            item.useTime = 11;
            item.knockBack = 4f;
            item.width = 32;
            item.height = 42;
            item.rare = 1;
            item.UseSound = SoundID.Item1;
            item.value = 0;
            item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrimtaneBar, 7);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}