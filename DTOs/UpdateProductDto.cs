namespace tasl_Net.DTOs;

public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}