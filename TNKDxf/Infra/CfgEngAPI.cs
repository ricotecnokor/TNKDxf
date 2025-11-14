namespace TNKDxf.Infra
{
    public class CfgEngAPI
    {

        public CfgEngAPI()
        {
         
            //URI = "http://192.168.200.67:8003";
        
            
            URI = @"https://localhost:7463";
        }

        public string URI { get; set; } = string.Empty;

    }
}
