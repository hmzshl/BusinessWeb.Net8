﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BusinessWeb.Models.DB;

public partial class API_T_CertifInstrument
{
    public int id { get; set; }

    public string Intitule { get; set; }

    public string IdentificationInterne { get; set; }

    public string Etat { get; set; }

    public string Remarque { get; set; }

    public virtual ICollection<API_T_CertifOrdreMissionLigne> API_T_CertifOrdreMissionLigne { get; set; } = new List<API_T_CertifOrdreMissionLigne>();

    public virtual ICollection<API_T_CertifRapportMissionLigne> API_T_CertifRapportMissionLigne { get; set; } = new List<API_T_CertifRapportMissionLigne>();
}