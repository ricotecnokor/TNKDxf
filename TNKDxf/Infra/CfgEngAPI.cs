namespace TNKDxf.Infra
{
    public class CfgEngAPI
    {

        public CfgEngAPI()
        {
            //remoto produção ANTIGO
            //URI = "http://192.168.200.67:8003";

            //DEBUG mediator
            //URI = "http://192.168.200.67:7080";

            //DEBUG DIRETO
            URI = @"https://localhost:7463";

            //mediator
            //URI = @"https://localhost:7098";

            //PRODUÇÃO DIRETO
            //URI = "http://192.168.200.67/conversor";

            //PRODUÇÃO INDIRETO
            //URI = "http://192.168.200.67:8040/conversor";

            //HOMOLOGAÇÃO DIRETO
            //URI = "http://192.168.200.67:8080/conversor";


        }

        public string URI { get; set; } = string.Empty;

    }
}
