public partial class Program
{
    private static readonly DatabaseManager _db = new DatabaseManager("products.db");

    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== SISTEMA DE GESTIÓN DE PRODUCTOS ===");
            Console.WriteLine("1. Crear Producto");
            Console.WriteLine("2. Listar Productos");
            Console.WriteLine("3. Buscar Producto por Nombre");
            Console.WriteLine("4. Actualizar Producto");
            Console.WriteLine("5. Eliminar Producto");
            Console.WriteLine("6. Salir");
            Console.Write("\nSeleccione una opción: ");

            try
            {
                int option = int.Parse(Console.ReadLine() ?? "0");
                Console.Clear();
                switch (option)
                {
                    case 1:
                        CreateProduct();
                        break;
                    case 2:
                        ListProducts();
                        break;
                    case 3:
                        SearchProducts();
                        break;
                    case 4:
                        UpdateProduct();
                        break;
                    case 5:
                        DeleteProduct();
                        break;
                    case 6:
                        return;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Por favor, ingrese un número válido.");
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }

    private static void CreateProduct()
    {
        Console.WriteLine("=== CREAR PRODUCTO ===\n");

        try
        {
            Console.Write("Nombre del producto: ");
            string name = Console.ReadLine() ?? throw new Exception("El nombre no puede estar vacío");

            Console.Write("Precio del producto: ");
            decimal price = decimal.Parse(Console.ReadLine() ?? "0");

            Console.Write("Cantidad del producto: ");
            int quantity = int.Parse(Console.ReadLine() ?? "0");

            var product = new Product
            {
                Name = name,
                Price = price,
                Quantity = quantity
            };

            _db.InsertProduct(product);
            Console.WriteLine("\nProducto creado exitosamente.");
        }
        catch (FormatException)
        {
            Console.WriteLine("Error: Ingrese valores numéricos válidos para precio y cantidad.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void ListProducts()
    {
        Console.WriteLine("=== LISTA DE PRODUCTOS ===\n");

        var products = _db.GetAllProducts();
        if (products.Count == 0)
        {
            Console.WriteLine("No hay productos registrados.");
            return;
        }

        foreach (var product in products)
        {
            Console.WriteLine(product);
        }
    }

    private static void SearchProducts()
    {
        Console.WriteLine("=== BUSCAR PRODUCTOS ===\n");

        Console.Write("Ingrese el nombre a buscar: ");
        string searchTerm = Console.ReadLine() ?? string.Empty;

        var products = _db.SearchProductsByName(searchTerm);
        if (products.Count == 0)
        {
            Console.WriteLine("No se encontraron productos.");
            return;
        }

        foreach (var product in products)
        {
            Console.WriteLine(product);
        }
    }

    private static void UpdateProduct()
    {
        Console.WriteLine("=== ACTUALIZAR PRODUCTO ===\n");

        try
        {
            Console.Write("Ingrese el ID del producto a actualizar: ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            var products = _db.GetAllProducts();
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                Console.WriteLine("Producto no encontrado.");
                return;
            }

            Console.WriteLine($"Producto actual: {product}");

            Console.Write("\nNuevo nombre (presione Enter para mantener el actual): ");
            string name = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(name))
            {
                product.Name = name;
            }

            Console.Write("Nuevo precio (presione Enter para mantener el actual): ");
            string priceInput = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(priceInput))
            {
                product.Price = decimal.Parse(priceInput);
            }

            Console.Write("Nueva cantidad (presione Enter para mantener la actual): ");
            string quantityInput = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(quantityInput))
            {
                product.Quantity = int.Parse(quantityInput);
            }

            _db.UpdateProduct(product);
            Console.WriteLine("\nProducto actualizado exitosamente.");
        }
        catch (FormatException)
        {
            Console.WriteLine("Error: Ingrese valores numéricos válidos.");
        }
    }

    private static void DeleteProduct()
    {
        Console.WriteLine("=== ELIMINAR PRODUCTO ===\n");

        try
        {
            Console.Write("Ingrese el ID del producto a eliminar: ");
            int id = int.Parse(Console.ReadLine() ?? "0");

            var products = _db.GetAllProducts();
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                Console.WriteLine("Producto no encontrado.");
                return;
            }

            Console.WriteLine($"\nProducto a eliminar: {product}");
            Console.Write("\n¿Está seguro de que desea eliminar este producto? (S/N): ");

            if (Console.ReadLine()?.Trim().ToUpper() == "S")
            {
                _db.DeleteProduct(id);
                Console.WriteLine("\nProducto eliminado exitosamente.");
            }
            else
            {
                Console.WriteLine("\nOperación cancelada.");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Error: Ingrese un ID válido.");
        }
    }
}
