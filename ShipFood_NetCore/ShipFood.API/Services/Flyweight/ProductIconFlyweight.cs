using System.Collections.Concurrent;

namespace ShipFood.API.Services.Flyweight
{
    // The Flyweight Interface
    public interface IProductIcon
    {
        string Render();
    }

    // Concrete Flyweight (Shared State)
    public class BadgeIcon(string label, string cssClass) : IProductIcon
    {
        private readonly string _label = label;
        private readonly string _cssClass = cssClass;

        public string Render()
        {
            // This HTML string is shared, saving memory instead of allocating it per product
            return $"<span class='badge {_cssClass}'>{_label}</span>";
        }
    }

    // The Flyweight Factory
    public class IconFlyweightFactory
    {
        private readonly ConcurrentDictionary<string, IProductIcon> _icons = new();

        public IProductIcon GetIcon(string type)
        {
            return _icons.GetOrAdd(type, key =>
            {
                // Instantiate only ONCE per type
                return key switch
                {
                    "New" => new BadgeIcon("MỚI", "bg-warning text-dark"),
                    "Hot" => new BadgeIcon("HOT", "bg-danger"),
                    "Sale" => new BadgeIcon("GIẢM", "bg-success"),
                    _ => new BadgeIcon("", "d-none")
                };
            });
        }
    }
}
