namespace HwidHandler
{
    public static class HardwareIdHandler
    {
       private static HardwareId HardwareId = new HardwareId();

        /// <summary>
        /// The Method generate a Hardware Id as unique ID
        /// </summary>
        /// <returns>string: Return a Hardware Id</returns>
        /// <exception cref="Exception"></exception>
        public static string GenerateHwid()
        {
            try
            {
                return HardwareId.GenerateHwId();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// The Method try to get and set Hardware Properties base on the Operating System
        /// </summary>
        /// <returns>bool: true if the operation have success or false if the operation fail</returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="Exception"></exception>
        public static bool SetHardwareProperties()
        {
            try
            {
                HardwareId.SetHardwareProperties();
                return true;
            }
            catch (NotImplementedException eImplemented)
            {
                throw new NotImplementedException(eImplemented.Message);
            }
            catch (NotSupportedException eSupported)
            {
                throw new NotSupportedException(eSupported.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
