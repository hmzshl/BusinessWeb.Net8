﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BusinessWeb.Models.DB;

public partial class F_FAMFOURNISS
{
    public string FA_CodeFamille { get; set; }

    public byte[] cbFA_CodeFamille { get; set; }

    public string CT_Num { get; set; }

    public byte[] cbCT_Num { get; set; }

    public short? FF_Unite { get; set; }

    public decimal? FF_Conversion { get; set; }

    public short? FF_DelaiAppro { get; set; }

    public short? FF_Garantie { get; set; }

    public decimal? FF_Colisage { get; set; }

    public decimal? FF_QteMini { get; set; }

    public short? FF_QteMont { get; set; }

    public short? EG_Champ { get; set; }

    public short? FF_Principal { get; set; }

    public short? FF_Devise { get; set; }

    public decimal? FF_Remise { get; set; }

    public decimal? FF_ConvDiv { get; set; }

    public short? FF_TypeRem { get; set; }

    public short? cbProt { get; set; }

    public int cbMarq { get; set; }

    public string cbCreateur { get; set; }

    public DateTime? cbModification { get; set; }

    public int? cbReplication { get; set; }

    public short? cbFlag { get; set; }

    public virtual F_COMPTET CT_NumNavigation { get; set; }
}