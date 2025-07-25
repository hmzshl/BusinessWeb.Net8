﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BusinessWeb.Models.DB;

public partial class API_V_AGENCECONTRAT
{
    public int id { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? DateDebut { get; set; }

    public DateTime? DateFin { get; set; }

    public int Tiers { get; set; }

    public string CT_Num { get; set; }

    public int Statut { get; set; }

    public int Devise { get; set; }

    public decimal Taux { get; set; }

    public string Piece { get; set; }

    public string Libelle { get; set; }

    public string Fichier { get; set; }

    public int Type { get; set; }

    public string Reference { get; set; }

    public int KidAgeEnd { get; set; }

    public int Regime { get; set; }

    public int InfantStart { get; set; }

    public int InfantEnd { get; set; }

    public int ChildStart { get; set; }

    public int ChildEnd { get; set; }

    public int AdultStart { get; set; }

    public decimal RemiseInfant { get; set; }

    public decimal RemiseChild { get; set; }

    public decimal Tax { get; set; }

    public string CT_Intitule { get; set; }

    public string D_Intitule { get; set; }
}