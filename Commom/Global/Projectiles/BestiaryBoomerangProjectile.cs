using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AlphaBestiary.Projectiles
{
    internal class BestiaryBoomerangProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;

            Projectile.aiStyle = ProjAIStyleID.Boomerang;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;

            Projectile.timeLeft = 300;
        }

        public override void AI()
        {
            Projectile.rotation += 0.4f * Projectile.direction;

            Lighting.AddLight(Projectile.Center, 0.1f, 0.25f, 0.1f);
        }
    }
}