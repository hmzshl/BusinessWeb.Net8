﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BusinessWeb.Models.DB;

public partial class F_REGLECH
{
    public int RG_No { get; set; }

    public int DR_No { get; set; }

    public short? DO_Domaine { get; set; }

    public short? DO_Type { get; set; }

    public string DO_Piece { get; set; }

    public byte[] cbDO_Piece { get; set; }

    public decimal? RC_Montant { get; set; }

    public short? RG_TypeReg { get; set; }

    public short? cbProt { get; set; }

    public int cbMarq { get; set; }

    public string cbCreateur { get; set; }

    public DateTime? cbModification { get; set; }

    public int? cbReplication { get; set; }

    public short? cbFlag { get; set; }

    public virtual F_DOCREGL DR_NoNavigation { get; set; }

    public virtual F_CREGLEMENT RG_NoNavigation { get; set; }
}