﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BusinessWeb.Models.DB;

public partial class F_COMPTETMODELE
{
    public string CT_Num { get; set; }

    public byte[] cbCT_Num { get; set; }

    public int? CM_Creator { get; set; }

    public short? CM_Type { get; set; }

    public string CM_Modele { get; set; }

    public short? CM_NbExemplaire { get; set; }

    public short? cbProt { get; set; }

    public int cbMarq { get; set; }

    public string cbCreateur { get; set; }

    public DateTime? cbModification { get; set; }

    public int? cbReplication { get; set; }

    public short? cbFlag { get; set; }

    public virtual F_COMPTET CT_NumNavigation { get; set; }
}