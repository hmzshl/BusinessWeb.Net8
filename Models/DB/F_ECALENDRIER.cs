﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BusinessWeb.Models.DB;

public partial class F_ECALENDRIER
{
    public int? CAL_No { get; set; }

    public DateTime? ECAL_Date { get; set; }

    public string ECAL_Motif { get; set; }

    public short? cbProt { get; set; }

    public int cbMarq { get; set; }

    public string cbCreateur { get; set; }

    public DateTime? cbModification { get; set; }

    public int? cbReplication { get; set; }

    public short? cbFlag { get; set; }

    public virtual F_CALENDRIER CAL_NoNavigation { get; set; }
}