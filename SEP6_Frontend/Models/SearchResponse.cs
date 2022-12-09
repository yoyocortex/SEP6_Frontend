namespace SEP6_Frontend.Models {
    public class SearchResponse {
        public int id { get; set; }
        public string title { get; set; }

        public override string ToString()
        {
            return title;
        }
    }
}