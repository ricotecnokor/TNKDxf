namespace TNKDxf.TeklaManipulacao
{
    public class LeitorRelatorioParams
    {
        public LeitorRelatorioParams(string template, string filename, string title = "", string subtitle = "", string footer = "")
        {
            Template = template;
            Filename = filename;
            Title = title;
            Subtitle = subtitle;
            Footer = footer;
        }

        public string Template { get; set; }
        public string Filename { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Footer { get; set; }

    }
}
