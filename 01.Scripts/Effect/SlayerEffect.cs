namespace  YTH.Effect
{
    public class SlayerEffect : BossEffect
    {
        private void PlayChangeEffect()
        {
            PlayEffect(0);
        }

        private void PlayGroundEffect()
        {
            PlayEffect(1);
        }
    }
}

