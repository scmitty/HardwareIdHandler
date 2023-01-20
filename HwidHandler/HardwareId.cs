using HwidHandler.HardwareInfo;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace HwidHandler
{
    /// <summary>
    /// this class use to define Hardware Id
    /// </summary>
    internal class HardwareId
    {
        /// <summary>
        /// class field:Number Hardware Id
        /// </summary>
        public readonly string HwId;

        private HardwareProperties HarwareProperties= new HardwareProperties();
        private object swicht;

        /// <summary>
        /// using the computer propriets create a hash string, that allow to have a unique id
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
        /// get the Hardware Info for OS and set the class field
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public void SetHardwareProperties()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                HarwareProperties.CpuId = InfoWindows.CpuId();
                HarwareProperties.BoardId = InfoWindows.BoardId();
                HarwareProperties.BiosId = InfoWindows.BiosId();
                HarwareProperties.DiskId = InfoWindows.DiskId();
                HarwareProperties.NetworkId = InfoWindows.NetworkId();
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new NotImplementedException("Linux is for now not Supported");
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                throw new NotImplementedException("OSX is for now not Supported");
            }

            throw new NotSupportedException("OS not Supported");
        }

        /// <summary>
        /// read the hardware properties 
        /// </summary>
        /// <returns>IEnumerable<String> string list of Hardware Properties</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private IEnumerable<string> GetHardwareProperties()
        {
            if(this.HarwareProperties!=null)
            {
                List<string> hardwareProperties = new List<string>();

                hardwareProperties.Add(this.HarwareProperties.CpuId);
                hardwareProperties.Add(this.HarwareProperties.BoardId);
                hardwareProperties.Add(this.HarwareProperties.BiosId);
                hardwareProperties.Add(this.HarwareProperties.DiskId);
                hardwareProperties.Add(this.HarwareProperties.NetworkId);

                return (IEnumerable<string>)hardwareProperties;
            }
            else
            {
                throw new ArgumentNullException("Harware Properties are not Setted");
            }
        }

    }
}
