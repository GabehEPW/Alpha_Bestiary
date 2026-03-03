using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using AlphaBestiary.Common.Players;

namespace AlphaBestiary.Items
{
    internal class BestiaryWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Quantas cópias precisa sacrificar pro catálogo criativo
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            // Tamanho do hitbox da sprite
            Item.width = 28;
            Item.height = 28;

            // Cajado/varinha que atira projéteis
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            // Tipo de dano: mágico
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;   // não dá dano de contato
            Item.mana = 8;         // mana por uso

            Item.damage = 24;
            Item.knockBack = 3.2f;

            // Projétil disparado (estrela cadente vanilla)
            Item.shoot = ProjectileID.FallingStar;
            Item.shootSpeed = 8f; // 1f é muito devagar pra magia, 8f fica mais normal

            // Som de varinha mágica
            Item.UseSound = SoundID.Item71;

            // Valor e raridade
            Item.value = Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
        }

        // Bônus de dano baseado no nível do cajado (mesma lógica da espada)
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            var modPlayer = player.GetModPlayer<AlphaBestiaryPlayer>();

            int level = modPlayer.GetWeaponLevel(Item.type);

            // Exemplo: +3% de dano POR nível
            float bonusPerLevel = 0.03f;
            damage *= 1f + level * bonusPerLevel;
        }

        // Tooltip mostrando o nível do cajado e explicação
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<AlphaBestiaryPlayer>();
            int level = modPlayer.GetWeaponLevel(Item.type);

            tooltips.Add(new TooltipLine(Mod, "AlphaBestiaryLevel",
                $"Nível da arma: {level}"));

            tooltips.Add(new TooltipLine(Mod, "AlphaBestiaryHint",
                "Aumenta de nível ao matar tipos diferentes de monstros usando este cajado."));
        }

        public override void AddRecipes()
        {
            // Receita simples pra testar o cajado
            CreateRecipe()
                .AddIngredient(ItemID.FallenStar, 3) // 3 estrelas cadentes
                .AddIngredient(ItemID.Wood, 10)      // 10 madeiras
                .AddTile(TileID.WorkBenches)        // craft na bancada
                .Register();
        }
    }
}
