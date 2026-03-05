using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using AlphaBestiary.Common.Players;

namespace AlphaBestiary.Items
{
    internal class BestiaryGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Quantas cópias precisa sacrificar pro catálogo criativo
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            // Tamanho do hitbox da sprite (metralhadora ~30x15)
            Item.width = 32;
            Item.height = 16;

            // Arma de fogo
            Item.useStyle = ItemUseStyleID.Shoot; // segura e atira
            Item.useTime = 5;                     // tempo entre tiros (menor = mais rápido)
            Item.useAnimation = 5;
            Item.autoReuse = true;                // segurar botão pra metralhar

            Item.DamageType = DamageClass.Ranged;
            Item.noMelee = true;                  // não bate corpo a corpo

            Item.damage = 10;
            Item.knockBack = 1.5f;
            Item.crit = 4;

            // Usa munição de bala
            Item.useAmmo = AmmoID.Bullet;

            // Projétil padrão (caso não tenha munição especial)
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 10f;

            Item.UseSound = SoundID.Item11;       // som de arma de fogo

            Item.value = Item.buyPrice(silver: 80);
            Item.rare = ItemRarityID.Blue;

            // Recuo visual (como o player segura a arma)
            Item.scale = 1f;
        }

        // Bônus de dano baseado no nível da metralhadora
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            var modPlayer = player.GetModPlayer<AlphaBestiaryPlayer>();

            int level = modPlayer.GetWeaponLevel(Item.type);

            // Ex.: +3% de dano por nível (ranged costuma atirar muito rápido)
            float bonusPerLevel = 0.03f;
            damage *= 1f + level * bonusPerLevel;
        }

        // Tooltip mostrando nível e explicação
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<AlphaBestiaryPlayer>();
            int level = modPlayer.GetWeaponLevel(Item.type);

            tooltips.Add(new TooltipLine(Mod, "AlphaBestiaryLevel",
                $"Nível da arma: {level}"));

            tooltips.Add(new TooltipLine(Mod, "AlphaBestiaryHint",
                "Aumenta de nível ao matar tipos diferentes de monstros usando esta metralhadora."));
        }

        public override void AddRecipes()
        {
            // Receita exemplo (ajusta como quiser)
            CreateRecipe()
                .AddIngredient(ItemID.IllegalGunParts, 1)
                .AddIngredient(ItemID.IronBar, 10)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
