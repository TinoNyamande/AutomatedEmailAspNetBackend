namespace EmailAPI.Services
{
    public interface IPDFService
    {
        public byte[] GeneratePdf(string htmlContent,string fileName);

    }
}
