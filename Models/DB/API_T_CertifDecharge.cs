﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BusinessWeb.Models.DB;

public partial class API_T_CertifDecharge
{
    public int id { get; set; }

    public string NumeroFiche { get; set; }

    public DateTime DateFiche { get; set; }

    public string NumeroDossier { get; set; }

    public string NumeroFicheReceptionCorrespondante { get; set; }

    public string SoussigneNom { get; set; }

    public string SocieteNom { get; set; }

    public int? ReceptionID { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}