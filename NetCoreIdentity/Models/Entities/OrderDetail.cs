namespace NetCoreIdentity.Models.Entities
{
    public class OrderDetail : BaseEntity
    {
        public int ProductID { get; set; }
        public int OrderID { get; set; }

        //Relational Properties

        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }

    }
}
