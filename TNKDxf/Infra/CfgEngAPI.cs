namespace TNKDxf.Infra
{
    public class CfgEngAPI
    {

        public CfgEngAPI()
        {
            //URI = "http://localhost:5001";
            //URI = "http://localhost:5114";
            //URI = "http://192.168.200.67:8005";

            //URI = "http://192.168.200.67:8003";
            URI = @"https://localhost:7576";
        }

        public string URI { get; set; } = string.Empty;

    }
}
