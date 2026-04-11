namespace BusinessWeb.Models.Perso;

public class RFMClientItem
{
    public string CT_Num { get; set; }
    public string CT_Intitule { get; set; }
    public DateTime? DernierAchat { get; set; }
    public int Recence { get; set; }        // days since last order (lower = more recent)
    public int Frequence { get; set; }      // distinct orders count
    public decimal Montant { get; set; }    // total CA HT
    public int ScoreR { get; set; }         // 1–5 (5 = most recent)
    public int ScoreF { get; set; }         // 1–5 (5 = most frequent)
    public int ScoreM { get; set; }         // 1–5 (5 = highest CA)
    public string ScoreRFM => $"{ScoreR}{ScoreF}{ScoreM}";
    public string Segment { get; set; }
    public string SegmentColor { get; set; }
}
