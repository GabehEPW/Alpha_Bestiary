using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AlphaBestiary.Items
{
    public class BestiarySword : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Nome e tooltip agora são feitos via Localization (arquivo .hjson).
            // Deixe esse método vazio ou só coisas que não sejam texto.
        }
        public override void SetDefaults()
        {
            Item.DamageType = DamageClass.Melee; // tipo de dano

            Item.damage = 20;
            Item.useStyle = ItemUseStyleID.Rapier; // 3 ≈ estilo de esfaqueada
            Item.useAnimation = 18;
            Item.useTime = 11;
            Item.knockBack = 4f;
            Item.width = 32;
            Item.height = 42;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.value = 0;
            Item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(); // jeito novo de criar receita
            recipe.AddIngredient(ItemID.CrimtaneBar, 7);
            recipe.AddTile(TileID.Anvils);
            recipe.Register(); // substitui SetResult + AddRecipe
        }
    }
}
