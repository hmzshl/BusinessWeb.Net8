﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BusinessWeb.Models.DB;

public partial class API_T_CertifOrdreMissionLigne
{
    public int id { get; set; }

    public int Ordre { get; set; }

    public int Instrument { get; set; }

    public string Libelle { get; set; }

    public bool Statut { get; set; }

    public int Ligne { get; set; }

    public virtual API_T_CertifInstrument InstrumentNavigation { get; set; }

    public virtual API_T_CertifOrdreMission OrdreNavigation { get; set; }
}