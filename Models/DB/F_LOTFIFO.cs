﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BusinessWeb.Models.DB;

public partial class F_LOTFIFO
{
    public string AR_Ref { get; set; }

    public byte[] cbAR_Ref { get; set; }

    public int? AG_No1 { get; set; }

    public int? AG_No2 { get; set; }

    public decimal? LF_Qte { get; set; }

    public decimal? LF_QteRestant { get; set; }

    public short? LF_LotEpuise { get; set; }

    public int DE_No { get; set; }

    public int? DL_NoIn { get; set; }

    public int? DL_NoOut { get; set; }

    public short? LF_MvtStock { get; set; }

    public DateTime? LF_DateBL { get; set; }

    public short? cbProt { get; set; }

    public int cbMarq { get; set; }

    public string cbCreateur { get; set; }

    public DateTime? cbModification { get; set; }

    public int? cbReplication { get; set; }

    public short? cbFlag { get; set; }

    public virtual F_ARTICLE AR_RefNavigation { get; set; }

    public virtual F_DEPOT DE_NoNavigation { get; set; }
}