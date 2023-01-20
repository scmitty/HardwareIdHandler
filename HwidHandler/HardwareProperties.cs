namespace HwidHandler
{
    internal class HardwareProperties
    {
        //motherboard info
        public string CpuId { get; set; }

        //cpu info
        public string BoardId { get; set; }

        //diskdrive info
        public string DiskId { get; set; }

        //bios info
        public string BiosId { get; set; }

        //network card info
        public string NetworkId { get; set; }
    }
}
