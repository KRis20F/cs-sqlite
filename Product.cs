public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public Product()
    {
        Name = string.Empty;
    }

    public override string ToString()
    {
        return $"ID: {Id}, Nombre: {Name}, Precio: ${Price:F2}, Cantidad: {Quantity}";
    }
}