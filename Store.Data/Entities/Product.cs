namespace Store.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ManufacturerId { get; set; }
        public decimal Price { get; set; }


        #region Navigation properties

        public virtual Manufacturer Manufacturer { get; set; }
        #endregion
    }
}
