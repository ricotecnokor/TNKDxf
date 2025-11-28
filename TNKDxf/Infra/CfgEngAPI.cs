namespace TNKDxf.Infra
{
    public class CfgEngAPI
    {

        public CfgEngAPI()
        {
            //remoto
            //URI = "http://192.168.200.67:8003";

            //local
            //URI = @"https://localhost:7463";

            //mediator
            URI = @"https://localhost:7098";


        }

        public string URI { get; set; } = string.Empty;

    }
}
