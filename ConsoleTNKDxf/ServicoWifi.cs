using System;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Threading;

namespace ConsoleTNKDxf
{
    /// <summary>
    /// Verifica se o Windows já possui alguma conexão Wi-Fi ativa.
    /// Se não houver, cria (ou atualiza) o perfil da rede dedicada e conecta por conta própria,
    /// usando a WLAN API nativa (wlanapi.dll).
    /// </summary>
    public static class ServicoWifi
    {
        // ---------- P/Invoke (wlanapi.dll) ----------

        [DllImport("wlanapi.dll")]
        private static extern uint WlanOpenHandle(uint dwClientVersion, IntPtr pReserved, out uint pdwNegotiatedVersion, out IntPtr phClientHandle);

        [DllImport("wlanapi.dll")]
        private static extern uint WlanEnumInterfaces(IntPtr hClientHandle, IntPtr pReserved, out IntPtr ppInterfaceList);

        [DllImport("wlanapi.dll")]
        private static extern uint WlanSetProfile(IntPtr hClientHandle, [In] ref Guid pInterfaceGuid, WLAN_PROFILE_FLAG dwFlags, [MarshalAs(UnmanagedType.LPWStr)] string strProfileXml, [MarshalAs(UnmanagedType.LPWStr)] string strAllUserProfileSecurity, IntPtr bReserved, out uint pdwReasonCode);

        [DllImport("wlanapi.dll")]
        private static extern uint WlanDeleteProfile(IntPtr hClientHandle, [In] ref Guid pInterfaceGuid, [MarshalAs(UnmanagedType.LPWStr)] string strProfileName, IntPtr pReserved);

        [DllImport("wlanapi.dll")]
        private static extern uint WlanConnect(IntPtr hClientHandle, [In] ref Guid pInterfaceGuid, [In] ref WLAN_CONNECTION_PARAMETERS pConnectionParameters);

        [DllImport("wlanapi.dll")]
        private static extern uint WlanFreeMemory(IntPtr pMemory);

        [DllImport("wlanapi.dll")]
        private static extern uint WlanCloseHandle(IntPtr hClientHandle, IntPtr pReserved);

        private const uint ERROR_SUCCESS = 0;
        private const uint ERROR_ACCESS_DENIED = 5;
        private const uint ERROR_ALREADY_EXISTS = 183;

        private enum WLAN_INTERFACE_STATE
        {
            NotReady = 0,
            Connected = 1,
            AdHocNetworkFormed = 2,
            Disconnecting = 3,
            Disconnected = 4,
            Associating = 5,
            Discovering = 6,
            NotAuthenticating = 7,
            NotConnected = 8,
            Authenticating = 9,
            Scanning = 10
        }

        private enum WLAN_PROFILE_FLAG
        {
            User = 0,
            GroupPolicy = 1,
            AllUser = 2
        }

        private enum WLAN_CONNECTION_MODE
        {
            Profile = 0,
            TemporaryProfile = 1,
            DiscoverySecure = 2,
            DiscoveryUnsecure = 3,
            Auto = 4,
            Invalid = 5
        }

        private enum DOT11_BSS_TYPE
        {
            Infrastructure = 1,
            Independent = 2,
            Any = 3
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct WLAN_INTERFACE_INFO
        {
            public Guid InterfaceGuid;
            public WLAN_INTERFACE_STATE isState;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string strInterfaceDescription;
        }

        // Cabeçalho nativo: dwNumberOfItems (uint), dwIndex (uint); os itens WLAN_INTERFACE_INFO vêm inline em seguida.
        [StructLayout(LayoutKind.Sequential)]
        private struct WLAN_INTERFACE_INFO_LIST_HEADER
        {
            public uint dwNumberOfItems;
            public uint dwIndex;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WLAN_CONNECTION_PARAMETERS
        {
            public WLAN_CONNECTION_MODE wlanConnectionMode;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string strProfile;
            public IntPtr pDot11Ssid;
            public IntPtr pDesiredBssidList;
            public DOT11_BSS_TYPE dot11BssType;
        }

        // ---------- Lógica ----------

        public static WifiResultado VerificarEConectar()
        {
            string ssid = ConfigurationManager.AppSettings["Rico3d"];
            string senha = ConfigurationManager.AppSettings["UmsaMija@45"];
            int tempoEspera = ObterTempoEspera();

            if (string.IsNullOrWhiteSpace(ssid))
            {
                return new WifiResultado(false, "Wi-Fi não configurado: WifiSsid vazio no App.config. Nenhuma ação de rede executada.");
            }

            uint versaoNegociada;
            IntPtr handle;
            uint resultado = WlanOpenHandle(2, IntPtr.Zero, out versaoNegociada, out handle);
            if (resultado != ERROR_SUCCESS)
            {
                return new WifiResultado(false, $"Não foi possível abrir a WLAN API (código {resultado}). Verifique se o serviço 'WLAN AutoConfig' está ativo.");
            }

            try
            {
                if (TemWifiAtivo(handle))
                {
                    return new WifiResultado(true, "Wi-Fi do Windows já está ativo. Nenhuma ação necessária.");
                }

                return ConectarNaRede(handle, ssid, senha, tempoEspera);
            }
            finally
            {
                WlanCloseHandle(handle, IntPtr.Zero);
            }
        }

        private static bool TemWifiAtivo(IntPtr handle)
        {
            IntPtr pList;
            if (WlanEnumInterfaces(handle, IntPtr.Zero, out pList) != ERROR_SUCCESS || pList == IntPtr.Zero)
            {
                return false;
            }

            try
            {
                int quantidade = Marshal.ReadInt32(pList);
                int tamanhoInfo = Marshal.SizeOf(typeof(WLAN_INTERFACE_INFO));
                long enderecoBase = pList.ToInt64() + Marshal.SizeOf(typeof(WLAN_INTERFACE_INFO_LIST_HEADER));

                for (int i = 0; i < quantidade; i++)
                {
                    IntPtr pInfo = new IntPtr(enderecoBase + i * tamanhoInfo);
                    WLAN_INTERFACE_INFO info = (WLAN_INTERFACE_INFO)Marshal.PtrToStructure(pInfo, typeof(WLAN_INTERFACE_INFO));
                    if (info.isState == WLAN_INTERFACE_STATE.Connected)
                    {
                        return true;
                    }
                }

                return false;
            }
            finally
            {
                WlanFreeMemory(pList);
            }
        }

        private static WifiResultado ConectarNaRede(IntPtr handle, string ssid, string senha, int tempoEspera)
        {
            IntPtr pList;
            if (WlanEnumInterfaces(handle, IntPtr.Zero, out pList) != ERROR_SUCCESS || pList == IntPtr.Zero)
            {
                return new WifiResultado(false, "Nenhuma interface Wi-Fi encontrada. Verifique se a máquina possui adaptador Wi-Fi ativo.");
            }

            Guid interfaceGuid;
            try
            {
                int quantidade = Marshal.ReadInt32(pList);
                if (quantidade < 1)
                {
                    return new WifiResultado(false, "Nenhuma interface Wi-Fi encontrada. Verifique se a máquina possui adaptador Wi-Fi ativo.");
                }

                int tamanhoInfo = Marshal.SizeOf(typeof(WLAN_INTERFACE_INFO));
                long enderecoBase = pList.ToInt64() + Marshal.SizeOf(typeof(WLAN_INTERFACE_INFO_LIST_HEADER));
                WLAN_INTERFACE_INFO primeira = (WLAN_INTERFACE_INFO)Marshal.PtrToStructure(new IntPtr(enderecoBase), typeof(WLAN_INTERFACE_INFO));
                interfaceGuid = primeira.InterfaceGuid;
            }
            finally
            {
                WlanFreeMemory(pList);
            }

            // 1) Garante que o perfil exista (cria ou atualiza com a senha atual)
            uint codigo = WlanSetProfile(handle, ref interfaceGuid, WLAN_PROFILE_FLAG.AllUser, MontarXmlPerfil(ssid, senha), null, IntPtr.Zero, out _);
            if (codigo == ERROR_ALREADY_EXISTS)
            {
                // Perfil já existe: remove e recria para aplicar a senha atual
                WlanDeleteProfile(handle, ref interfaceGuid, ssid, IntPtr.Zero);
                codigo = WlanSetProfile(handle, ref interfaceGuid, WLAN_PROFILE_FLAG.AllUser, MontarXmlPerfil(ssid, senha), null, IntPtr.Zero, out _);
            }

            if (codigo == ERROR_ACCESS_DENIED)
            {
                return new WifiResultado(false, "Acesso negado ao criar o perfil Wi-Fi. Execute o aplicativo como administrador.");
            }
            if (codigo != ERROR_SUCCESS)
            {
                return new WifiResultado(false, $"Falha ao criar o perfil Wi-Fi '{ssid}' (código {codigo}).");
            }

            // 2) Conecta ao perfil
            WLAN_CONNECTION_PARAMETERS parametros = new WLAN_CONNECTION_PARAMETERS
            {
                wlanConnectionMode = WLAN_CONNECTION_MODE.Profile,
                strProfile = ssid,
                pDot11Ssid = IntPtr.Zero,
                pDesiredBssidList = IntPtr.Zero,
                dot11BssType = DOT11_BSS_TYPE.Any
            };

            codigo = WlanConnect(handle, ref interfaceGuid, ref parametros);
            if (codigo == ERROR_ACCESS_DENIED)
            {
                return new WifiResultado(false, "Acesso negado ao conectar no Wi-Fi. Execute o aplicativo como administrador.");
            }
            if (codigo != ERROR_SUCCESS)
            {
                return new WifiResultado(false, $"Falha ao conectar no Wi-Fi '{ssid}' (código {codigo}).");
            }

            // 3) Aguarda a conexão ficar ativa
            DateTime limite = DateTime.Now.AddSeconds(tempoEspera);
            while (DateTime.Now < limite)
            {
                if (TemWifiAtivo(handle))
                {
                    return new WifiResultado(true, $"Wi-Fi conectado na rede '{ssid}' pelo aplicativo.");
                }
                Thread.Sleep(1000);
            }

            return new WifiResultado(false, $"Tempo esgotado aguardando conexão na rede '{ssid}'. Verifique o SSID e a senha configurados.");
        }

        private static string MontarXmlPerfil(string ssid, string senha)
        {
            string ssidEscapado = System.Security.SecurityElement.Escape(ssid);

            if (string.IsNullOrEmpty(senha))
            {
                // Rede aberta (sem senha)
                return "<?xml version=\"1.0\"?>" +
                       "<WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\">" +
                       "<name>" + ssidEscapado + "</name>" +
                       "<SSIDConfig><SSID><name>" + ssidEscapado + "</name></SSID></SSIDConfig>" +
                       "<connectionType>ESS</connectionType>" +
                       "<connectionMode>auto</connectionMode>" +
                       "<MSM><security>" +
                       "<authEncryption><authentication>open</authentication><encryption>none</encryption><useOneX>false</useOneX></authEncryption>" +
                       "</security></MSM>" +
                       "</WLANProfile>";
            }

            string senhaEscapada = System.Security.SecurityElement.Escape(senha);

            // WPA2-PSK / AES (padrão da maioria das redes dedicadas)
            return "<?xml version=\"1.0\"?>" +
                   "<WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\">" +
                   "<name>" + ssidEscapado + "</name>" +
                   "<SSIDConfig><SSID><name>" + ssidEscapado + "</name></SSID></SSIDConfig>" +
                   "<connectionType>ESS</connectionType>" +
                   "<connectionMode>auto</connectionMode>" +
                   "<MSM><security>" +
                   "<authEncryption><authentication>WPA2PSK</authentication><encryption>AES</encryption><useOneX>false</useOneX></authEncryption>" +
                   "<sharedKey><keyType>passPhrase</keyType><protected>false</protected><keyMaterial>" + senhaEscapada + "</keyMaterial></sharedKey>" +
                   "</security></MSM>" +
                   "</WLANProfile>";
        }

        private static int ObterTempoEspera()
        {
            int valor;
            if (int.TryParse(ConfigurationManager.AppSettings["WifiTempoEsperaSegundos"], out valor) && valor > 0)
            {
                return valor;
            }
            return 30;
        }
    }

    public class WifiResultado
    {
        public bool Conectado { get; }
        public string Mensagem { get; }

        public WifiResultado(bool conectado, string mensagem)
        {
            Conectado = conectado;
            Mensagem = mensagem;
        }
    }
}
