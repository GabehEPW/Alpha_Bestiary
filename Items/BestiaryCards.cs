using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using AlphaBestiary.Common.Global;
using AlphaBestiary.Common.Players;
using AlphaBestiary.Projectiles;

namespace AlphaBestiary.Items
{
    // Cartas mágicas que disparam 4 projéteis
    internal class BestiaryCards : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Necessário para o modo Journey
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            // Tamanho do item
            Item.width = 24;
            Item.height = 28;

            // Classe mágica
            Item.DamageType = DamageClass.Magic;
            Item.damage = 10; // mais fraco que a wand
            Item.knockBack = 1.5f;
            Item.crit = 2;

            // Comportamento de disparo
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.autoReuse = true;

            // Não causa dano melee direto
            Item.noMelee = true;

            // Custo de mana
            Item.mana = 6;

            // Projétil das cartas
            Item.shoot = ModContent.ProjectileType<BestiaryCardProjectile>();
            Item.shootSpeed = 10f;

            // Som ao usar
            Item.UseSound = SoundID.Item8;

            // Valor e raridade
            Item.value = Item.buyPrice(silver: 75);
            Item.rare = ItemRarityID.Blue;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // Aumenta o dano com o nível da arma
            var modPlayer = player.GetModPlayer<AlphaBestiaryPlayer>();
            int level = modPlayer.GetWeaponLevel(Item.type);

            damage *= 1f + level * 0.03f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // Mostra o nível atual da arma
            var modPlayer = Main.LocalPlayer.GetModPlayer<AlphaBestiaryPlayer>();
            int level = modPlayer.GetWeaponLevel(Item.type);

            tooltips.Add(new TooltipLine(Mod, "Level",
                $"Nível da arma: {level}"));

            tooltips.Add(new TooltipLine(Mod, "Hint",
                "Evolui ao derrotar diferentes criaturas."));
        }

        public override bool Shoot(
            Player player,
            EntitySource_ItemUse_WithAmmo source,
            Vector2 position,
            Vector2 velocity,
            int type,
            int damage,
            float knockback)
        {
            // Quantidade de cartas disparadas por uso
            int totalProjectiles = 4;

            // Ângulo total de espalhamento
            float spread = MathHelper.ToRadians(16f);

            for (int i = 0; i < totalProjectiles; i++)
            {
                // Distribui os projéteis em um pequeno leque
                float offset = MathHelper.Lerp(-spread, spread, i / (float)(totalProjectiles - 1));
                Vector2 newVelocity = velocity.RotatedBy(offset);

                int projIndex = Projectile.NewProjectile(
                    source,
                    position,
                    newVelocity,
                    type,
                    damage,
                    knockback,
                    player.whoAmI
                );

                // Liga cada projétil ao sistema do mod
                Projectile proj = Main.projectile[projIndex];
                var globalProj = proj.GetGlobalProjectile<AlphaBestiaryGlobalProjectile>();

                globalProj.sourceItemType = Item.type;
                globalProj.sourcePlayerWhoAmI = player.whoAmI;
            }

            return false; // evita criar projétil extra automaticamente
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 8)
                .AddIngredient(ItemID.FallenStar, 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}