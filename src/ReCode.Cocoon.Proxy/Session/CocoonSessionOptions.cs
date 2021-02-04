namespace ReCode.Cocoon.Proxy.Session
{
    public class CocoonSessionOptions
    {
        public string BackendApiUrl { get; set; }
        
        public string[] Headers { get; set; }
        public string[] Cookies { get; set; }
    }
}