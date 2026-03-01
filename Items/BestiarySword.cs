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
            // Tamanho do hitbox da sprite
            Item.width = 32;
            Item.height = 32;

            // Estilo de uso (espada que balança)
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;       // tempo de uso (menor = mais rápido)
            Item.useAnimation = 20;  // duração da animação
            Item.autoReuse = true;   // segura o botão pra bater em loop

            // Valores de dano
            Item.DamageType = DamageClass.Melee; // tipo melee
            Item.damage = 20;                    // dano base
            Item.knockBack = 3.5f;               // recuo
            Item.crit = 5;                       // chance de crítico base

            // Valor e raridade do item
            Item.value = Item.buyPrice(silver: 80, copper: 50);
            Item.rare = ItemRarityID.Blue;

            // Som ao usar
            Item.UseSound = SoundID.Item1;
        }

        // Aqui aplicamos o BÔNUS de dano baseado no "nível" da arma
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            var modPlayer = player.GetModPlayer<AlphaBestiaryPlayer>();

            // Pega o nível da arma pra ESTE jogador
            int level = modPlayer.GetWeaponLevel(Item.type);

            // Exemplo: +3% de dano POR nível
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
