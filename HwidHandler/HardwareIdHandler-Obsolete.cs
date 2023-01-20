using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.Management;

namespace HwidHandlerObsolete
{
    /// <summary>
    /// this class allow to manage a Hardware Id
    /// </summary>
    internal class HardwareIdHandler
    {
        /// <summary>
        /// class field to set the activ HwId
        /// </summary>
        public readonly string HwId;

        /// <summary>
        /// class field to set (url / file path(not implemented) ) the source of allow HwId List
        /// </summary>
        public string sourceOfHwidList { get; set; }

        /// <summary>
        /// private properti HttpClient that allow to do http request
        /// </summary>
        private static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// constructor of class without parameters
        /// </summary>
        public HardwareIdHandler() { }

        /// <summary>
        /// constructor of class
        /// </summary>
        /// <param name="HwId">String to set the activ HwId</param>
        /// <param name="sourceOfHwidList">String (url) the source of allow HwId List</param>
        public HardwareIdHandler(string HwId, string sourceOfHwidList)
        {
            this.HwId = HwId;
            this.sourceOfHwidList = sourceOfHwidList;
        }

        /// <summary>
        /// constructor of class
        /// </summary>
        /// <param name="UserHwId">String to set the activ HwId</param>
        public HardwareIdHandler(string HwId)
        {
            this.HwId = HwId;
        }

        /// <summary>
        /// using the computer propriets create a hash string, that allow to have a unique id and not allow to read Hardware Properties of the PC
        /// </summary>
        /// <returns>String Hardware Id</returns>
        public String GenerateHwId()
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                HashAlgorithm hashAlgorithm = sha256Hash;
                var hash = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(string.Join("", GetHardwareProperties())));
                var hardwareId = string.Join("", hash.Select(x => x.ToString("X2")));

                return hardwareId;
            }
        }

        /// <summary>
        /// check if the HwId is in the allow HwId List
        /// </summary>
        /// <param name="Hwid">String HwId to check</param>
        /// <param name="url">String url of allow HwId List</param>
        /// <returns>true if HwId is Allow, false if HwId is not allow</returns>
        /// <exception cref="System.Exception"> You Have to set parameters</exception>
        public async Task<bool> HwIdIsAllowedAsync(string Hwid, string url)
        {
            if (null == Hwid)
            {
                throw new Exception("parameters hwid null");
            }
            if (null == url)
            {
                throw new Exception("parameters url null");
            }
            var isAllowed = false;

            var stringAllHwId = await ReadUrlAsStringAsync(url);

            List<string> hwIdList = ManipulateString(stringAllHwId);

            foreach (var HwIdOfList in hwIdList)
            {
                if (HwIdOfList == Hwid)
                {
                    isAllowed = true;
                    break;
                }
            }

            return isAllowed;
        }

        /// <summary>
        /// check if the HwId is in the allow HwId List (version only HwId parameters, url )
        /// </summary>
        /// <param name="Hwid">String HwId to check</param>
        /// <returns>true if HwId is Allow, false if HwId is not allow</returns>
        /// <exception cref="System.Exception"> You Have to set sourceOfHwidList field</exception>
        public async Task<bool> HwIdIsAllowedAsync(string Hwid)
        {
            if (null == Hwid)
            {
                throw new Exception("parameters Hwid null");
            }
            var isAllowed = false;

            if (!String.IsNullOrEmpty(this.sourceOfHwidList))
            {
                var stringAllHwId = await ReadUrlAsStringAsync(this.sourceOfHwidList);

                List<string> hwIdList = ManipulateString(stringAllHwId);

                foreach (var HwIdOfList in hwIdList)
                {
                    if (HwIdOfList == Hwid)
                    {
                        isAllowed = true;
                        break;
                    }
                }
            }
            else
            {
                throw new Exception("You Have to set sourceOfHwidList field");
            }

            return isAllowed;
        }

        /// <summary>
        /// read the hardware properties of pc
        /// </summary>
        /// <returns>IEnumerable<String> string list of Hardware Properties</returns>
        private IEnumerable<string> GetHardwareProperties()
        {
            foreach (var properties in
             new Dictionary<string, string[]>
            {
                { "Win32_DiskDrive", new[] { "Model", "Manufacturer", "DeviceID", "SerialNumber" } },
                { "Win32_Processor", new[] { "UniqueId", "ProcessorId", "Name", "Manufacturer" } },
                { "Win32_BaseBoard", new[] { "Model", "Manufacturer", "Name", "SerialNumber" } },
                { "Win32_BIOS",      new[] { "Name", "Manufacturer", "SoftwareElementID", "SerialNumber" } }
            })

            {
                var managementClass = new ManagementClass(properties.Key);
                var managementObject = managementClass.GetInstances().Cast<ManagementBaseObject>().First();

                foreach (var prop in properties.Value)
                {
                    if (null != managementObject[prop])
                        yield return managementObject[prop].ToString();
                }
            }
        }

        /// <summary>
        /// send http request to a specific url only for String Response
        /// </summary>
        /// <param name="url">String source´s url</param>
        /// <returns>String string response to http request</returns>
        /// <exception cref="System.Exception"> You Have to set parameters</exception>
        private async Task<string> ReadUrlAsStringAsync(string url)
        {
            if (null == url)
            {
                throw new Exception("parameters url null");
            }
            try
            {
                var responseBody = await client.GetStringAsync(url);

                return responseBody;
            }
            catch (HttpRequestException e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// send http request to a specific url, that allow to manage any type of response
        /// </summary>
        /// <param name="url">String source´s url</param>
        /// <returns>HttpResponseMessage generic response to http request</returns>
        /// <exception cref="System.Exception"> You Have to set parameters</exception>
        private async Task<HttpResponseMessage> ReadUrlAsync(string url)
        {
            if (null == url)
            {
                throw new Exception("parameters url null");
            }
            try
            {
                var responseBody = await client.GetAsync(url);

                return responseBody;
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// generate a list of string from a unique string
        /// </summary>
        /// <param name="stringOfAllHwid">String string to split</param>
        /// <returns>List<String> list of string</returns>
        /// <exception cref="System.Exception"> You Have to set parameters</exception>
        private List<String> ManipulateString(string stringOfAllHwid)
        {
            if (null == stringOfAllHwid)
            {
                throw new Exception("parameters stringOfAllHwid null");
            }
            char[] charSepator = { '\n' };
            char[] charsToRemove = { '\r' };
            foreach (char c in charsToRemove)
            {
                stringOfAllHwid = stringOfAllHwid.Replace(c.ToString(), String.Empty);
            }

            return stringOfAllHwid.Split(charSepator).ToList();
        }
    }
}