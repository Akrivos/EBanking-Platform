using EBanking.Domain.Common;

namespace EBanking.Domain.Entities
{
    public class Currency : BaseEntity
    {
        public decimal AUD { get; private set; }
        public decimal CHF { get; private set; }
        public decimal GBP { get; private set; }
        public decimal USD { get; private set; }
        public DateTime RetrievedAtUtc { get; private set; }

        private Currency() { }

        public Currency(decimal aud, decimal chf, decimal gbp, decimal usd)
        {
            if (aud <= 0)
                throw new ArgumentException("AUD exchange rate must be greater than zero.", nameof(aud));

            if (chf <= 0)
                throw new ArgumentException("CHF exchange rate must be greater than zero.", nameof(chf));

            if (gbp <= 0)
                throw new ArgumentException("GBP exchange rate must be greater than zero.", nameof(gbp));

            if (usd <= 0)
                throw new ArgumentException("USD exchange rate must be greater than zero.", nameof(usd));

            AUD = aud;
            CHF = chf;
            GBP = gbp;
            USD = usd;
            RetrievedAtUtc = DateTime.UtcNow;
        }

    }
}
