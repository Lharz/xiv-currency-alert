using CurrencyAlert.Enum;
using CurrencyAlert.Helper;
using ImGuiScene;

namespace CurrencyAlert.Entity
{
    internal class Currency
    {
        public int Id { get; }
        public string Name { get; }
        public CurrencyType Type { get; }
        public Category Category { get; }
        public TextureWrap? Image { get; }
        public int DefaultThreshold { get; }

        public Currency(int id, string name, CurrencyType type, Category category, int defaultThreshold)
        {
            Id = id;
            Name = name;
            Type = type;
            Category = category;
            DefaultThreshold = defaultThreshold;
            Image = ImageHelper.LoadImage(id.ToString());
        }
    }
}
