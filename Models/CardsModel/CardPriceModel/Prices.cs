namespace TCGManager.Models.CardModel.CardPriceModel
{
    public class Prices
    {
        private string _usd = "0.00";
        public string usd_foil = "0.00";
        public string usd_etched = "0.00";
        public string eur_foil = "0.00";
        public string tix = "0.00";

        public string usd { get => _usd; set => _usd = value; }

        public Prices()
        {
            this.usd = "0.00";
            this.usd_foil = "0.00";
            this.usd_etched = "0.00";
            this.eur_foil = "0.00";
            this.tix = "0.00";
        }
    }
}