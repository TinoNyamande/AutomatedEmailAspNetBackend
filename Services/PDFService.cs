using DinkToPdf;
using DinkToPdf.Contracts;

namespace EmailAPI.Services
{
    public class PDFService 
    {
        private readonly IConverter _converter;

        public PDFService(IConverter converter)
        {
            this._converter = converter;
        }
        public byte[] GeneratePdf (string htmlContent,string fileName)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Margins = new MarginSettings{Top = 10, Bottom = 10, Left = 10, Right = 10},
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Landscape,
                DocumentTitle = fileName
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = {DefaultEncoding = "utf-8"},
                HeaderSettings = {FontSize=12,Right="Page [page] of [toPage]",Line=true,Spacing = 2.812},
                FooterSettings = {FontSize =12,Line = true,Right="0 " + DateTime.Now.Year}
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            return _converter.Convert(pdf);
        }
    }
}
