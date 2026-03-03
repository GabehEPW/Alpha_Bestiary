using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using AlphaBestiary.Common.Players;

namespace AlphaBestiary.Items
{
    internal class BestiarySword : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Quantas cópias precisa sacrificar pro catálogo criativo
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            // ajuste fino de tamanho visual
            Item.scale = 1.2f;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 20;
            Item.knockBack = 3.5f;
            Item.crit = 5;

            Item.value = Item.buyPrice(silver: 80, copper: 50);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
        }

        // Aqui aplicamos o bônus de dano baseado no nível da arma
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            var modPlayer = player.GetModPlayer<AlphaBestiaryPlayer>();

            // Pega o nível da arma pra este jogador
            int level = modPlayer.GetWeaponLevel(Item.type);

            // Exemplo: +3% de dano por nível
            // nível 0 -> 1.00x    (sem bônus)
            // nível 1 -> 1.03x
            // nível 10 -> 1.30x
            float bonusPerLevel = 0.03f;
            damage *= 1f + level * bonusPerLevel;
        }

        // Mostra o nível da arma no tooltip (descrição do item)
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<AlphaBestiaryPlayer>();
            int level = modPlayer.GetWeaponLevel(Item.type);

            // Linha mostrando o nível atual
            tooltips.Add(new TooltipLine(Mod, "AlphaBestiaryLevel",
                $"Nível da arma: {level}"));

            // Exemplo de descrição extra explicando como upar
            tooltips.Add(new TooltipLine(Mod, "AlphaBestiaryHint",
                "Aumenta de nível ao matar tipos diferentes de monstros usando esta arma."));
        }

        public override void AddRecipes()
        {
            // Receita simples pra testar a espada
            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 8) // 8 barras de ferro
                .AddTile(TileID.Anvils)          // craft na bigorna
                .Register();
        }
    }
}
