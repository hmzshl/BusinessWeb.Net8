﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BusinessWeb.Models.DB;

public partial class F_REGLEARCHIVE
{
    public string TA_Piece { get; set; }

    public byte[] cbTA_Piece { get; set; }

    public decimal? RA_Montant { get; set; }

    public short? N_Devise { get; set; }

    public decimal? RA_MontantDev { get; set; }

    public short? N_Reglement { get; set; }

    public DateTime? RA_Date { get; set; }

    public int? CA_No { get; set; }

    public int? cbCA_No { get; set; }

    public short? cbProt { get; set; }

    public int cbMarq { get; set; }

    public string cbCreateur { get; set; }

    public DateTime? cbModification { get; set; }

    public int? cbReplication { get; set; }

    public short? cbFlag { get; set; }

    public virtual F_TICKETARCHIVE TA_PieceNavigation { get; set; }

    public virtual F_CAISSE cbCA_NoNavigation { get; set; }
}