using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using AlphaBestiary.Common.Players;
using AlphaBestiary.Common.Global;

namespace AlphaBestiary.Items
{
    // Cajado mágico que evolui com kills únicas
    internal class BestiaryWand : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Necessário para liberar no modo Journey
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // Faz o player segurar como cajado (animação de staff)
            Item.staff[Type] = true;
        }

        public override void SetDefaults()
        {
            // Tamanho do item
            Item.width = 28;
            Item.height = 28;

            // Dispara projétil
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            // Classe mágica
            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.mana = 8;

            // Dano base
            Item.damage = 24;
            Item.knockBack = 3.2f;

            // Projétil usado
            Item.shoot = ProjectileID.FallingStar;
            Item.shootSpeed = 8f;

            // Som
            Item.UseSound = SoundID.Item71;

            // Valor e raridade
            Item.value = Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // Aumenta dano baseado no nível da arma
            var modPlayer = player.GetModPlayer<AlphaBestiaryPlayer>();
            int level = modPlayer.GetWeaponLevel(Item.type);

            damage *= 1f + level * 0.03f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // Mostra nível da arma no tooltip
            var modPlayer = Main.LocalPlayer.GetModPlayer<AlphaBestiaryPlayer>();
            int level = modPlayer.GetWeaponLevel(Item.type);

            tooltips.Add(new TooltipLine(Mod, "Level",
                $"Nível da arma: {level}"));

            tooltips.Add(new TooltipLine(Mod, "Hint",
                "Evolui ao derrotar diferentes criaturas."));
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Cria o projétil manualmente
            int projIndex = Projectile.NewProjectile(
                source, position, velocity, type, damage, knockback, player.whoAmI
            );

            // Liga o projétil ao sistema do mod
            var proj = Main.projectile[projIndex];
            var globalProj = proj.GetGlobalProjectile<AlphaBestiaryGlobalProjectile>();

            globalProj.sourceItemType = Item.type;
            globalProj.sourcePlayerWhoAmI = player.whoAmI;

            return false; // evita duplicar projétil
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FallenStar, 3)
                .AddIngredient(ItemID.Wood, 10)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}