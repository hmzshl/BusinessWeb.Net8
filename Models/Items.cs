namespace BusinessWeb.Models
{
    public class Items
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public short? ShortId { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
		public string TextID { get; set; }
		public Items(int id, string name)
		{
			Id = id;
			Name = name;
		}
		public Items()
		{

		}
	}
}
