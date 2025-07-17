namespace Pix
{
    /// <summary>
    /// Resultado da geração do payload Pix.
    /// </summary>
    public class PixPayloadResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Payload { get; set; }
    }
}