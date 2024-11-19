
namespace WPF_Spotify.MVVM.Model
{
    public class Trend
    {
        public int Amount { get; set; }
        public string View { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public Trend(int amount, bool all=false)
        {
            this.Amount = amount;
            this.View = GetTrendView(all); 
            this.Color = GetTrendColor(all);
            this.Icon = GetTrendIcon(all);
        }
        // Get trend tooltip view
        private string GetTrendView(bool all=false)
        {
            if (this.Amount == -99) return all ? "Old" : "New";
            return this.Amount > 0 ? "+" + this.Amount : this.Amount + "";
        }
        // Get trend color
        private string GetTrendColor(bool all = false)
        {
            //if (this.Amount == -99) return all ? "OrangeRed" : "LightBlue";
            if (this.Amount > 0) return "LightGreen";
            if (this.Amount < 0) return "Red";
            //if (this.Amount == 0) return "yellow";
            return "yellow";
        }
        // Get trend icon
        private string GetTrendIcon(bool all = false)
        {
            if (this.Amount == -99) return "";
            if (this.Amount == 0) return "\uf068";
            if (this.Amount > 0) return "\uf0d8";
            if (this.Amount < 0) return "\uf0d7";
            //return "\uf068";
            return "";
        }
    }
}
