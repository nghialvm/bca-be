using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Reports.Dtos
{
    public class OfferStatisticsDto
    {
        public int TotalOffers { get; set; }

        public int DraftOffers { get; set; }

        public int SentOffers { get; set; }

        public int AcceptedOffers { get; set; }

        public int DeclinedOffers { get; set; }

        public int ExpiredOffers { get; set; }

        public decimal AcceptanceRate { get; set; }
    }
}
