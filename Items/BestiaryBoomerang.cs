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
    // Classe do item em si (o bumerangue no inventário do jogador)
    internal class BestiaryBoomerang : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Quantidade necessária para pesquisa no Journey Mode / catálogo criativo
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            // Tamanho base do item (hitbox/sprite no inventário)
            Item.width = 28;
            Item.height = 28;

            // Estilo de uso:
            // Swing = animação parecida com espada/arremesso
            Item.useStyle = ItemUseStyleID.Swing;

            // Tempo entre usos e duração da animação
            Item.useTime = 20;
            Item.useAnimation = 20;

            // Permite segurar o botão para reutilizar automaticamente
            Item.autoReuse = true;

            // Tipo de dano da arma
            Item.DamageType = DamageClass.Melee;

            // Atributos básicos do dano
            Item.damage = 20;
            Item.knockBack = 4f;
            Item.crit = 5;

            // Não mostra o item na mão enquanto usa
            // porque quem aparece na tela é o projétil do bumerangue
            Item.noUseGraphic = true;

            // Impede dano corpo a corpo direto do item
            // o dano virá apenas do projétil
            Item.noMelee = true;

            // Define qual projétil será lançado
            Item.shoot = ModContent.ProjectileType<BestiaryBoomerangProjectile>();

            // Velocidade inicial do projétil
            Item.shootSpeed = 10f;

            // Som ao usar
            Item.UseSound = SoundID.Item1;

            // Valor de venda/compra e raridade
            Item.value = Item.buyPrice(silver: 80);
            Item.rare = ItemRarityID.Blue;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            // Pega o ModPlayer responsável por salvar o progresso do Alpha Bestiary
            var modPlayer = player.GetModPlayer<AlphaBestiaryPlayer>();

            // Descobre o "nível" da arma com base nas kills únicas registradas
            int level = modPlayer.GetWeaponLevel(Item.type);

            // Bônus de dano por nível
            // Exemplo: nível 1 = +3%, nível 2 = +6%, etc.
            float bonusPerLevel = 0.03f;
            damage *= 1f + level * bonusPerLevel;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // Usa o jogador local para mostrar no tooltip o nível atual da arma
            var modPlayer = Main.LocalPlayer.GetModPlayer<AlphaBestiaryPlayer>();
            int level = modPlayer.GetWeaponLevel(Item.type);

            // Linha mostrando o nível da arma
            tooltips.Add(new TooltipLine(Mod, "AlphaBestiaryLevel",
                $"Nível da arma: {level}"));

            // Linha explicando como a arma evolui
            tooltips.Add(new TooltipLine(Mod, "AlphaBestiaryHint",
                "Aumenta de nível ao matar tipos diferentes de monstros usando este bumerangue."));
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
            // Cria manualmente o projétil do bumerangue
            int projIndex = Projectile.NewProjectile(
                source,
                position,
                velocity,
                type,
                damage,
                knockback,
                player.whoAmI
            );

            // Pega a referência do projétil recém-criado
            Projectile proj = Main.projectile[projIndex];

            // Pega o GlobalProjectile personalizado do mod
            var globalProj = proj.GetGlobalProjectile<AlphaBestiaryGlobalProjectile>();

            // Salva de qual item esse projétil veio
            globalProj.sourceItemType = Item.type;

            // Salva qual jogador lançou esse projétil
            globalProj.sourcePlayerWhoAmI = player.whoAmI;

            // false = impede o Terraria de criar outro projétil automaticamente
            // Como já criamos o projétil manualmente acima, não queremos duplicar
            return false;
        }

        public override bool CanUseItem(Player player)
        {
            // Impede usar o item se já existir um bumerangue desse mesmo tipo ativo do jogador
            // Isso evita spam de múltiplos bumerangues ao mesmo tempo
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active &&
                    Main.projectile[i].owner == player.whoAmI &&
                    Main.projectile[i].type == Item.shoot)
                {
                    return false;
                }
            }

            return true;
        }

        public override void AddRecipes()
        {
            // Receita de criação do bumerangue
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 12)
                .AddIngredient(ItemID.Vine, 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}