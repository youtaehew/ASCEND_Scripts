namespace YTH.Effect
{
    public class BowerEffect : BossEffect
    {
        private void PlayStingEffect()
        {
            PlayEffect(0);
        }

        private void PlayStingEffect_S()
        {
            PlayEffect(1);
        }

        private void PlayChangeEffect()
        {
            PlayEffect(2);
        }

        private void PlayJumpAttack()
        {
            PlayEffect(3);
        }
    }
}

