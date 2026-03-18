using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AlphaBestiary.Projectiles
{
    // Projétil das cartas mágicas
    internal class BestiaryCardProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            // Tamanho do projétil
            Projectile.width = 14;
            Projectile.height = 18;

            // Dano e comportamento
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;

            // Quantos inimigos pode atravessar
            Projectile.penetrate = 1;

            // Duração do projétil
            Projectile.timeLeft = 180;

            // Colisão com blocos
            Projectile.tileCollide = true;

            // Ignora água
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            // Faz a carta girar na direção do movimento
            if (Projectile.velocity != Vector2.Zero)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }

            // Luz suave
            Lighting.AddLight(Projectile.Center, 0.15f, 0.20f, 0.30f);

            // Poeira opcional
            Dust dust = Dust.NewDustDirect(
                Projectile.position,
                Projectile.width,
                Projectile.height,
                DustID.Enchanted_Gold
            );

            dust.scale = 0.8f;
            dust.velocity *= 0.2f;
            dust.noGravity = true;
        }
    }
}