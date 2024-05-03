namespace BusinessWeb.Models.Perso
{
    public class data
    {
        public DateTime? do_date1 { get; set; } = new DateTime(2023, 1, 1);
        public DateTime? do_date2 { get; set; } = new DateTime(DateTime.Today.Year, 12, 31);
        public short? do_domaine { get; set; } = 0;
        public short? do_type { get; set; } = 6;
        public int exercice { get; set; } = 2022;
        public string famille { get; set; }
    }
}
