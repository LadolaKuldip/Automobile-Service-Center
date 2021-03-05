namespace ASC.Entities
{
    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; }
    }
}
