﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace BusinessWeb.Models.DB;

public partial class API_T_PersonnelMateriel
{
    public int id { get; set; }

    public int ParentID { get; set; }

    public int MaterielID { get; set; }

    public DateTime? DateDebut { get; set; }

    public DateTime? DateFin { get; set; }

    public DateTime? Creation { get; set; }

    public DateTime? Modification { get; set; }

    public string CreationIP { get; set; }

    public string ModificationIP { get; set; }

    public string CreationHost { get; set; }

    public string ModificationHost { get; set; }

    public string CreationUser { get; set; }

    public string ModificationUser { get; set; }
}