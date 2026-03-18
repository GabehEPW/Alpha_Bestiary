using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AlphaBestiary.Projectiles
{
    // Projétil da lança
    internal class BestiarySpearProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // Marca como lança para o comportamento correto do Terraria
            ProjectileID.Sets.Spears[Type] = true;
        }

        public override void SetDefaults()
        {
            // Tamanho do projétil
            Projectile.width = 18;
            Projectile.height = 18;

            // Faz a lança funcionar como spear vanilla
            Projectile.aiStyle = ProjAIStyleID.Spear;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.DamageType = DamageClass.Melee;

            // A lança some ao acertar
            Projectile.penetrate = -1;

            // Dura pouco tempo porque acompanha a animação do golpe
            Projectile.timeLeft = 90;

            // O player é quem controla a rotação/posição
            Projectile.ownerHitCheck = true;

            // Não colide com tiles durante o avanço do golpe
            Projectile.tileCollide = false;

            Projectile.hide = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Direção do projétil baseada no movimento
            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            }

            // Mantém o player segurando a animação enquanto a lança existe
            player.heldProj = Projectile.whoAmI;
            player.itemTime = player.itemAnimation;
        }
    }
}