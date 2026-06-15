namespace ShipFood.API.Services.UI
{
    // The Abstract Factory Interface
    public interface IUIFactory
    {
        string CreateButton(string text, string url);
    }

    // Concrete Factory 1: Creates Primary (Blue/Green) Buttons
    public class PrimaryUIFactory : IUIFactory
    {
        public string CreateButton(string text, string url)
        {
            return $"<a href='{url}' class='btn btn-primary btn-sm'>{text}</a>";
        }
    }

    // Concrete Factory 2: Creates Danger (Red) Buttons
    public class DangerUIFactory : IUIFactory
    {
        public string CreateButton(string text, string url)
        {
            return $"<a href='{url}' class='btn btn-danger btn-sm'>{text}</a>";
        }
    }
}
