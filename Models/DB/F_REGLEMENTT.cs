﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BusinessWeb.Models.DB;

public partial class F_REGLEMENTT
{
    public string CT_Num { get; set; }

    public byte[] cbCT_Num { get; set; }

    public short? N_Reglement { get; set; }

    public short? RT_Condition { get; set; }

    public short? RT_NbJour { get; set; }

    public short? RT_JourTb01 { get; set; }

    public short? RT_JourTb02 { get; set; }

    public short? RT_JourTb03 { get; set; }

    public short? RT_JourTb04 { get; set; }

    public short? RT_JourTb05 { get; set; }

    public short? RT_JourTb06 { get; set; }

    public short? RT_TRepart { get; set; }

    public decimal? RT_VRepart { get; set; }

    public short? cbProt { get; set; }

    public int cbMarq { get; set; }

    public string cbCreateur { get; set; }

    public DateTime? cbModification { get; set; }

    public int? cbReplication { get; set; }

    public short? cbFlag { get; set; }

    public virtual F_COMPTET CT_NumNavigation { get; set; }
}