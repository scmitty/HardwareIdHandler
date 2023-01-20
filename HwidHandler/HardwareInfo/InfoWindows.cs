namespace HwidHandler.HardwareInfo
{
    internal static class InfoWindows
    {
        /// <summary>
        /// Get Cpu Id from pc Windows
        /// </summary>
        /// <returns>Return Cpu Id</returns>
        public static string CpuId()
        {
            string retVal = Identifier("Win32_Processor", "UniqueId");
            if (retVal == "") //If no UniqueID, use ProcessorID
            {
                retVal = Identifier("Win32_Processor", "ProcessorId");
                if (retVal == "") //If no ProcessorId, use Name
                {
                    retVal = Identifier("Win32_Processor", "Name");
                    if (retVal == "") //If no Name, use Manufacturer
                    {
                        retVal = Identifier("Win32_Processor", "Manufacturer");
                    }
                    //Add clock speed for extra security
                    retVal += Identifier("Win32_Processor", "MaxClockSpeed");
                }
            }
            return retVal;
        }

        /// <summary>
        /// Get Mother Board Id from pc Windows
        /// </summary>
        /// <returns>Return Mother Board Id</returns>
        public static string BoardId()
        {
            return Identifier("Win32_BaseBoard", "Model")
            + Identifier("Win32_BaseBoard", "Manufacturer")
            + Identifier("Win32_BaseBoard", "Name")
            + Identifier("Win32_BaseBoard", "SerialNumber");
        }

        /// <summary>
        /// Get Bios Id from pc Windows
        /// </summary>
        /// <returns>Return Bios Id</returns>
        public static string BiosId()
        {
            return Identifier("Win32_BIOS", "Manufacturer")
            + Identifier("Win32_BIOS", "SMBIOSBIOSVersion")
            + Identifier("Win32_BIOS", "IdentificationCode")
            + Identifier("Win32_BIOS", "SerialNumber")
            + Identifier("Win32_BIOS", "ReleaseDate")
            + Identifier("Win32_BIOS", "Version");
        }

        /// <summary>
        /// Get Drive Disk Id from pc Windows
        /// </summary>
        /// <returns>Return Drive Disk Id</returns>
        public static string DiskId()
        {
            return Identifier("Win32_DiskDrive", "Model")
            + Identifier("Win32_DiskDrive", "Manufacturer")
            + Identifier("Win32_DiskDrive", "Signature")
            + Identifier("Win32_DiskDrive", "TotalHeads");
        }

        /// <summary>
        /// Get Network Card Id from pc Windows
        /// </summary>
        /// <returns>Return Network Card Id</returns>
        public static string NetworkId()
        {
            return Identifier("Win32_NetworkAdapterConfiguration",
                "MACAddress");
        }

        private static string Identifier(string wmiClass, string wmiProperty)
        {
            string result = "";
            System.Management.ManagementClass mc =
            new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                if (result == "")
                {
                    try
                    {
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                    catch
                    {
                    }
                }
            }
            return result;
        }
    }
}
