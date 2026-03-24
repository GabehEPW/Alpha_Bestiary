using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace BestiaryAlpha.Common.Global.Projectiles
{
    internal class BestiaryWhipProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
            Main.projFrames[Type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();

            Projectile.width = 18;
            Projectile.height = 18;

            Projectile.WhipSettings.Segments = 20;
            Projectile.WhipSettings.RangeMultiplier = 1.0f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;

            // reduz um pouco o dano após atingir, comportamento comum de chicote
            Projectile.damage = (int)(Projectile.damage * 0.7f);
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.10f, 0.25f, 0.12f);
        }
    }
}