﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BusinessWeb.Models.DB;

public partial class API_V_AGENCEBOOKING
{
    public int id { get; set; }

    public DateTime? Date { get; set; }

    public string Piece { get; set; }

    public DateTime? SellingDateStart { get; set; }

    public DateTime? SellingDateEnd { get; set; }

    public DateTime? ArrivalDateStart { get; set; }

    public DateTime? ArrivalDateEnd { get; set; }

    public string Libelle { get; set; }

    public decimal NbrPax { get; set; }

    public decimal NbrPaxAdult { get; set; }

    public decimal NbrPaxChild { get; set; }

    public string Reference { get; set; }

    public int Prestation { get; set; }

    public int Client { get; set; }

    public int Fournisseur { get; set; }

    public decimal PU { get; set; }

    public decimal PUDevise { get; set; }

    public int Devise { get; set; }

    public decimal Taux { get; set; }

    public decimal NbrPaxInfant { get; set; }

    public string ClientIntitule { get; set; }

    public string FournisseurIntitule { get; set; }
}