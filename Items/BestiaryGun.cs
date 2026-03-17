using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;
using AlphaBestiary.Common.Players;
using AlphaBestiary.Common.Global;

namespace AlphaBestiary.Items
{
    // Classe da arma BestiaryGun
    internal class BestiaryGun : ModItem
    {
        // Escala forçada para desenhar a sprite menor no inventário e no mundo
        private const float ForcedDrawScale = 0.03f;

        public override void SetStaticDefaults()
        {
            // Quantidade necessária para pesquisa no catálogo criativo
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            // Tamanho base do item
            Item.width = 30;
            Item.height = 15;

            // Dano e classe da arma
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 1.5f;
            Item.crit = 4;

            // Configurações de disparo
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.reuseDelay = 0;
            Item.autoReuse = true;
            Item.useTurn = false;

            // Não causa dano corpo a corpo direto
            Item.noMelee = true;

            // A arma continua aparecendo na mão do jogador
            Item.noUseGraphic = false;

            // Define munição usada e projétil padrão
            Item.useAmmo = AmmoID.Bullet;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 10f;

            // Som do disparo
            Item.UseSound = SoundID.Item11;

            // Valor e raridade
            Item.value = Item.buyPrice(silver: 80);
            Item.rare = ItemRarityID.Blue;

            // Escala geral do item
            Item.scale = 1f;
        }

        public override Vector2? HoldoutOffset()
        {
            // Ajusta a posição visual da arma quando segurada
            return new Vector2(-6f, 0f);
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // Recupera o progresso do jogador para o sistema Alpha Bestiary
            var modPlayer = player.GetModPlayer<AlphaBestiaryPlayer>();

            // Nível atual da arma
            int level = modPlayer.GetWeaponLevel(Item.type);

            // Aplica bônus de dano por nível
            float bonusPerLevel = 0.03f;
            damage *= 1f + level * bonusPerLevel;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // Mostra o nível atual da arma no tooltip
            var modPlayer = Main.LocalPlayer.GetModPlayer<AlphaBestiaryPlayer>();
            int level = modPlayer.GetWeaponLevel(Item.type);

            tooltips.Add(new TooltipLine(Mod, "AlphaBestiaryLevel",
                $"Nível da arma: {level}"));

            tooltips.Add(new TooltipLine(Mod, "AlphaBestiaryHint",
                "Aumenta de nível ao matar tipos diferentes de monstros usando esta metralhadora."));
        }

        public override void AddRecipes()
        {
            // Receita da arma
            CreateRecipe()
                .AddIngredient(ItemID.IllegalGunParts, 1)
                .AddIngredient(ItemID.IronBar, 10)
                .AddTile(TileID.Anvils)
                .Register();
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
            // Cria manualmente o projétil da arma
            int projIndex = Projectile.NewProjectile(
                source,
                position,
                velocity,
                type,
                damage,
                knockback,
                player.whoAmI
            );

            // Pega o projétil criado
            Projectile proj = Main.projectile[projIndex];

            // Pega o GlobalProjectile do mod
            var globalProj = proj.GetGlobalProjectile<AlphaBestiaryGlobalProjectile>();

            // Guarda qual item gerou o tiro
            globalProj.sourceItemType = Item.type;

            // Guarda qual player disparou
            globalProj.sourcePlayerWhoAmI = player.whoAmI;

            // Retorna false para não duplicar o projétil
            return false;
        }

        public override bool PreDrawInInventory(
            SpriteBatch spriteBatch,
            Vector2 position,
            Rectangle frame,
            Color drawColor,
            Color itemColor,
            Vector2 origin,
            float scale)
        {
            // Pega a textura real do item
            Texture2D texture = TextureAssets.Item[Type].Value;

            // Desenha manualmente a arma no inventário com escala personalizada
            spriteBatch.Draw(
                texture,
                position,
                null,
                drawColor,
                0f,
                texture.Size() * 0.5f,
                ForcedDrawScale,
                SpriteEffects.None,
                0f
            );

            // false = impede o desenho padrão do Terraria
            return false;
        }

        public override bool PreDrawInWorld(
            SpriteBatch spriteBatch,
            Color lightColor,
            Color alphaColor,
            ref float rotation,
            ref float scale,
            int whoAmI)
        {
            // Pega a textura do item
            Texture2D texture = TextureAssets.Item[Type].Value;

            // Converte a posição do mundo para a tela
            Vector2 position = Item.Center - Main.screenPosition;

            // Desenha manualmente a arma no chão com a escala desejada
            spriteBatch.Draw(
                texture,
                position,
                null,
                lightColor,
                rotation,
                texture.Size() * 0.5f,
                ForcedDrawScale,
                SpriteEffects.None,
                0f
            );

            // false = impede o desenho padrão do Terraria
            return false;
        }
    }
}