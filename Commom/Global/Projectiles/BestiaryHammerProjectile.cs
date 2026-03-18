using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AlphaBestiary.Projectiles
{
    // Projétil do martelo carregável
    internal class BestiaryHammerProjectile : ModProjectile
    {
        private const int MaxCharge = 60;
        private const float MinSwingSpeed = 8f;
        private const float MaxSwingSpeed = 16f;

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;

            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;

            Projectile.penetrate = 1;
            Projectile.timeLeft = 360;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.ownerHitCheck = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Mata o projétil se o player não estiver válido
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            // Guarda o projétil na mão do player
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            // ai[0] = estado
            // 0 = carregando
            // 1 = atacando

            if (Projectile.ai[0] == 0f)
            {
                // Guarda a direção inicial do golpe
                if (Projectile.localAI[0] == 0f)
                {
                    Vector2 aim = Projectile.velocity;
                    if (aim == Vector2.Zero)
                        aim = new Vector2(player.direction, 0f);

                    aim.Normalize();

                    Projectile.velocity = aim;
                    Projectile.localAI[0] = 1f;
                }

                // Mantém o martelo próximo do player enquanto carrega
                Vector2 holdOffset = Projectile.velocity * 26f;
                Projectile.Center = player.MountedCenter + holdOffset;

                // Rotação visual do martelo
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

                // Conta o tempo de carga em ai[1]
                if (player.channel && Projectile.ai[1] < MaxCharge)
                {
                    Projectile.ai[1]++;
                }
                else
                {
                    // Soltou o botão: começa o ataque
                    float chargePercent = Projectile.ai[1] / MaxCharge;
                    if (chargePercent < 0.2f)
                        chargePercent = 0.2f;

                    float speed = MathHelper.Lerp(MinSwingSpeed, MaxSwingSpeed, chargePercent);

                    // Multiplica o dano conforme a carga
                    Projectile.damage = (int)(Projectile.damage * (1f + chargePercent));

                    Projectile.velocity *= speed;
                    Projectile.friendly = true;
                    Projectile.ai[0] = 1f;
                    Projectile.timeLeft = 20;

                    // Direção do player acompanha o golpe
                    player.direction = Projectile.velocity.X >= 0f ? 1 : -1;
                }
            }
            else
            {
                // Movimento do golpe depois de soltar
                Projectile.rotation += 0.35f * player.direction;

                // Pequena luz para dar presença visual
                Lighting.AddLight(Projectile.Center, 0.15f, 0.12f, 0.08f);
            }

            // Ajusta direção visual do player
            player.ChangeDir(Projectile.velocity.X >= 0f ? 1 : -1);
            player.itemRotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Ao acertar, o martelo desaparece
            Projectile.Kill();
        }
    }
}