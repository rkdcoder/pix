namespace Pix
{
    /// <summary>
    /// Utilitário interno para cálculo do CRC16 do Pix.
    /// </summary>
    internal static class CRC16
    {
        public static string Compute(string input)
        {
            ushort crc = 0xFFFF;
            foreach (var c in input)
            {
                crc ^= (ushort)(c << 8);
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x8000) != 0)
                        crc = (ushort)((crc << 1) ^ 0x1021);
                    else
                        crc <<= 1;
                    crc &= 0xFFFF;
                }
            }
            return crc.ToString("X4");
        }
    }
}