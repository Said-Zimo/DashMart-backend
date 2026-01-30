using DashMart.Domain.Abstraction;


namespace DashMart.Domain.Products
{
    public sealed class ProductImageGallery : Entity
    {
        public int ProductId { get; private set; }
        public Product Product { get; private set; } = default!;
        public string ImagePath { get; private set; } = default!;
        public bool IsMain { get; private set; }

        private ProductImageGallery( string imagePath, bool isMain)
        {
            if (string.IsNullOrWhiteSpace(imagePath)) throw new DomainException("Image path is null or empty");

            ImagePath = imagePath;
            IsMain = isMain;
        }

        private ProductImageGallery() { }

        public static ProductImageGallery Create(string imagePath, bool isMain)
        {
            return new ProductImageGallery(imagePath, isMain);
        }

        public void Update(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath)) throw new DomainException("Image path is null or empty");

            ImagePath = imagePath;
        }

        public void SetAsMain(bool isMain)
        {
            IsMain = isMain;
        }

    }
}
